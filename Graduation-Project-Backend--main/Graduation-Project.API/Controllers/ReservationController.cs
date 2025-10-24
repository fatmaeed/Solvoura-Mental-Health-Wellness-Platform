using Graduation_Project.Application.DTOs.ReservationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService service;

        public ReservationController(IReservationService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("GetFreeSessions")]
        public async Task<IActionResult> GetFreeSessionsAsync(int serviceProviderId, string status, DateTime? startdate = null, DateTime? enddate = null, TimeSpan? duration = null)
        {
            try
            {
                var sessions = await service.GetFreeSessionsAsync(serviceProviderId, status, startdate, enddate, duration);
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

        [HttpPost]
        public async Task<IActionResult> CreateReservationAsync([FromBody] ReservationRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("Reservation request cannot be null.");
            }
            try
            {
                var reservation = await service.CreateReservationAsync(request);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
         $"Error: {ex.Message} - Inner: {ex.InnerException?.Message}");
            }
        }




    }
}
