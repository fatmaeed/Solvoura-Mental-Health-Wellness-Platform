using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProviderController : ControllerBase {

        readonly IServiceProviderService _serviceProvider;
        readonly ISessionService _sessionService;

        public ServiceProviderController(IServiceProviderService serviceProvider, ISessionService sessionService) {
            _serviceProvider = serviceProvider;
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult GetAll() {
            var result = _serviceProvider.GetAllServiceProviders();
            return result != null ? Ok(result) : NotFound();
        }
        [HttpGet("getUnApprovedServiceProviders")]
        public IActionResult GetAllUnApprovedServiceProviders() {
            var result = _serviceProvider.GetUnApprovedServiceProviders();
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var result = _serviceProvider.GetServiceProviderById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("CreateSessions")]
        public async Task<IActionResult> CreateSessions([FromBody] CreateSessionsFromSPDTO session) {
            if (session == null) {
                return BadRequest();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _sessionService.CreateSessionsFromSP(session);
            return Ok();

        }
        [HttpGet("GetSessionsForServiceProvider/{serviceProviderId}")]
        public IActionResult GetSessionsForServiceProvider(int serviceProviderId) {

            var result = _sessionService.GetAllSessionsForServiceProvider(serviceProviderId);
            return result != null ? Ok(result) : NotFound();

        }

        [HttpGet("GetSessionByStatus/{providerId}")]
        public IActionResult GetSessionByStatus(int providerId) {
            var result = _serviceProvider.GetSessionByStatus(providerId);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("ApproveServiceProvider/{id}")]
        public async Task<IActionResult> ApproveServiceProvider(int id) {
            try {
                await _serviceProvider.ApproveServiceProvider(id);
                return Ok(new { message = "Service provider approved successfully." });
            } catch (KeyNotFoundException ex) {
                return NotFound(ex.Message);
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while approving the service provider.");
            }
        }
        [HttpPost("RejectServiceProvider/{id}")]
        public async Task<IActionResult> RejectServiceProvider(int id) {
            try {
                await _serviceProvider.RejectServiceProvider(id);
                return Ok(new { message = "Service provider rejected successfully." });
            } catch (KeyNotFoundException ex) {
                return NotFound(ex.Message);
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while rejecting the service provider.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceProviderDTO dto)
        {
            var result = await _serviceProvider.UpdateServiceProviderAsync(id, dto);
            if (!result)
                return NotFound();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceProvider(int id) {
            try {
                await _serviceProvider.DeleteServiceProvider(id);
                return Ok(new { message = "Service provider deleted successfully." });
            } catch (KeyNotFoundException ex) {
                return NotFound(ex.Message);
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the service provider.");
            }
        }



        [HttpPost("HandleDecideOnSession")]
        public async Task<IActionResult> HandleDecideOnSession([FromBody] SessionDecisionforClientDTO request) {
            if (request == null) {
                return BadRequest("Invalid request data.");
            }
            try {
                await _serviceProvider.HandeDecideOnSession(request);
                return Ok(new { message = "Session handled successfully." });
            } catch (KeyNotFoundException ex) {
                return NotFound(ex.Message);
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while handling the session decision.");
            }
        }

        [HttpPut("Editprovider")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditServiceProvider([FromForm] EditServiceProviderDto providerDto)
        {
            if (providerDto == null) {
                return BadRequest();
            }
            try
            {
                await _serviceProvider.EditServiceProvider(providerDto);
                return Ok(new { message = "serviceProvider Updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while updating the session.");
            }
        }
        [HttpGet("Incoming/{providerId}")]
        public async Task<IActionResult> IncomingSessions(int providerId)
        {
            try
            {
                var sessions = await _serviceProvider.GetIncomingSessions(providerId);
                return Ok(sessions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}