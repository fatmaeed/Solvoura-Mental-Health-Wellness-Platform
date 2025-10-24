using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Hubs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;

namespace Graduation_Project.Application.Utils {

    public class NotificationHandler : INotificationService {
        private readonly IHubContext<AppHub> hubContext;
        private readonly IAppHubService appHubService;

        public NotificationHandler(IHubContext<AppHub> hubContext, IAppHubService appHubService) {
            this.hubContext = hubContext;
            this.appHubService = appHubService;
        }

        public async Task SendNotification(int userId, CreateNotificationDTO notification) {
            var result = await appHubService.SendNotificationToUser(userId, notification);
            result.Fold((failure) => { }, async (notificationToDTO) => {
                await hubContext.Clients.Client(notificationToDTO.ConnectionId).SendAsync("ReceiveNotification", notificationToDTO.Notification);
            });
        }
        public async Task SendNotificationToAll(CreateNotificationDTO notification) {
            var result = await appHubService.SendNotificationToAllUsers(notification);
            result.Fold((failure) => { }, async (notification) => {
                await hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
            });
        }
    }
}