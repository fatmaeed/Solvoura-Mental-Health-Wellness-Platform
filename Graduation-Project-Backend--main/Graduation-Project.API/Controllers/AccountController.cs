using Graduation_Project.API.Utils;
using Graduation_Project.Application.DTOs.AccountDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService, INotificationService notificationService) {
            this.accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser) {
            var result = await accountService.Login(loginUser);
            return result.Fold(
                error => FailureIActionResult.FailureHandler(error),
                 token => Ok(new { token, message = "Login successful" })
            );
        }
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePassword) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var result = await accountService.ChangePassword(changePassword);
            return result.Fold(
                (error) => FailureIActionResult.FailureHandler(error),
                (token) => Ok(new { token, message = "Password changed successfully" })
                );
        }

        [HttpGet("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var result = await accountService.ForgetPassword(email);
            return result.Fold(
                (error) => FailureIActionResult.FailureHandler(error),
                (token) => Ok(new { token, message = "Email sent successfully" })
                );
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPassword) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var result = await accountService.ResetPassword(resetPassword);
            return result.Fold(
                (error) => FailureIActionResult.FailureHandler(error),
                (token) => Ok(new { token, message = "Password reset successfully" })
                );
        }

        [HttpPost("register/client")]
        [Consumes("multipart/form-data")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClient([FromForm] RegisterClientDTO registerClientDTO) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var result = await accountService.RegisterClient(registerClientDTO);
            return result.Fold(
                (error) => FailureIActionResult.FailureHandler(error),
                (token) => Ok(new { token, message = "Account created successfully" })
                );
        }

        [HttpPost("register/provider")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RegisterServiceProvider([FromForm] RegisterServiceProviderDTO registerServiceProviderDTO) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var result = await accountService.RegisterServiceProvider(registerServiceProviderDTO);
            return result.Fold(
                (error) => FailureIActionResult.FailureHandler(error),
                (token) => Ok(new { token, message = "Account created successfully" })
                );
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token) {
            var result = await accountService.ConfirmEmail(email, token);
            return result.Fold(

                (error) => FailureIActionResult.FailureHandler(error),
                (token) => Ok(new { token, message = "Email confirmed successfully" })
                );
        }

        //[Authorize]
        //[HttpGet("logout")]
        //public IActionResult Logout() {
        //    return Ok();
        //}

        //[HttpGet("profile")]
        //public IActionResult Profile() {
        //    return Ok();
        //}
    }
}