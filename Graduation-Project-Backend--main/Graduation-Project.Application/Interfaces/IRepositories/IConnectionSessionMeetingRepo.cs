using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Interfaces.IRepositories {

    public interface IConnectionSessionMeetingRepo : IBaseRepo<ConnectionSessionMeetingEntity> {

        ConnectionSessionMeetingEntity? RemoveBySessionId(int sessionId);

        ConnectionSessionMeetingEntity? RemoveByConnectionId(string connectionId);

        ConnectionSessionMeetingEntity? GetBySessionId(int sessionId);
    }
}