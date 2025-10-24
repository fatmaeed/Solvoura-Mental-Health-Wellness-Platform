namespace Graduation_Project.Domain.Entities {

    public class ServiceEntity : BaseEntity {
        public required string Title { get; set; }

        public required string Icon { get; set; }
        public required string Description { get; set; }
    }
}