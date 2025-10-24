namespace Graduation_Project.Application.DTOs.SessionDTOs {

    public class DisplayMeetingSessionDTO {
        public int SessionId { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndDateTime { get; set; }

        public required string Status { get; set; }
        public required string DoctorName { get; set; }
        public required string PatientName { get; set; }
    }
}