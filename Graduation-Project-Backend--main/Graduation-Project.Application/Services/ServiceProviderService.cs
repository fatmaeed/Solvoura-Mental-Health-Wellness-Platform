using AutoMapper;
using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Graduation_Project.Application.Services {
    public class ServiceProviderService : IServiceProviderService {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly INotificationService notificationService;

        public ServiceProviderService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<UserEntity> userManager, IEmailSender emailSender, INotificationService notificationService) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            this.notificationService = notificationService;
        }

        public async Task EditServiceProvider(EditServiceProviderDto providerDto) {
            if (providerDto == null) {
                throw new ArgumentNullException(nameof(providerDto), "provider has not data");
            }
            var provider = unitOfWork.ServiceProviderRepo.Get(providerDto.Id);
            if (provider == null) {
                throw new ArgumentException("Provider not found");
            }

            if (providerDto.UserImage != null) {
                var result = await ImageHandler.UploadImage(providerDto.UserImage, "Providers");
                if (result is UploadImageSuccess success) {
                    provider.UserImagePath = success.Path;
                } else if (result is UploadImageFailed failed) {
                    throw new InvalidOperationException(failed.Message);
                }
            }

            if (providerDto.Certificates != null && providerDto.Certificates.Any()) {
                foreach (var certDto in providerDto.Certificates) {
                    var certFileResult = await ImageHandler.UploadImage(certDto.Image, "Certificates");
                    if (certFileResult is not UploadImageSuccess certSuccess) {
                        throw new InvalidOperationException("Certificate file upload failed");
                    }

                    var cert = mapper.Map<CertificateEntity>(certDto);
                    cert.ServiceProviderId = providerDto.Id;
                    cert.ImagePath = certSuccess.Path;
                    provider.Certificates.Add(cert);
                }
            }
            mapper.Map(providerDto, provider);
            provider.IsAprroved = false;
            unitOfWork.ServiceProviderRepo.Update(provider);
            await unitOfWork.SaveChangesAsync();

        }

        public List<DisplayServiceProviderDTO> GetAllServiceProviders() {
            var serviceProviders = unitOfWork.ServiceProviderRepo.GetAll().Where(sp => sp.IsAprroved).ToList();
            return mapper.Map<List<DisplayServiceProviderDTO>>(serviceProviders).ToList();
        }

        public List<DisplayServiceProviderDTO> GetUnApprovedServiceProviders() {
            var serviceProviders = unitOfWork.ServiceProviderRepo.GetAll().Where(sp => !sp.IsAprroved).ToList();
            return mapper.Map<List<DisplayServiceProviderDTO>>(serviceProviders).ToList();
        }
        public DisplayServiceProviderDTO GetServiceProviderById(int id) {
            var serviceProvider = unitOfWork.ServiceProviderRepo.Get(id);
            return mapper.Map<DisplayServiceProviderDTO>(serviceProvider);
        }
        public async Task ApproveServiceProvider(int id) {
            var serviceProvider = unitOfWork.ServiceProviderRepo.Get(id);
            if (serviceProvider == null) {
                throw new KeyNotFoundException("Service provider not found.");
            }
            serviceProvider.IsAprroved = true;
            unitOfWork.ServiceProviderRepo.Update(serviceProvider);
            await unitOfWork.SaveChangesAsync();
            var user = await _userManager.FindByIdAsync(serviceProvider.Id.ToString());
            string subject = "Service Provider Approved";
            string message = $"Dear {serviceProvider.FirstName},<br><br>" +
                             $"Congratulations! Your  account has been approved.<br>" +
                             $"You can now start providing services on our platform.<br><br>Thanks.";
            await _emailSender.SendEmailAsync(user.Email, subject, message);
        }
        public async Task RejectServiceProvider(int id) {
            var serviceProvider = unitOfWork.ServiceProviderRepo.Get(id);
            if (serviceProvider == null) {
                throw new KeyNotFoundException("Service provider not found.");
            }
            unitOfWork.ServiceProviderRepo.Delete(serviceProvider.Id);
            await unitOfWork.SaveChangesAsync();
            var user = await _userManager.FindByIdAsync(serviceProvider.Id.ToString());
            string subject = "Service Provider Rejected";
            string message = $"Dear {serviceProvider.FirstName},<br><br>" +
                             $"We regret to inform you that your service provider application has been rejected.<br>" +
                             $"If you have any questions or need further assistance, please contact us.<br><br>Thanks.";
            await _emailSender.SendEmailAsync(user.Email, subject, message);
        }
        public async Task<bool> UpdateServiceProviderAsync(int id, UpdateServiceProviderDTO dto) {
            var existingProvider = unitOfWork.ServiceProviderRepo.Get(id);
            if (existingProvider == null) {
                return false;
            }

            mapper.Map(dto, existingProvider);

            unitOfWork.ServiceProviderRepo.Update(existingProvider);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task DeleteServiceProvider(int id) {
            var serviceProvider = unitOfWork.ServiceProviderRepo.Get(id);
            if (serviceProvider == null) {
                throw new KeyNotFoundException("Service provider not found.");
            }
            unitOfWork.ServiceProviderRepo.Delete(serviceProvider.Id);
            await unitOfWork.SaveChangesAsync();
            var user = await _userManager.FindByIdAsync(serviceProvider.Id.ToString());
            string subject = "Service Provider Account Deleted";
            string message = $"Dear {serviceProvider.FirstName},<br><br>" +
                             $"Your service provider account has been deleted.<br>" +
                             $"If you have any questions or need further assistance, please contact us.<br><br>Thanks.";
            await _emailSender.SendEmailAsync(user.Email, subject, message);
        }
        public DisplayServiceProviderDTO GetUnApprovedServiceProvider(int id) {
            var serviceProvider = unitOfWork.ServiceProviderRepo.GetAll().FirstOrDefault(sp => sp.Id == id && !sp.IsAprroved);
            if (serviceProvider == null) {
                throw new KeyNotFoundException("Service provider not found.");
            }
            return mapper.Map<DisplayServiceProviderDTO>(serviceProvider);
        }


        public IEnumerable<PosponedandcanceledSessionDTO> GetSessionByStatus(int providerId) {
            var sessions = unitOfWork.SessionRepo.GetAll().Where(s => s.ServiceProviderId == providerId &&
                 (s.Status == SessionStatus.Posponed || s.Status == SessionStatus.Canceled)).OrderBy(s => s.StartDateTime).ToList();

            if (sessions == null) {
                throw new KeyNotFoundException("Session not found.");
            }
            return mapper.Map<List<PosponedandcanceledSessionDTO>>(sessions);
        }

        public async Task HandeDecideOnSession(SessionDecisionforClientDTO request) {
            var session = unitOfWork.SessionRepo.Get(request.SessionId);

            if (session == null) {
                throw new KeyNotFoundException("Session not found.");
            }

            var client = unitOfWork.ClientRepo.Get(session.Reservation.ClientId);
            var provider = unitOfWork.ServiceProviderRepo.Get(session.ServiceProvider.Id);


            string subject = "";
            string message = "";
            if (request.IsApproved) {
                if (request.ClientRequetStatus == DTOs.ClientDTOs.SessionActionType.Cancel) {
                    session.Status = SessionStatus.AcceptCancelation;
                    subject = "Session Canceled";
                    message = $"Dear {client.FirstName},<br><br>" +
                                    $"Your session has been canceled.<br>" +
                                    $"<b>Notes:</b> {request.Notes}<br><br>Thanks.";
                    var notification = new CreateNotificationDTO {
                        Title = subject,
                        Message = $"Your session has been canceled.From {provider.FirstName} {provider.LastName} ",
                        Routing = $"/client-profile/client-session",
                        Readed = false,
                        SenderId = client.Id,
                        Type = "SessionDecision",

                    };

                    await notificationService.SendNotification(client.Id, notification);

                } else {
                    session.StartDateTime = request.NewStartDateTime ?? session.StartDateTime;
                    session.Status = SessionStatus.AcceptPosponed;
                    // Send email to client
                    subject = "Session Approved";
                    message = $"Dear {client.FirstName},<br><br>" +
                                    $"Your session has been approved and rescheduled to {session.StartDateTime:g}.<br>" +
                                    $"<b>Notes:</b> {request.Notes}<br><br>Thanks.";

                    var notification = new CreateNotificationDTO {
                        Title = subject,
                        Message = $"Your session has been approved and rescheduled by {provider.FirstName} {provider.LastName}",
                        Routing = $"/client-profile/client-session",
                        Readed = false,
                        SenderId = client.Id,
                        Type = "SessionDecision",

                    };
                    await notificationService.SendNotification(client.Id, notification);
                }

            } else if (!request.IsApproved) {
                if (request.ClientRequetStatus == DTOs.ClientDTOs.SessionActionType.Cancel) {
                    session.Status = SessionStatus.NotStarted;
                    subject = "Session Cancelation Request Denied";
                    message = $"Dear {client.FirstName},<br><br>" +
                                    $"We received your request to cancel the upcoming session.<br>" +
                                    $"However, your cancellation request was <b>not approved</b> by the doctor.<br>" +
                                    $"<b>Reason:</b> {request.Notes}<br><br>" +
                                    $"If you have any concerns or need further assistance, please don’t hesitate to contact us.<br><br>" +
                                    $"Best regards,<br>";

                    var notification = new CreateNotificationDTO {
                        Title = subject,
                        Message = $"Your session has not been canceled. by {provider.FirstName} {provider.LastName}",
                        Routing = $"/client-profile/client-session",
                        Readed = false,
                        SenderId = client.Id,
                        Type = "SessionDecision",

                    };
                    await notificationService.SendNotification(client.Id, notification);
                } else {
                    session.Status = SessionStatus.NotStarted;
                    // Send email to client
                    subject = "Session Reschedule Request Denied";

                    message = $"Dear {client.FirstName},<br><br>" +
                                    $"We received your request to reschedule your upcoming session.<br>" +
                                    $"However, the doctor has <b>declined</b> your reschedule request.<br>" +
                                    $"Your session will take place as originally scheduled on <b>{session.StartDateTime:g}</b>.<br>" +
                                    $"<b>Reason:</b> {request.Notes}<br><br>" +
                                    $"If you have any questions or concerns, please contact us.<br><br>" +
                                    $"Best regards,<br>";
                    var notification = new CreateNotificationDTO {
                        Title = subject,
                        Message = $"Your session will take place as originally  scheduled by {provider.FirstName} {provider.LastName} ",
                        Routing = $"/client-profile/client-session",
                        Readed = false,
                        SenderId = client.Id,
                        Type = "SessionDecision",

                    };
                    await notificationService.SendNotification(client.Id, notification);
                }

            } else {
                throw new ArgumentException("Invalid decision provided.");
            }
            unitOfWork.SessionRepo.Update(session);
            await unitOfWork.SaveChangesAsync();
            var user = await _userManager.FindByIdAsync(client.Id.ToString());
            await _emailSender.SendEmailAsync(user.Email, subject, message);

        }

        public async Task<IList<DisplayMeetingSessionDTO>> GetIncomingSessions(int proiderId) {
            var sessions = unitOfWork.SessionRepo.GetAll().Where(s => s.ServiceProviderId == proiderId &&
                s.ReservationId != null &&( s.Status == SessionStatus.NotStarted || s.Status == SessionStatus.AcceptPosponed)&&
                (s.Type == SessionType.Online || s.Type == SessionType.Both )
            ).ToList();

            var incomingSessions = mapper.Map<IList<DisplayMeetingSessionDTO>>(sessions);

            return await Task.FromResult(incomingSessions);
        }
    }
}
