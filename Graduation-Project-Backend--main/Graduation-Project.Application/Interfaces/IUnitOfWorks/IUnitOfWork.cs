using Graduation_Project.Application.Interfaces.IRepositories;

namespace Graduation_Project.Application.Interfaces.IUnitOfWorks {

    public interface IUnitOfWork {

        Task SaveChangesAsync();

        public IIllnessRepo IllnessRepo { get; }

        public IAdminRepo AdminRepo { get; }
        public ICertificateRepo CertificateRepo { get; }
        public IClientRepo ClientRepo { get; }
        public IPrescriptionRepo PrescriptionRepo { get; }
        public IPaymentRepo PaymentRepo { get; }
        public IReservationRepo ReservationRepo { get; }
        public ISessionRepo SessionRepo { get; }
        public IServiceProviderRepo ServiceProviderRepo { get; }
        public IFeedbackRepo FeedbackRepo { get; }
        public ISymptomRepo SymptomRepo { get; }
        IServiceRepo ServiceRepo { get; }
        public IPostRepo PostRepo { get; }
        public ICommentRepo CommentRepo { get; }

        public ISessionMeetingInfoRepo SessionMeetingInfoRepo { get; }
        public IConnectionSessionMeetingRepo ConnectionSessionMeetingRepo { get; }
        public ISessionUserConnectionRepo SessionUserConnectionRepo { get; }

        public ISessionMeetingClientsRepo SessionMeetingClientsRepo { get; }

        public IAppGroupConnectionRepo AppGroupConnectionRepo { get; }
        public IAppUserConnectionRepo AppUserConnectionRepo { get; }

        public INotificationRepo NotificationRepo { get; }
        public IUserNotificationRepo UserNotificationRepo { get; }

        public IUserRepo UserRepo { get; }
        public IUserLikesRepo UserLikesRepo { get; }
    }
}