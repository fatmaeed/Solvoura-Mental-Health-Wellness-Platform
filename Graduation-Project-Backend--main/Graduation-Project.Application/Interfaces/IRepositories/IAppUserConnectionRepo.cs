using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Interfaces.IRepositories {

    public interface IAppUserConnectionRepo : IBaseRepo<AppUserConnectionEntity> {

        public AppUserConnectionEntity? GetByConnectionId(string connectionId);

        public AppUserConnectionEntity? GetByUserId(int userId);

        public AppUserConnectionEntity? RemoveByConnectionId(string connectionId);
    }
}