using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;

namespace Graduation_Project.Application.Hubs {

    public class MeetingSessionHub : Hub {
        private readonly IMeetingSessionService meetingSessionService;

        public MeetingSessionHub(IMeetingSessionService meetingSessionService) {
            this.meetingSessionService = meetingSessionService;
        }

        public async Task JoinRoom(string userId, string sessionId) {
            var result = await meetingSessionService.JoinRoom(int.Parse(userId), int.Parse(sessionId));

            result.Fold((failure) => { }, async (conn) => {
                await Clients.Caller.SendAsync("ReceiveOffer", conn.Offer);
                await Clients.Caller.SendAsync("ReceiveIceCandidate", conn.IceCandidate);
            });
        }

        public async Task ConnectSession(string userId, string sessionId) {
            var result = await meetingSessionService.ConnectSession(int.Parse(userId), int.Parse(sessionId), Context.ConnectionId);
            result.Fold((failure) => {
                Clients.Caller.SendAsync("Failure", failure.Message);
            }, (isStarted) => {
                Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());

                Clients.Caller.SendAsync("IsStarted", isStarted);
            });
        }

        public async Task StartSession(string userId, string sessionId) {
            var result = await meetingSessionService.StartSession(int.Parse(userId), int.Parse(sessionId), Context.ConnectionId);
            result.Fold((failure) => {
                meetingSessionService.RemoveUserConnection(Context.ConnectionId);
                Clients.Caller.SendAsync("Failure", failure.Message);
            }, (isStarted) => {
                Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());
                Clients.OthersInGroup(sessionId).SendAsync("IsStarted", isStarted);
            });
        }

        public async Task SendOffer(string offer, string sessionId) {
            var result = await meetingSessionService.SendOffer(offer, int.Parse(sessionId));
            result.Fold((failure) => { }, (isStarted) => {
                //await Clients.OthersInGroup(sessionId).SendAsync("ReceiveOffer", offer);
            });
        }

        public async Task SendAnswer(string answer, string sessionId) {
            //var result = await meetingSessionService.SendAnswer(answer, int.Parse(sessionId));
            //result.Fold((failure) => { }, (isStarted) => {
            await Clients.OthersInGroup(sessionId).SendAsync("ReceiveAnswer", answer);
            //});
        }

        public async Task SendIceCandidate(string candidate, string sessionId) {
            var result = await meetingSessionService.SendIceCandidate(candidate, int.Parse(sessionId));
            result.Fold((failure) => { }, async (isStarted) => {
                await Clients.OthersInGroup(sessionId).SendAsync("ReceiveIceCandidate", candidate);
            });
        }

        public async Task EndMeeting(string userId, string roomId) {
            var result = await meetingSessionService.EndRoom(int.Parse(userId), int.Parse(roomId));
            result.Fold((failure) => { }, async (isFinished) => {
                await Clients.Group(roomId).SendAsync("MeetingEnded");
            });
        }

        public override async Task OnConnectedAsync() {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) {
            var result = await meetingSessionService.LeaveRoom(Context.ConnectionId);
            result.Fold((failure) => { }, (sessionId) => {
                Clients.OthersInGroup(sessionId.ToString()).SendAsync("UserLeft");
            });
            await base.OnDisconnectedAsync(exception);
        }
    }
}