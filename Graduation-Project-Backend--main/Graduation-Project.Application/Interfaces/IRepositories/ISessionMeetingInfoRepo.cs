using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Interfaces.IRepositories {

    public interface ISessionMeetingInfoRepo : IBaseRepo<SessionMeetingInfoEntity> {

        SessionMeetingInfoEntity? GetBySessionId(int sessionId);
    }
}