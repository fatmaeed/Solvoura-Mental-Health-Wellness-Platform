using AutoMapper;
using Graduation_Project.Application.DTOs.AccountDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;

namespace Graduation_Project.Application.Services {

    public class AccountService : IAccountService {
        private readonly UserManager<UserEntity> userManager;
        private readonly IMapper mapper;
        private readonly IEmailSender emailHandler;
        private readonly INotificationService notificationService;
        private readonly IUnitOfWork unitOfWork;

        public AccountService(
            UserManager<UserEntity> userManager,
            IUnitOfWork unitOfWork,
            IEmailSender emailHandler,
            INotificationService notificationService,
            IMapper mapper) {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.emailHandler = emailHandler;
            this.notificationService = notificationService;
            this.mapper = mapper;
        }

        public async Task<Either<Failure, string>> Login(LoginUserDTO loginUserDTO) {
            try {
                UserEntity? user = await userManager.FindByEmailAsync(loginUserDTO.UserNameOrEmail);
                user ??= await userManager.FindByNameAsync(loginUserDTO.UserNameOrEmail);

                if (user == null) {
                    return Either<Failure, string>.SendLeft(new UnauthorizedFailure("Invalid User, Username or Password is wrong."));
                }
                bool isValidUser = await userManager.CheckPasswordAsync(user, loginUserDTO.Password);
                if (!isValidUser) {
                    return Either<Failure, string>.SendLeft(new UnauthorizedFailure("Invalid User, Username or Password is wrong."));
                }
                if (!user.EmailConfirmed) { return Either<Failure, string>.SendLeft(new UnauthorizedFailure("This email is not confirmed.")); }
                string? userRole = userManager.GetRolesAsync(user).Result.FirstOrDefault();

                return Either<Failure, string>.SendRight(MyTokenHandler.GenerateToken(user, new Claim(MyCalims.Role, userRole!)));
            } catch (Exception ex) {
                return Either<Failure, string>.SendLeft(new Failure(ex.Message));
            }
        }

        public async Task<UserEntity?> RegisterUser(RegisterUserDTO registerUserDTO) {
            if (registerUserDTO.UserName == null) {
                registerUserDTO.UserName = registerUserDTO.Email.Split('@')[0];
            }
            IdentityResult result = await userManager.CreateAsync(mapper.Map<UserEntity>(registerUserDTO), registerUserDTO.Password);
            if (!result.Succeeded) {
                return null;
            }
            return await userManager.FindByNameAsync(registerUserDTO.UserName);
        }

        public async Task<Either<Failure, string>> RegisterServiceProvider(RegisterServiceProviderDTO registerServiceProviderDTO) {
            try {
                UserEntity? user = await RegisterUser(registerServiceProviderDTO);
                if (user == null) {
                    return Either<Failure, string>.SendLeft(new UnauthorizedFailure("Error in registering user."));
                }
                ServiceProviderEntity serviceProvider = mapper.Map<ServiceProviderEntity>(registerServiceProviderDTO);
                serviceProvider.Id = user.Id;

                (bool flowControl, Either<Failure, string> value) = await UploadingImages(registerServiceProviderDTO, serviceProvider);
                if (!flowControl) {
                    return value;
                }

                unitOfWork.ServiceProviderRepo.Add(serviceProvider);
                await unitOfWork.SaveChangesAsync();
                await userManager.AddToRoleAsync(user, "SERVICEPROVIDER");

                await SendConfirmationMail(user);
                return Either<Failure, string>.SendRight("Check your email to confirm your account.");
            } catch (Exception ex) {
                return Either<Failure, string>.SendLeft(new Failure(ex.Message));
            }
        }

        private async Task SendConfirmationMail(UserEntity user) {
            string verificationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

            string url = $"{ConstantData.forntEndServerName}/confirming?token={EncodeDecodeHandler.Encode(verificationToken)}&email={EncodeDecodeHandler.Encode(user.Email!)}";
            await emailHandler.SendEmailAsync(user.Email!, "Confirm your email", "Hi Dear : \n To confirm you email please click on confirm button \n <a href=\"" + url + "\">Confirm Email</a>");
        }


        public async Task<Either<Failure, string>> RegisterClient(RegisterClientDTO registerClientDTO) {
            try {
                UserEntity? user = await RegisterUser(registerClientDTO);
                if (user == null) {
                    return Either<Failure, string>.SendLeft(new UnauthorizedFailure("Error in registering user."));
                }
                ClientEntity client = mapper.Map<ClientEntity>(registerClientDTO);
                client.Id = user.Id;
                (bool flowControl, Either<Failure, string> value) = await UploadingImages(registerClientDTO, client);
                if (!flowControl) {
                    return value;
                }
                unitOfWork.ClientRepo.Add(client);
                await unitOfWork.SaveChangesAsync();
                await SendConfirmationMail(user);
                await userManager.AddToRoleAsync(user, "CLIENT");



                return Either<Failure, string>.SendRight("Check your email to confirm your account.");
            } catch (Exception ex) {
                return Either<Failure, string>.SendLeft(new Failure(ex.Message));
            }
        }

        public async Task<Either<Failure, string>> ForgetPassword(string email) {
            UserEntity? user = await userManager.FindByEmailAsync(email);
            if (user == null) {
                return Either<Failure, string>.SendLeft(new UnauthorizedFailure("This email is not registered."));
            }
            if (!user.EmailConfirmed) {
                return Either<Failure, string>.SendLeft(new UnauthorizedFailure("This email is not confirmed."));
            }
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            string callbackUrl = $"http://localhost:4200/reset-password?token={EncodeDecodeHandler.Encode(token)}&email={EncodeDecodeHandler.Encode(email)}";
            await emailHandler.SendEmailAsync(email, "Reset Password", "Hi Dear : \n To reset you password please click on reset button \n <a href=\"" + callbackUrl + "\">Reset Password</a>");

            return Either<Failure, string>.SendRight("Email sent successfully.");
        }

        public async Task<Either<Failure, string>> ResetPassword(ResetPasswordDTO resetPasswordDTO) {
            try {
                UserEntity? user = await userManager.FindByEmailAsync(EncodeDecodeHandler.Decode(resetPasswordDTO.Email));
                if (user == null) {
                    return Either<Failure, string>.SendLeft(new UnauthorizedFailure("This email is not registered."));
                }
                IdentityResult result = await userManager.ResetPasswordAsync(user, EncodeDecodeHandler.Decode(resetPasswordDTO.Token), resetPasswordDTO.Password);
                if (!result.Succeeded) { return Either<Failure, string>.SendLeft(new UnauthorizedFailure(result.Errors.First().Description)); }
                string? role = userManager.GetRolesAsync(user).Result.FirstOrDefault();
                return Either<Failure, string>.SendRight(MyTokenHandler.GenerateToken(user, new Claim(MyCalims.Role, role!)));
            } catch (Exception ex) {
                return Either<Failure, string>.SendLeft(new Failure(ex.Message));
            }
        }

        async Task<Either<Failure, string>> IAccountService.ChangePassword(ChangePasswordDTO changePasswordDTO) {
            try {
                UserEntity? user = await userManager.FindByIdAsync(changePasswordDTO.UserId.ToString());
                if (user == null) {
                    return Either<Failure, string>.SendLeft(new UnauthorizedFailure("This user is not registered."));
                }
                IdentityResult result = await userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
                if (!result.Succeeded) { return Either<Failure, string>.SendLeft(new UnauthorizedFailure(result.Errors.First().Description)); }
                string? role = userManager.GetRolesAsync(user).Result.FirstOrDefault();
                return Either<Failure, string>.SendRight(MyTokenHandler.GenerateToken(user, new Claim(MyCalims.Role, role!)));
            } catch (Exception ex) {
                return Either<Failure, string>.SendLeft(new Failure(ex.Message));
            }
        }

        public async Task<Either<Failure, string>> ConfirmEmail(string email, string token) {

            UserEntity? userEntity = await userManager.FindByEmailAsync(EncodeDecodeHandler.Decode(email));
            if (userEntity == null) {
                return Either<Failure, string>.SendLeft(new UnauthorizedFailure("This email is not registered."));
            }
            IdentityResult result = await userManager.ConfirmEmailAsync(userEntity, EncodeDecodeHandler.Decode(token));
            if (!result.Succeeded) { return Either<Failure, string>.SendLeft(new UnauthorizedFailure(result.Errors.First().Description)); }
            string? role = userManager.GetRolesAsync(userEntity).Result.FirstOrDefault();
            return Either<Failure, string>.SendRight(MyTokenHandler.GenerateToken(userEntity, new Claim(MyCalims.Role, role!)));
        }




        private static async Task<(bool flowControl, Either<Failure, string> value)> UploadingImages(RegisterServiceProviderDTO registerServiceProviderDTO, ServiceProviderEntity serviceProvider) {
            UploadingImageStatus status = await ImageHandler.UploadImage(registerServiceProviderDTO.UserImage, "Providers");
            if (status is UploadImageFailed) {
                return (flowControl: false, value: Either<Failure, string>.SendLeft(new Failure(status.Message)));
            }

            serviceProvider.UserImagePath = ((UploadImageSuccess)status).Path;
            status = await ImageHandler.UploadImage(registerServiceProviderDTO.UserAndNationalImage, "Providers");
            if (status is UploadImageFailed) {
                return (flowControl: false, value: Either<Failure, string>.SendLeft(new Failure(status.Message)));
            }

            serviceProvider.UserAndNationalImagePath = ((UploadImageSuccess)status).Path;
            status = await ImageHandler.UploadImage(registerServiceProviderDTO.NationalImage, "Providers");
            if (status is UploadImageFailed) {
                return (flowControl: false, value: Either<Failure, string>.SendLeft(new Failure(status.Message)));
            }

            serviceProvider.NationalImagePath = ((UploadImageSuccess)status).Path;

            for (int i = 0; i < registerServiceProviderDTO.Certificates.Count; i++) {
                status = await ImageHandler.UploadImage(registerServiceProviderDTO.Certificates[i].Image, "Certificates");
                if (status is UploadImageFailed) {
                    return (flowControl: false, value: Either<Failure, string>.SendLeft(new Failure(status.Message)));
                }

                serviceProvider.Certificates[i].ImagePath = ((UploadImageSuccess)status).Path;
            }

            return (flowControl: true, value: null);
        }

        private static async Task<(bool flowControl, Either<Failure, string> value)> UploadingImages(RegisterClientDTO registerClientDTO, ClientEntity client) {
            if (registerClientDTO.UserImage != null) {
                UploadingImageStatus status = await ImageHandler.UploadImage(registerClientDTO.UserImage, "Clients");
                if (status is UploadImageFailed) {
                    return (flowControl: false, value: Either<Failure, string>.SendLeft(new Failure(status.Message)));
                }

                client.UserImagePath = ((UploadImageSuccess)status).Path;
            }
            if (registerClientDTO.UserAndNationalImage != null) {
                UploadingImageStatus status = await ImageHandler.UploadImage(registerClientDTO.UserAndNationalImage, "Clients");
                if (status is UploadImageFailed) {
                    return (flowControl: false, value: Either<Failure, string>.SendLeft(new Failure(status.Message)));
                }

                client.UserAndNationalImagePath = ((UploadImageSuccess)status).Path;
            }
            if (registerClientDTO.NationalImage != null) {
                UploadingImageStatus status = await ImageHandler.UploadImage(registerClientDTO.NationalImage, "Clients");
                if (status is UploadImageFailed) {
                    return (flowControl: false, value: Either<Failure, string>.SendLeft(new Failure(status.Message)));
                }

                client.NationalImagePath = ((UploadImageSuccess)status).Path;
            }


            return (flowControl: true, value: null);
        }



    }
}