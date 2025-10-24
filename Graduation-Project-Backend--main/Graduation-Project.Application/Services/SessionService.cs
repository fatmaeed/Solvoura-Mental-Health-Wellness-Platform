using AutoMapper;
using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Services {

    public class SessionService : ISessionService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateSessionsFromSP(CreateSessionsFromSPDTO sessionsFromSPDTO) {
            sessionsFromSPDTO.EndDateTime = sessionsFromSPDTO.StartDateTime.Add(sessionsFromSPDTO.Duration);
            sessionsFromSPDTO.SessionPrice = _unitOfWork.ServiceProviderRepo.Get(sessionsFromSPDTO.ServiceProviderId).PricePerHour * (decimal)sessionsFromSPDTO.Duration.TotalHours;

            if (sessionsFromSPDTO.RepeatedFor == 0) {
                var session = _mapper.Map<SessionEntity>(sessionsFromSPDTO);
                //session.Status = SessionStatus.NotStarted;
                _unitOfWork.SessionRepo.Add(session);
                await _unitOfWork.SaveChangesAsync();
            } else {
                var currentDate = sessionsFromSPDTO.StartDateTime;
                var endDate = sessionsFromSPDTO.ToDate?.ToDateTime(TimeOnly.MinValue);
                while (currentDate.Day <= endDate.Value.Day || currentDate.Month < endDate.Value.Month) {
                    var session = _mapper.Map<SessionEntity>(sessionsFromSPDTO);
                    session.StartDateTime = currentDate;
                    session.EndDateTime = currentDate.Add(sessionsFromSPDTO.Duration);
                    _unitOfWork.SessionRepo.Add(session);
                    currentDate = currentDate.AddDays(sessionsFromSPDTO.RepeatedFor);
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteSession(int id) {
            var session = _unitOfWork.SessionRepo.Get(id);

            if (session == null) {
                throw new ArgumentException("Session not found.");
            }

            if (session.ReservationId != null) {
                throw new InvalidOperationException("Cannot delete a reserved session.");
            }

            _unitOfWork.SessionRepo.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditSession(EditSessionDto sessionDto) {
            if (sessionDto == null) {
                throw new ArgumentNullException(nameof(sessionDto), "EditSession request cannot be null.");
            }
            var session = _unitOfWork.SessionRepo.Get(sessionDto.Id);
            if (session == null) {
                throw new ArgumentException("session not found.");
            }
            if (session.ReservationId == null) {
                _mapper.Map(sessionDto, session);
                session.SessionPrice = _unitOfWork.ServiceProviderRepo.Get(sessionDto.ServerProvider).PricePerHour * (decimal)sessionDto.Duration.TotalHours;
                _unitOfWork.SessionRepo.Update(session);
                await _unitOfWork.SaveChangesAsync();
            } else {
                throw new InvalidOperationException("Cannot edit a reserved session.");
            }
        }

        public List<DisplaySessionsForSPDTO> GetAllSessionsForServiceProvider(int serviceProviderId) {
            var sessions = _unitOfWork.SessionRepo.GetAll().Where(s => s.ServiceProviderId == serviceProviderId).ToList();
            return _mapper.Map<List<DisplaySessionsForSPDTO>>(sessions);
        }

        public Either<Failure, DisplayMeetingSessionDTO> GetMeetingSession(int id) {
            SessionEntity? session = _unitOfWork.SessionRepo.Get(id);
            if (session == null) {
                return Either<Failure, DisplayMeetingSessionDTO>.SendLeft(new NotFoundFailure("The Session Not Exists"));
            }

            return Either<Failure, DisplayMeetingSessionDTO>.SendRight(_mapper.Map<DisplayMeetingSessionDTO>(session));
        }
        public DisplaySessionDto GetSessionById(int id) {
            var session = _unitOfWork.SessionRepo.Get(id);
            if (session == null) {
                throw new ArgumentException("session not found");
            }

            return new DisplaySessionDto {
                Id = session.Id,
                StartDateTime = session.StartDateTime,
                Duration = session.Duration.ToString(),
                Type = (int)session.Type,
                Status = (int)session.Status
            };
        }

        Task<DisplaySessionDto> ISessionService.GetSessionById(int id) {
            throw new NotImplementedException();
        }
    }
}