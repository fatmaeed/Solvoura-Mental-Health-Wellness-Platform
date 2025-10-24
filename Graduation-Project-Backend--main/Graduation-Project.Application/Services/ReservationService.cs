using AutoMapper;
using Graduation_Project.Application.DTOs.ReservationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;

namespace Graduation_Project.Application.Services {

    public class ReservationService : IReservationService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservationService(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReservationRequestDto> CreateReservationAsync(ReservationRequestDto request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request), "Reservation request cannot be null.");
            }
            var providerExists = _unitOfWork.ServiceProviderRepo
                .Get(request.ServiceProviderId);
            if (providerExists == null) {
                throw new ArgumentException("Service provider not found.");
            }
            var clientExists = _unitOfWork.ClientRepo
                .Get(request.ClientId);
            if (clientExists == null) {
                throw new ArgumentException("Client not found.");
            }
            var sessions = _unitOfWork.SessionRepo.GetAll()
                .Where(s => request.SessionIds.Contains(s.Id) && s.ReservationId == null && s.ServiceProviderId == request.ServiceProviderId && (request.Status == "Both" || s.Type.ToString() == request.Status))
                .ToList();

            if (sessions.Count != request.SessionIds.Count) {
                throw new ArgumentException("Some sessions are not available for reservation.");
            }

            var reservationEntity = _mapper.Map<ReservationEntity>(request);
            foreach (var session in sessions) {
                reservationEntity.Price += session.SessionPrice;
            }
            _unitOfWork.ReservationRepo.Add(reservationEntity);
            await _unitOfWork.SaveChangesAsync();
            foreach (var session in sessions) {
                session.ReservationId = reservationEntity.Id;

                _unitOfWork.SessionRepo.Update(session);
            }
            await _unitOfWork.SaveChangesAsync();
            return await Task.FromResult(request);
        }

        public Task<List<SessionDto>> GetFreeSessionsAsync(int serviceProviderId, string status, DateTime? startDate = null, DateTime? endDate = null, TimeSpan? duration = null) {
            var providerExists = _unitOfWork.ServiceProviderRepo.Get(serviceProviderId);
            if (providerExists == null) {
                throw new ArgumentException("Service provider not found.");
            }

            if (status.ToLower() != SessionType.Online.ToString().ToLower() &&
                status.ToLower() != SessionType.Offline.ToString().ToLower() && status.ToLower() != SessionType.Both.ToString().ToLower()) {
                throw new ArgumentException("Invalid status. Use 'Online' or 'Offline' or 'Both'.");
            }

            var now = DateTime.Now;
            IEnumerable<SessionEntity> sessions = _unitOfWork.SessionRepo.GetAll()
                .Where(s =>
                    s.ServiceProviderId == serviceProviderId &&
                    s.ReservationId == null && s.StartDateTime >= now &&
                    (status == "Both" || s.Type.ToString().ToLower() == status.ToLower()));

            if (startDate.HasValue) {
                sessions = sessions.Where(s => s.StartDateTime >= startDate.Value);
            }

            if (endDate.HasValue) {
                sessions = sessions.Where(s => s.StartDateTime <= endDate.Value);
            }

            if (duration.HasValue) {
                sessions = sessions.Where(s => s.Duration <= duration.Value);
            }

            sessions = sessions.OrderBy(s => s.StartDateTime);

            var sessionList = _mapper.Map<List<SessionDto>>(sessions).ToList();
            return Task.FromResult(sessionList);
        }
    }
}