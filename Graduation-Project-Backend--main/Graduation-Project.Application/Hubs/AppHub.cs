using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;

namespace Graduation_Project.Application.Hubs {

    public class AppHub : Hub {
        private readonly IAppHubService appHubService;

        public AppHub(IAppHubService appHubService) {
            this.appHubService = appHubService;
        }

        public async Task RegisterUser(string userId) {
            if (userId == null) {
                return;
            }

            await appHubService.RegisterUser(int.Parse(userId), Context.ConnectionId);
        }

        public async Task SendNotificationToAllSystem() {
            await Clients.All.SendAsync("ReceiveNotification");
        }

        public async Task SendNotificationToUser(int userId, CreateNotificationDTO notification) {
            var result = await appHubService.SendNotificationToUser(userId, notification);
            result.Fold((failure) => { }, async (notificationToDTO) => {
                await Clients.User(notificationToDTO.ConnectionId).SendAsync("ReceiveNotification", notificationToDTO.Notification);
            });
        }

        public async Task SendNotificationToGroup(string groupId) {
            await Clients.Group(groupId).SendAsync("ReceiveNotification");
        }

        public override Task OnConnectedAsync() {
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception) {
            await appHubService.UnRegisterUser(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}