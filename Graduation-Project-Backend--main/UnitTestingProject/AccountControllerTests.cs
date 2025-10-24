using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.AccountDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class AccountControllerTests
    {
        private class MockAccountService : IAccountService
        {
            public Task<Either<Failure, string>> Login(LoginUserDTO loginUserDTO) => Task.FromResult(_loginResult);
            public Task<Either<Failure, string>> RegisterServiceProvider(RegisterServiceProviderDTO dto) => Task.FromResult(_registerProviderResult);
            public Task<Either<Failure, string>> RegisterClient(RegisterClientDTO dto) => Task.FromResult(_registerClientResult);
            public Task<Either<Failure, string>> ForgetPassword(string email) => Task.FromResult(_forgetPasswordResult);
            public Task<Either<Failure, string>> ResetPassword(ResetPasswordDTO dto) => Task.FromResult(_resetPasswordResult);
            public Task<Either<Failure, string>> ChangePassword(ChangePasswordDTO dto) => Task.FromResult(_changePasswordResult);
            public Task<Either<Failure, string>> ConfirmEmail(string email, string token) => Task.FromResult(_confirmEmailResult);
            public Task<Graduation_Project.Domain.Entities.UserEntity?> RegisterUser(RegisterUserDTO dto) => Task.FromResult<Graduation_Project.Domain.Entities.UserEntity?>(null);

            public Either<Failure, string> _loginResult;
            public Either<Failure, string> _registerProviderResult;
            public Either<Failure, string> _registerClientResult;
            public Either<Failure, string> _forgetPasswordResult;
            public Either<Failure, string> _resetPasswordResult;
            public Either<Failure, string> _changePasswordResult;
            public Either<Failure, string> _confirmEmailResult;
        }

        private class MockNotificationService : INotificationService
        {
            public Task SendNotification(int userId, Graduation_Project.Application.DTOs.NotificationDTOs.CreateNotificationDTO notification) => Task.CompletedTask;
            public Task SendNotificationToAll(Graduation_Project.Application.DTOs.NotificationDTOs.CreateNotificationDTO notification) => Task.CompletedTask;
        }

        private MockAccountService _accountService;
        private MockNotificationService _notificationService;
        private AccountController _controller;

        [TestInitialize]
        public void Setup()
        {
            _accountService = new MockAccountService();
            _notificationService = new MockNotificationService();
            _controller = new AccountController(_accountService, _notificationService);
        }

        [TestMethod]
        public async Task Login_ReturnsOk_OnSuccess()
        {
            _accountService._loginResult = Either<Failure, string>.SendRight("token123");
            var dto = new LoginUserDTO { UserNameOrEmail = "user@test.com", Password = "password", RememberMe = false };
            var result = await _controller.Login(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Login_ReturnsFailure_OnError()
        {
            _accountService._loginResult = Either<Failure, string>.SendLeft(new Failure("Login failed"));
            var dto = new LoginUserDTO { UserNameOrEmail = "user@test.com", Password = "password", RememberMe = false };
            var result = await _controller.Login(dto);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ChangePassword_ReturnsOk_OnSuccess()
        {
            _accountService._changePasswordResult = Either<Failure, string>.SendRight("token123");
            var dto = new ChangePasswordDTO { UserId = 1, OldPassword = "oldpass", NewPassword = "newpassword", ConfirmPassword = "newpassword" };
            _controller.ModelState.Clear();
            var result = await _controller.ChangePassword(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ChangePassword_ReturnsBadRequest_OnInvalidModel()
        {
            var dto = new ChangePasswordDTO { UserId = 1, OldPassword = "oldpass", NewPassword = "newpassword", ConfirmPassword = "newpassword" };
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.ChangePassword(dto);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task ForgetPassword_ReturnsOk_OnSuccess()
        {
            _accountService._forgetPasswordResult = Either<Failure, string>.SendRight("token123");
            _controller.ModelState.Clear();
            var result = await _controller.ForgetPassword("user@test.com");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ForgetPassword_ReturnsBadRequest_OnInvalidModel()
        {
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.ForgetPassword("user@test.com");
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task ResetPassword_ReturnsOk_OnSuccess()
        {
            _accountService._resetPasswordResult = Either<Failure, string>.SendRight("token123");
            var dto = new ResetPasswordDTO { Email = "user@test.com", Password = "newpassword", ConfirmPassword = "newpassword", Token = "token" };
            _controller.ModelState.Clear();
            var result = await _controller.ResetPassword(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ResetPassword_ReturnsBadRequest_OnInvalidModel()
        {
            var dto = new ResetPasswordDTO { Email = "user@test.com", Password = "newpassword", ConfirmPassword = "newpassword", Token = "token" };
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.ResetPassword(dto);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task RegisterClient_ReturnsOk_OnSuccess()
        {
            _accountService._registerClientResult = Either<Failure, string>.SendRight("token123");
            var dto = new RegisterClientDTO { FirstName = "Test", LastName = "User", Address = "Address", Gender = 0, BirthDate = new System.DateOnly(2000, 1, 1) };
            _controller.ModelState.Clear();
            var result = await _controller.RegisterClient(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task RegisterClient_ReturnsBadRequest_OnInvalidModel()
        {
            var dto = new RegisterClientDTO { FirstName = "Test", LastName = "User", Address = "Address", Gender = 0, BirthDate = new System.DateOnly(2000, 1, 1) };
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.RegisterClient(dto);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task RegisterServiceProvider_ReturnsOk_OnSuccess()
        {
            _accountService._registerProviderResult = Either<Failure, string>.SendRight("token123");
            var dto = new RegisterServiceProviderDTO { NationalNumber = "12345678901234", Specialization = 1, UserImage = null, NationalImage = null, UserAndNationalImage = null, Description = "desc", ExaminationType = 1, ExperienceInYears = 1, PricePerHour = 100, Certificates = new System.Collections.Generic.List<Graduation_Project.Application.DTOs.CertificateDTOs.CreateCertificateDTO>() };
            _controller.ModelState.Clear();
            var result = await _controller.RegisterServiceProvider(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task RegisterServiceProvider_ReturnsBadRequest_OnInvalidModel()
        {
            var dto = new RegisterServiceProviderDTO { NationalNumber = "12345678901234", Specialization = 1, UserImage = null, NationalImage = null, UserAndNationalImage = null, Description = "desc", ExaminationType = 1, ExperienceInYears = 1, PricePerHour = 100, Certificates = new System.Collections.Generic.List<Graduation_Project.Application.DTOs.CertificateDTOs.CreateCertificateDTO>() };
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.RegisterServiceProvider(dto);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task ConfirmEmail_ReturnsOk_OnSuccess()
        {
            _accountService._confirmEmailResult = Either<Failure, string>.SendRight("token123");
            var result = await _controller.ConfirmEmail("user@test.com", "token");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task ConfirmEmail_ReturnsFailure_OnError()
        {
            _accountService._confirmEmailResult = Either<Failure, string>.SendLeft(new Failure("Confirm failed"));
            var result = await _controller.ConfirmEmail("user@test.com", "token");
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
} 