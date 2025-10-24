using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Utils;

namespace Graduation_Project.Application.Interfaces.IServices {

    public interface IMeetingSessionService {

        Task SaveChangesAsync();

        Either<Failure, SessionUserConnectionDTO> AddUserConnection(int userId, string connectionId);

        Either<Failure, List<SessionUserConnectionDTO>> GetUserConnectionId(int userId);

        Either<Failure, SessionUserConnectionDTO> RemoveUserConnection(string connectionId);

        Either<Failure, DisplayConnectionSessionMeetingDTO> GetConnectionBySessionId(int sessionId);

        Either<Failure, CreateConnectionSessionMeetingDTO> AddConnectionSession(CreateConnectionSessionMeetingDTO createConnectionSessionMeetingDTO);

        Either<Failure, DisplayConnectionSessionMeetingDTO> RemoveConnectionSession(int sessionId);

        //------------------------
        public Task<Either<Failure, bool>> ConnectSession(int userId, int sessionId, string connectionId);

        public Task<Either<Failure, bool>> StartSession(int userId, int sessionId, string connectionId);

        public Task<Either<Failure, DisplayConnectionSessionMeetingDTO>> JoinRoom(int userId, int sessionId);

        public Task<Either<Failure, string>> SendOffer(string canditade, int sessionId);

        public Task<Either<Failure, string>> SendIceCandidate(string canditade, int sessionId);

        public Task<Either<Failure, string>> SendAnswer(string answer, int sessionId);

        public Task<Either<Failure, string>> LeaveRoom(string connectionId);

        public Task<Either<Failure, bool>> EndRoom(int userId, int sessionId);
    }
}