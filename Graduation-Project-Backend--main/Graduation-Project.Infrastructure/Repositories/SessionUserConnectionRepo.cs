using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories {

    public class SessionUserConnectionRepo : BaseRepo<SessionUserConnectionEntity>, ISessionUserConnectionRepo {

        public SessionUserConnectionRepo(MentalDbContext mentalDbContext) : base(mentalDbContext) {
        }

        public List<SessionUserConnectionEntity> GetConnentionsByUserId(int userId) {
            return mentalDbContext.Set<SessionUserConnectionEntity>().Where(x => x.UserId == userId && !x.IsDeleted).ToList();
        }
        public List<SessionUserConnectionEntity> RemoveBySessionId(int sessionId) {
            var entities = mentalDbContext.Set<SessionUserConnectionEntity>().Where(x => x.SessionId == sessionId && !x.IsDeleted).ToList();
            foreach (var entity in entities) {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.Now;
            }
            return entities;
        }
        public SessionUserConnectionEntity? RemoveByConnectionId(string connectionId) {
            var entity = mentalDbContext.Set<SessionUserConnectionEntity>().FirstOrDefault(x => x.ConnectionId == connectionId && !x.IsDeleted);
            if (entity == null) {
                return entity;
            }

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
            return entity;
        }
    }
}