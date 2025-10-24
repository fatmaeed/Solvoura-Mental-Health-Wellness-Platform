using Graduation_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Infrastructure.Data {

    public class MentalDbContext : IdentityDbContext<UserEntity, IdentityRole<int>, int> {
        public virtual DbSet<AdminEntity> Admins { get; set; }
        public virtual DbSet<ClientEntity> Clients { get; set; }
        public virtual DbSet<ServiceProviderEntity> ServiceProviders { get; set; }
        public virtual DbSet<CertificateEntity> Certificates { get; set; }
        public virtual DbSet<FeedBackEntity> FeedBacks { get; set; }
        public virtual DbSet<SessionEntity> Sessions { get; set; }
        public virtual DbSet<ReservationEntity> Reservations { get; set; }
        public virtual DbSet<PrescriptionEntity> Prescriptions { get; set; }
        public virtual DbSet<PaymentEntity> Payments { get; set; }
        public virtual DbSet<IllnessEntity> Illnesses { get; set; }
        public virtual DbSet<SymptomEntity> Symptoms { get; set; }
        public virtual DbSet<ServiceEntity> Services { get; set; }
        public virtual DbSet<RelatedToEntity> IllnessRelatedTo { get; set; }
        public virtual DbSet<PostEntity> Posts { get; set; }
        public virtual DbSet<CommentEntity> Comments { get; set; }

        public virtual DbSet<NotificationEntity> Notifications { get; set; }
        public virtual DbSet<SessionUserConnectionEntity> SessionUserConnections { get; set; }
        public virtual DbSet<SessionMeetingInfoEntity> SessionMeetingInfos { get; set; }
        public virtual DbSet<SessionMeetingClientsEntity> SessionMeetingClients { get; set; }
        public virtual DbSet<ConnectionSessionMeetingEntity> ConnectionSessionMeetings { get; set; }
        public virtual DbSet<UserLikes> UserLikes { get; set; }

        public virtual DbSet<AppGroupConnectionEntity> AppGroupConnections { get; set; }
        public virtual DbSet<UserNotificationEntity> UserNotifications { get; set; }
        public virtual DbSet<AppUserConnectionEntity> AppUserConnections { get; set; }

        public MentalDbContext(DbContextOptions<MentalDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<SessionEntity>().Property(x => x.EndDateTime).HasComputedColumnSql("DATEADD(SECOND, DATEDIFF(SECOND, 0, [Duration]), [StartDateTime])", stored: true);
            builder.Entity<ReservationEntity>().Property(x => x.RamainingSessionsNumber).HasComputedColumnSql("SessionsNumber - DoneSessionsNumber", stored: true);
            builder.Entity<ClientEntity>().Property(x => x.IsAnon).HasDefaultValue(false);
            builder.Entity<ClientEntity>().Property(x => x.IsVerified).HasDefaultValue(false);
            builder.Entity<SessionMeetingInfoEntity>()
                .Property(x => x.DurationInMins)
                .HasComputedColumnSql(@"
                                CASE
                                    WHEN [StartSessionTime] IS NOT NULL AND [EndSessionTime] IS NOT NULL
                                    THEN DATEDIFF(MINUTE, [StartSessionTime], [EndSessionTime])
                                    ELSE NULL
                                END", stored: true);
        }
    }
}