using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Infrastructure.Data;
using Graduation_Project.Infrastructure.Repositories;

namespace Graduation_Project.Infrastructure.UnitOfWorks {
    public class UnitOfWork(MentalDbContext context) : IUnitOfWork {
        private readonly MentalDbContext context = context;
        private readonly IAdminRepo? adminRepo;
        private readonly ICertificateRepo? certificateRepo;
        private readonly IClientRepo? clientRepo;
        private readonly IPrescriptionRepo? prescriptionRepo;
        private readonly IPaymentRepo? paymentRepo;
        private readonly IReservationRepo? reservationRepo;
        private readonly ISessionRepo? sessionRepo;
        private readonly IServiceProviderRepo? serviceProviderRepo;
        private readonly IFeedbackRepo? feedbackRepo;
        private readonly IServiceRepo? serviceRepo;

        private readonly IIllnessRepo? illnessRepo;
        private readonly ISymptomRepo? symptomRepo;
        private readonly IPostRepo? postRepo;
        private readonly ICommentRepo? commentRepo;

        private readonly ISessionUserConnectionRepo? sessionUserConnectionRepo;
        private readonly IConnectionSessionMeetingRepo? connectionSessionMeetingRepo;
        private readonly ISessionMeetingInfoRepo? sessionMeetingInfoRepo;
        private readonly ISessionMeetingClientsRepo? sessionMeetingClientsRepo;
        private readonly IAppUserConnectionRepo? appUserConnectionRepo;
        private readonly IAppGroupConnectionRepo? appGroupConnectionRepo;
        private readonly INotificationRepo? notificationRepo;
        private readonly IUserNotificationRepo? userNotificationRepo;
        private readonly IUserRepo? userRepo;
        private readonly IUserLikesRepo? userLikesRepo;

        public async Task SaveChangesAsync() {
            await context.SaveChangesAsync();
        }

        public IAdminRepo AdminRepo => adminRepo ?? new AdminRepo(context);
        public ICertificateRepo CertificateRepo => certificateRepo ?? new CertificateRepo(context);
        public IClientRepo ClientRepo => clientRepo ?? new ClientRepo(context);
        public IPrescriptionRepo PrescriptionRepo => prescriptionRepo ?? new PrescriptionRepo(context);
        public IPaymentRepo PaymentRepo => paymentRepo ?? new PaymentRepo(context);
        public IReservationRepo ReservationRepo => reservationRepo ?? new ReservationRepo(context);
        public ISessionRepo SessionRepo => sessionRepo ?? new SessionRepo(context);
        public IServiceProviderRepo ServiceProviderRepo => serviceProviderRepo ?? new ServiceProviderRepo(context);
        public IFeedbackRepo FeedbackRepo => feedbackRepo ?? new FeedbackRepo(context);

        public IServiceRepo ServiceRepo => serviceRepo ?? new ServiceRepo(context);

        public IIllnessRepo IllnessRepo => illnessRepo ?? new IllnessRepo(context);
        public ISymptomRepo SymptomRepo => symptomRepo ?? new SymptomRepo(context);
        public IPostRepo PostRepo => postRepo ?? new PostRepo(context);
        public ICommentRepo CommentRepo => commentRepo ?? new CommentRepo(context);

        public ISessionUserConnectionRepo SessionUserConnectionRepo => sessionUserConnectionRepo ?? new SessionUserConnectionRepo(context);

        public ISessionMeetingInfoRepo SessionMeetingInfoRepo => sessionMeetingInfoRepo ?? new SessionMeetingInfoRepo(context);

        public IConnectionSessionMeetingRepo ConnectionSessionMeetingRepo => connectionSessionMeetingRepo ?? new ConnectionSessionMeetingRepo(context);

        public ISessionMeetingClientsRepo SessionMeetingClientsRepo => sessionMeetingClientsRepo ?? new SessionMeetingClientsRepo(context);

        public IAppGroupConnectionRepo AppGroupConnectionRepo => appGroupConnectionRepo ?? new AppGroupConnectionRepo(context);

        public IAppUserConnectionRepo AppUserConnectionRepo => appUserConnectionRepo ?? new AppUserConnectionRepo(context);

        public INotificationRepo NotificationRepo => notificationRepo ?? new NotificationRepo(context);

        public IUserNotificationRepo UserNotificationRepo => userNotificationRepo ?? new UserNotificationRepo(context);

        public IUserRepo UserRepo => userRepo ?? new UserRepo(context);
        public IUserLikesRepo UserLikesRepo => userLikesRepo ?? new UserLikesRepo(context);
    }
}