namespace Graduation_Project.Application.DTOs.NotificationDTOs {

    public class DisplayNotificationDTO : NotificationDTO {
        public int Id { get; set; }
        public DateTime NotificationTime { get; set; }

        public string? SenderName { get; set; }
    }
}