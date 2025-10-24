using AutoMapper;
using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Services {

    public class AppHubService : IAppHubService {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AppHubService(IUnitOfWork unitOfWork, IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Either<Failure, string>> RegisterUser(int userId, string connectionId) {
            unitOfWork.AppUserConnectionRepo.Add(new AppUserConnectionEntity() { UserId = userId, ConnectionId = connectionId });
            await unitOfWork.SaveChangesAsync();
            return Either<Failure, string>.SendRight("User Registered Successfully.");
        }

        public async Task<Either<Failure, NotificationToDTO>> SendNotificationToUser(int usersId, CreateNotificationDTO notification) {
            NotificationEntity notificationEntity = unitOfWork.NotificationRepo.Add(mapper.Map<NotificationEntity>(notification));
            await unitOfWork.SaveChangesAsync();
            UserNotificationEntity userNotification = unitOfWork.UserNotificationRepo.Add(new UserNotificationEntity() { NotificationId = notificationEntity.Id, UserId = usersId });
            await unitOfWork.SaveChangesAsync();
            AppUserConnectionEntity? appUserConnection = unitOfWork.AppUserConnectionRepo.GetByUserId(usersId)!;
            if (appUserConnection == null) {
                return Either<Failure, NotificationToDTO>.SendLeft(new NotFoundFailure("User Not Found."));
            }
            DisplayNotificationDTO displayNotification = mapper.Map<DisplayNotificationDTO>(notificationEntity);
            return Either<Failure, NotificationToDTO>.SendRight(new NotificationToDTO() { ConnectionId = appUserConnection.ConnectionId, Notification = displayNotification });
        }

        public async Task<Either<Failure, string>> UnRegisterUser(string connectionId) {
            AppUserConnectionEntity? appUserConnection = unitOfWork.AppUserConnectionRepo.RemoveByConnectionId(connectionId)!;
            if (appUserConnection == null) {
                return Either<Failure, string>.SendLeft(new NotFoundFailure("User Not Found."));
            }
            await unitOfWork.SaveChangesAsync();
            return Either<Failure, string>.SendRight("User Removed Successfully.");
        }

        public async Task<Either<Failure, DisplayNotificationDTO>> SendNotificationToAllUsers(CreateNotificationDTO notification) {
            NotificationEntity notificationEntity = unitOfWork.NotificationRepo.Add(mapper.Map<NotificationEntity>(notification));
            await unitOfWork.SaveChangesAsync();
            return Either<Failure, DisplayNotificationDTO>.SendRight(mapper.Map<DisplayNotificationDTO>(notificationEntity));
        }

        Either<Failure, NotificationToDTO> IAppHubService.SendNotificationToGroup(string groupName, CreateNotificationDTO notification) {
            throw new NotImplementedException();
        }

        Either<Failure, NotificationToDTO> IAppHubService.SendNotificationToSpecificUsers(List<int> usersId, CreateNotificationDTO notification) {
            throw new NotImplementedException();
        }
    }
}