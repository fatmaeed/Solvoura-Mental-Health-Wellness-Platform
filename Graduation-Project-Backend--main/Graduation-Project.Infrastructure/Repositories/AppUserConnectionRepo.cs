using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories {

    public class AppUserConnectionRepo : BaseRepo<AppUserConnectionEntity>, IAppUserConnectionRepo {

        public AppUserConnectionRepo(MentalDbContext mentalDbContext) : base(mentalDbContext) {
        }

        public AppUserConnectionEntity? GetByConnectionId(string connectionId) {
            return mentalDbContext.Set<AppUserConnectionEntity>().FirstOrDefault(x => x.ConnectionId == connectionId && !x.IsDeleted);
        }

        public AppUserConnectionEntity? GetByUserId(int userId) {
            return mentalDbContext.Set<AppUserConnectionEntity>().FirstOrDefault(x => x.UserId == userId && !x.IsDeleted);
        }

        public AppUserConnectionEntity? RemoveByConnectionId(string connectionId) {
            var entity = mentalDbContext.Set<AppUserConnectionEntity>().FirstOrDefault(x => x.ConnectionId == connectionId && !x.IsDeleted);
            if (entity == null) {
                return entity;
            }
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
            return entity;
        }
    }
}