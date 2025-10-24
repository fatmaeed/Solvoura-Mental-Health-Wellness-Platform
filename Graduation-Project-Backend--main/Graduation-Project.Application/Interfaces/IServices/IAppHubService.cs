using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Utils;

namespace Graduation_Project.Application.Interfaces.IServices {

    public interface IAppHubService {

        public Task<Either<Failure, string>> RegisterUser(int userId, string connectionId);

        public Task<Either<Failure, string>> UnRegisterUser(string connectionId);

        public Task<Either<Failure, NotificationToDTO>> SendNotificationToUser(int usersId, CreateNotificationDTO notification);

        Task<Either<Failure, DisplayNotificationDTO>> SendNotificationToAllUsers(CreateNotificationDTO notification);

        public Either<Failure, NotificationToDTO> SendNotificationToSpecificUsers(List<int> usersId, CreateNotificationDTO notification);

        public Either<Failure, NotificationToDTO> SendNotificationToGroup(string groupName, CreateNotificationDTO notification);
    }
}