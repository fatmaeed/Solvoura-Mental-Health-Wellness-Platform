using AutoMapper;
using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;

namespace Graduation_Project.Application.Services {

    public class MeetingSessionService : IMeetingSessionService {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationService notificationService;
        private readonly IMapper mapper;

        public MeetingSessionService(IUnitOfWork unitOfWork, INotificationService notificationService
            , IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.notificationService = notificationService;
            this.mapper = mapper;
        }

        public Either<Failure, CreateConnectionSessionMeetingDTO> AddConnectionSession(CreateConnectionSessionMeetingDTO createConnectionSessionMeetingDTO) {
            throw new NotImplementedException();
        }

        public async Task<Either<Failure, SessionUserConnectionDTO>> AddUserConnection(int userId, string connectionId, int sessionId) {
            try {
                var sessionUserConnection = unitOfWork.SessionUserConnectionRepo.Add(new SessionUserConnectionEntity { UserId = userId, ConnectionId = connectionId, SessionId = sessionId });
                await unitOfWork.SaveChangesAsync();
                return Either<Failure, SessionUserConnectionDTO>.SendRight(mapper.Map<SessionUserConnectionDTO>(sessionUserConnection));
            } catch (Exception ex) {
                return Either<Failure, SessionUserConnectionDTO>.SendLeft(new Failure(ex.Message));
            }
        }

        public async Task<Either<Failure, bool>> EndRoom(int userId, int sessionId) {
            //check if user is in session
            SessionEntity? Session;
            (bool flowControl, Either<Failure, bool> value) = SessionUserVaildation(userId, sessionId, out Session);
            if (!flowControl) {
                return value;
            }

            //end session
            Session!.Status = SessionStatus.Finished;
            unitOfWork.SessionRepo.Update(Session);

            SessionMeetingInfoEntity? sessionMeetingInfo = unitOfWork.SessionMeetingInfoRepo.GetBySessionId(sessionId);
            if (sessionMeetingInfo == null) {
                return Either<Failure, bool>.SendLeft(new NotFoundFailure("Session Info Not Found."));
            }
            sessionMeetingInfo.EndSessionTime = DateTime.Now;
            sessionMeetingInfo.IsCompleted = true;
            unitOfWork.SessionMeetingInfoRepo.Update(sessionMeetingInfo);

            int serviceProviderId = (int)Session!.ServiceProviderId!;
            string connectionIdToRemove = unitOfWork.SessionUserConnectionRepo.GetConnentionsByUserId(serviceProviderId)[0].ConnectionId;

            ConnectionSessionMeetingEntity? connectionSessionMeeting = unitOfWork.ConnectionSessionMeetingRepo.RemoveByConnectionId(connectionIdToRemove);
            if (connectionSessionMeeting == null) {
                return Either<Failure, bool>.SendLeft(new NotFoundFailure("The Session Not Exists"));
            }

            unitOfWork.SessionUserConnectionRepo.RemoveBySessionId(sessionId);

            await unitOfWork.SaveChangesAsync();
            return Either<Failure, bool>.SendRight(true);
        }

        public Either<Failure, DisplayConnectionSessionMeetingDTO> GetConnectionBySessionId(int sessionId) {
            ConnectionSessionMeetingEntity?
                connectionSessionMeetingEntity = unitOfWork.ConnectionSessionMeetingRepo.GetBySessionId(sessionId);
            if (connectionSessionMeetingEntity == null) {
                return Either<Failure, DisplayConnectionSessionMeetingDTO>.SendLeft(new NotFoundFailure("The Session Not Exists"));
            }
            return Either<Failure, DisplayConnectionSessionMeetingDTO>.SendRight(mapper.Map<DisplayConnectionSessionMeetingDTO>(connectionSessionMeetingEntity));
        }

        public Either<Failure, List<SessionUserConnectionDTO>> GetUserConnectionId(int userId) {
            try {
                List<SessionUserConnectionEntity> connections = unitOfWork.SessionUserConnectionRepo.GetConnentionsByUserId(userId);
                return Either<Failure, List<SessionUserConnectionDTO>>.SendRight(mapper.Map<List<SessionUserConnectionDTO>>(connections));
            } catch (Exception ex) {
                return Either<Failure, List<SessionUserConnectionDTO>>.SendLeft(new Failure(ex.Message));
            }
        }

        public async Task<Either<Failure, bool>> ConnectSession(int userId, int sessionId, string connectionId) {
            await AddUserConnection(userId, connectionId, sessionId);

            //check if user is in session
            SessionEntity? Session;
            (bool flowControl, Either<Failure, bool> value) = SessionUserVaildation(userId, sessionId, out Session);
            if (!flowControl) {
                return value;
            }
            return Either<Failure, bool>.SendRight(true);
        }

        private (bool flowControl, Either<Failure, bool> value) SessionUserVaildation(int userId, int sessionId, out SessionEntity? Session) {
            Session = unitOfWork.SessionRepo.Get(sessionId);
            if (Session == null) {
                return (flowControl: false, value: Either<Failure, bool>.SendLeft(new NotFoundFailure("Session Not Found.")));
            }
            if (Session.ServiceProviderId != userId) {
                if (Session!.Reservation?.ClientId != userId) {
                    return (flowControl: false, value: Either<Failure, bool>.SendLeft(new UnauthorizedFailure("You are not the client of this session.")));
                }
            }

            return (flowControl: true, value: null);
        }

        public async Task<Either<Failure, string>> LeaveRoom(string connectionId) {
            SessionUserConnectionEntity? sessionUserConnection = unitOfWork.SessionUserConnectionRepo.RemoveByConnectionId(connectionId)!;
            if (sessionUserConnection == null) {
                return Either<Failure, string>.SendLeft(new NotFoundFailure("Session Not Found."));
            }

            var session = unitOfWork.SessionRepo.Get((int)(sessionUserConnection!.SessionId)!);

            if (session == null) {
                return Either<Failure, string>.SendLeft(new NotFoundFailure("Session Not Found."));
            }
            if (session.ServiceProviderId != sessionUserConnection.UserId) {
                await unitOfWork.SaveChangesAsync();

                return Either<Failure, string>.SendRight(session.Id.ToString());
            }

            session.Status = SessionStatus.NotStarted;
            unitOfWork.SessionRepo.Update(session);

            SessionMeetingInfoEntity? sessionMeetingInfo = unitOfWork.SessionMeetingInfoRepo.GetBySessionId((int)sessionUserConnection!.SessionId!);
            if (sessionMeetingInfo == null) {
                return Either<Failure, string>.SendLeft(new NotFoundFailure("Session Info Not Found."));
            }
            sessionMeetingInfo.EndSessionTime = DateTime.Now;
            unitOfWork.SessionMeetingInfoRepo.Update(sessionMeetingInfo);

            ConnectionSessionMeetingEntity? connectionSessionMeeting = unitOfWork.ConnectionSessionMeetingRepo.RemoveByConnectionId(connectionId);
            await unitOfWork.SaveChangesAsync();
            return Either<Failure, string>.SendRight(session.Id.ToString());
        }

        public async Task<Either<Failure, DisplayConnectionSessionMeetingDTO>> JoinRoom(int userId, int sessionId) {
            ConnectionSessionMeetingEntity?
                 connectionSessionMeetingEntity = unitOfWork.ConnectionSessionMeetingRepo.GetBySessionId(sessionId);
            if (connectionSessionMeetingEntity == null) {
                return Either<Failure, DisplayConnectionSessionMeetingDTO>.SendLeft(new NotFoundFailure("The Session Not Exists"));
            }

            SessionMeetingInfoEntity? sessionMeetingInfo = unitOfWork.SessionMeetingInfoRepo.GetBySessionId(sessionId);
            if (sessionMeetingInfo == null) {
                return Either<Failure, DisplayConnectionSessionMeetingDTO>.SendLeft(new NotFoundFailure("Session Not Found."));
            }
            sessionMeetingInfo.StartSessionTime = DateTime.Now;
            unitOfWork.SessionMeetingInfoRepo.Update(sessionMeetingInfo);
            unitOfWork.SessionMeetingClientsRepo.Add(new SessionMeetingClientsEntity { SessionMeetingId = sessionMeetingInfo!.Id, ClientId = userId });
            await unitOfWork.SaveChangesAsync();

            return Either<Failure, DisplayConnectionSessionMeetingDTO>.SendRight(mapper.Map<DisplayConnectionSessionMeetingDTO>(connectionSessionMeetingEntity));
        }

        public Either<Failure, DisplayConnectionSessionMeetingDTO> RemoveConnectionSession(int sessionId) {
            throw new NotImplementedException();
        }

        public async Task<Either<Failure, SessionUserConnectionDTO>> RemoveUserConnection(string connectionId) {
            try {
                var session = unitOfWork.SessionUserConnectionRepo.RemoveByConnectionId(connectionId);
                await SaveChangesAsync();
                return Either<Failure, SessionUserConnectionDTO>.SendRight(mapper.Map<SessionUserConnectionDTO>(session));
            } catch (Exception ex) {
                return Either<Failure, SessionUserConnectionDTO>.SendLeft(new Failure(ex.Message));
            }
        }

        public async Task SaveChangesAsync() {
            await unitOfWork.SaveChangesAsync();
        }

        Either<Failure, SessionUserConnectionDTO> IMeetingSessionService.RemoveUserConnection(string connectionId) {
            return Either<Failure, SessionUserConnectionDTO>.SendRight(new SessionUserConnectionDTO());
        }

        Either<Failure, SessionUserConnectionDTO> IMeetingSessionService.AddUserConnection(int userId, string connectionId) {
            throw new NotImplementedException();
        }

        public async Task<Either<Failure, bool>> StartSession(int userId, int sessionId, string connectionId) {
            var session = unitOfWork.SessionRepo.Get(sessionId);
            if (session == null) {
                return Either<Failure, bool>.SendLeft(new NotFoundFailure("Session Not Found."));
            }
            if (session.ServiceProviderId != userId) {
                return Either<Failure, bool>.SendLeft(new UnauthorizedFailure("You are not the Doctor of this session."));
            }
            if (!(session.Status == SessionStatus.NotStarted || session.Status == SessionStatus.AcceptPosponed)) {
                return Either<Failure, bool>.SendLeft(new BadRequestFailure($"The Session is {EnumHandler<SessionStatus>.GetEnumName(session.Status)}."));
            }
            if (DateTime.Now < session.StartDateTime.AddMinutes(-15)) {
                return Either<Failure, bool>.SendLeft(new BadRequestFailure("The session date has not come yet."));
            }
            if (DateTime.Now > session.EndDateTime?.AddMinutes(15)) {
                return Either<Failure, bool>.SendLeft(new BadRequestFailure("Session Timeout."));
            }

            var sessionInfo = unitOfWork.SessionMeetingInfoRepo.Add(new SessionMeetingInfoEntity { SessionId = sessionId, ServiceProviderId = userId, });
            session.Status = SessionStatus.Started;
            unitOfWork.SessionRepo.Update(session);
            unitOfWork.ConnectionSessionMeetingRepo.Add(new ConnectionSessionMeetingEntity {
                ConnectionCreatorId = connectionId,
                IceCandidate = "",
                Answer = "",
                SessionId = sessionId,
                Offer = ""
            });
            await unitOfWork.SaveChangesAsync();
            await notificationService.SendNotification(session.Reservation!.ClientId, new CreateNotificationDTO() { Title = "Session Started", Message = "The Doctor Has Started The Session", Type = "Success", Routing = $"/meeting/{sessionId}" });
            return Either<Failure, bool>.SendRight(session.Status == SessionStatus.Started);
        }

        public async Task<Either<Failure, string>> SendIceCandidate(string canditade, int sessionId) {
            ConnectionSessionMeetingEntity? connection = unitOfWork.ConnectionSessionMeetingRepo.GetBySessionId(sessionId);
            if (connection == null) {
                return Either<Failure, string>.SendLeft(new NotFoundFailure("Session Not Found."));
            }
            connection!.IceCandidate = canditade;
            unitOfWork.ConnectionSessionMeetingRepo.Update(connection);
            await unitOfWork.SaveChangesAsync();
            return Either<Failure, string>.SendRight("Done");
        }

        public async Task<Either<Failure, string>> SendOffer(string offer, int sessionId) {
            ConnectionSessionMeetingEntity? connection = unitOfWork.ConnectionSessionMeetingRepo.GetBySessionId(sessionId);
            if (connection == null) {
                return Either<Failure, string>.SendLeft(new NotFoundFailure("Session Not Found."));
            }
            connection!.Offer = offer;
            unitOfWork.ConnectionSessionMeetingRepo.Update(connection);
            await unitOfWork.SaveChangesAsync();
            return Either<Failure, string>.SendRight("Done");
        }

        public async Task<Either<Failure, string>> SendAnswer(string answer, int sessionId) {
            var connection = unitOfWork.ConnectionSessionMeetingRepo.GetBySessionId(sessionId);
            connection!.Answer = answer;
            unitOfWork.ConnectionSessionMeetingRepo.Update(connection);
            await unitOfWork.SaveChangesAsync();
            return Either<Failure, string>.SendRight("Done");
        }
    }
}