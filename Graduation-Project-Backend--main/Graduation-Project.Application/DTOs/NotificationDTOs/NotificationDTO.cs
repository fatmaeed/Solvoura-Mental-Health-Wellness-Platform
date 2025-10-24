namespace Graduation_Project.Application.DTOs.NotificationDTOs {

    public class NotificationDTO {
        public required string Title { get; set; }

        public bool Readed { get; set; } = false;

        public required string Message { get; set; }

        public required string Type { get; set; }

        public string? Routing { get; set; }

        public int? SenderId { get; set; }
    }
}