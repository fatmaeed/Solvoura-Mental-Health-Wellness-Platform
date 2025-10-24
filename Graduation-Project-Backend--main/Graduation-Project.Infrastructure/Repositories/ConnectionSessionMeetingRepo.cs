using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories {

    public class ConnectionSessionMeetingRepo : BaseRepo<ConnectionSessionMeetingEntity>, IConnectionSessionMeetingRepo {

        public ConnectionSessionMeetingRepo(MentalDbContext mentalDbContext) : base(mentalDbContext) {
        }

        public ConnectionSessionMeetingEntity? GetBySessionId(int sessionId) {
            return mentalDbContext.Set<ConnectionSessionMeetingEntity>().FirstOrDefault(x => x.SessionId == sessionId && !x.IsDeleted);
        }

        public ConnectionSessionMeetingEntity? RemoveByConnectionId(string connectionId) {
            var entity = mentalDbContext.Set<ConnectionSessionMeetingEntity>().FirstOrDefault(x => x.ConnectionCreatorId == connectionId);
            if (entity == null) {
                return entity;
            }
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
            return entity;
        }

        public ConnectionSessionMeetingEntity? RemoveBySessionId(int sessionId) {
            var entity = mentalDbContext.Set<ConnectionSessionMeetingEntity>().FirstOrDefault(x => x.SessionId == sessionId);
            if (entity == null) {
                return entity;
            }
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
            return entity;
        }
    }
}