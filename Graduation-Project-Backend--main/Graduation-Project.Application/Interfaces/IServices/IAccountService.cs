using Graduation_Project.Application.DTOs.AccountDTOs;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Interfaces.IServices {

    public interface IAccountService {

        public Task<Either<Failure, string>> Login(LoginUserDTO loginUserDTO);

        public Task<UserEntity?> RegisterUser(RegisterUserDTO registerUserDTO);

        public Task<Either<Failure, string>> RegisterServiceProvider(RegisterServiceProviderDTO registerServiceProviderDTO);

        public Task<Either<Failure, string>> RegisterClient(RegisterClientDTO registerClientDTO);

        //public Either<Failure, bool> ChangeEmail(ChangeEmailDTO changeEmailDTO);

        public Task<Either<Failure, string>> ForgetPassword(string email);

        public Task<Either<Failure, string>> ResetPassword(ResetPasswordDTO resetPasswordDTO);

        public Task<Either<Failure, string>> ChangePassword(ChangePasswordDTO changePasswordDTO);

        public Task<Either<Failure, string>> ConfirmEmail(string email, string token);

        //public Either<Failure, string> Logout(string token);
    }
}