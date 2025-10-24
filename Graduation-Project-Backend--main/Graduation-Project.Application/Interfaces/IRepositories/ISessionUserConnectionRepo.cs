using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Interfaces.IRepositories {

    public interface ISessionUserConnectionRepo : IBaseRepo<SessionUserConnectionEntity> {

        public List<SessionUserConnectionEntity> GetConnentionsByUserId(int userId);

        public SessionUserConnectionEntity? RemoveByConnectionId(string connectionId);
        public List<SessionUserConnectionEntity> RemoveBySessionId(int sessionId);
    }
}