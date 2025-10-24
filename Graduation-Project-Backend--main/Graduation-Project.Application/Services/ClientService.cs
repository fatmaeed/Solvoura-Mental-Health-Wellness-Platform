using AutoMapper;
using Graduation_Project.Application.DTOs.ClientDTOs;
using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Graduation_Project.Application.Services {
    public class ClientService : IClientService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailHandler;
        private readonly UserManager<UserEntity> _userManager;
        private readonly INotificationService notificationService;

        public ClientService(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailHandler, UserManager<UserEntity> userManager, INotificationService notificationService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailHandler = emailHandler;
            _userManager = userManager;
            this.notificationService = notificationService;
        }
        public Task<List<DisplayClientDTO>> GetAllClients() {
            var clients = _unitOfWork.ClientRepo.GetAll().ToList();
            if (clients == null || !clients.Any()) {
                throw new KeyNotFoundException("No clients found.");
            }

            var clientDtos = _mapper.Map<List<DisplayClientDTO>>(clients);
            return Task.FromResult(clientDtos);
        }
        public Task<DisplayClientDTO> GetById(int id) {
            var client = _unitOfWork.ClientRepo.Get(id);
            if (client == null) {
                throw new KeyNotFoundException("Client not found.");
            }

            var clientDto = _mapper.Map<DisplayClientDTO>(client);
            return Task.FromResult(clientDto);

        }

        public List<ClientSessionDTO> GetClientSessions(int clientId) {

            var sessions = _unitOfWork.SessionRepo.GetAll()
                .Where(s => s.Reservation?.ClientId == clientId).OrderBy(s => s.StartDateTime)
                .ToList();
            var clientSessions = _mapper.Map<List<ClientSessionDTO>>(sessions);
            //foreach (var session in clientSessions)
            //{
            //    session.CanCancelOrPostpone = session.Status == "NotStarted" || session.Status == "Done";
            //    session.StartDateTime = session.StartDateTime.ToLocalTime();
            //    session.EndDateTime = session.EndDateTime.ToLocalTime();
            //}
            return clientSessions;
        }

        public Task<List<DoctorsForClientDto>> GetDoctorsForClient(int id) {
            var session = _unitOfWork.SessionRepo.GetAll().Where(s => s.Reservation.ClientId == id && s.Status != Domain.Enums.SessionStatus.Canceled).ToList();

            var providers = session
                    .GroupBy(s => s.ServiceProviderId)
                    .Select(g => new DoctorsForClientDto {
                        Name = g.First().ServiceProvider.FirstName,
                        Specialization = g.First().ServiceProvider.Specialization,
                        UserImagePath = g.First().ServiceProvider.UserImagePath,
                        SessionsCount = g.Count(),
                        Experience = g.First().ServiceProvider.Experience,
                        NumberOfExp = g.First().ServiceProvider.NumberOfExp,
                        ClinicLocation = g.First().ServiceProvider.ClinicLocation,
                        PricePerHour = g.First().ServiceProvider.PricePerHour,
                    })
                    .ToList();
            return Task.FromResult(providers);
        }

        public async Task HandleSessionForClient(ClientRequestForSessionDTO request, int userId) {


            var session = _unitOfWork.SessionRepo.Get(request.SessionId);
            if (session == null) {
                throw new KeyNotFoundException("Session not found.");
            }

            var serviceprovider = _unitOfWork.ServiceProviderRepo.Get(session.ServiceProvider.Id);

            var client = _unitOfWork.ClientRepo.Get(session.Reservation.ClientId);

            if (serviceprovider == null || client == null) {
                throw new Exception("Doctor or Client not found.");
            }

            string subject = "";
            string message = "";

            switch (request.ActionType) {
                case SessionActionType.Cancel:
                    subject = "Session Cancellation Request";
                    message = $"Dear Dr. {serviceprovider.FirstName},<br><br>" +
                              $"The patient {client.FirstName} has requested to cancel the session scheduled on {session.StartDateTime:g}.<br>" +
                              $"<b>Reason:</b> {request.Reason}<br><br>Thanks.";
                    session.Status = Domain.Enums.SessionStatus.Canceled;
                    var noti = new CreateNotificationDTO {
                        Title = "Cancellation Request",
                        Message = $"Patient {client.FirstName} has requested to cancel the session on {session.StartDateTime:g}.",
                        Type = "Session",
                        Routing = "/service-provider-dashboard/client-request",
                        Readed = false,
                        SenderId = serviceprovider.Id
                    };

                    await notificationService.SendNotification(serviceprovider.Id, noti);

                    _unitOfWork.SessionRepo.Update(session);
                    await _unitOfWork.SaveChangesAsync();
                    break;

                case SessionActionType.Postpone:
                    subject = "Session Postponement Request";
                    message = $"Dear Dr. {serviceprovider.FirstName},<br><br>" +
                              $"The patient {client.FirstName} has requested to postpone the session scheduled on {session.StartDateTime:g}.<br>" +
                              $"<b>Reason:</b> {request.Reason}<br><br>Thanks.";
                    session.Status = Domain.Enums.SessionStatus.Posponed;
                    var notific = new CreateNotificationDTO {
                        Title = "Postponement Request",
                        Message = $"Patient {client.FirstName} has requested to postpone the session on {session.StartDateTime:g}.",
                        Type = "Session",
                        Readed = false,
                        Routing = "/service-provider-dashboard/client-request",
                        SenderId = serviceprovider.Id
                    };
                    await notificationService.SendNotification(serviceprovider.Id, notific);

                    _unitOfWork.SessionRepo.Update(session);
                    await _unitOfWork.SaveChangesAsync();
                    break;

                default:
                    throw new ArgumentException("Invalid action type.");

            }

            var user = await _userManager.FindByIdAsync(serviceprovider.Id.ToString());

            if (user == null || string.IsNullOrEmpty(user.Email)) {
                throw new Exception("Doctor not found or missing email.");
            }

            await _emailHandler.SendEmailAsync(user.Email, subject, message);

        }
        public async Task<bool> UpdateClientAsync(int id, UpdateClientDTO dto) {
            var existingClient = _unitOfWork.ClientRepo.Get(id);
            if (existingClient == null) {
                return false;
            }

            _mapper.Map(dto, existingClient);

            _unitOfWork.ClientRepo.Update(existingClient);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task DeleteClient(int id) {
            var client = _unitOfWork.ClientRepo.Get(id);
            if (client == null) {
                throw new KeyNotFoundException("Client not found.");
            }
            _unitOfWork.ClientRepo.Delete(client.Id);
            await _unitOfWork.SaveChangesAsync();
            var user = await _userManager.FindByIdAsync(client.Id.ToString());
            string subject = "Client Account Has Been Deleted";
            string message = $"Dear {client.FirstName} {client.LastName},<br><br>" +
                             $"Your  account has been deleted.<br>" +
                             $"If you have any questions or need further assistance, please contact us.<br><br>Thanks.";
            await _emailHandler.SendEmailAsync(user.Email, subject, message);
        }

    }
}