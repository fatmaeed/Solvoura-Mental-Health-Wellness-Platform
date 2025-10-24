namespace Graduation_Project.Application.DTOs.SessionDTOs {

    public class BaseConnectionSessionMeetingDTO {
        public required string ConnectionCreatorId { get; set; }
        public required string IceCandidate { get; set; }
        public required string Offer { get; set; }
        public required string Answer { get; set; }
        public int SessionId { get; set; }
    }
}