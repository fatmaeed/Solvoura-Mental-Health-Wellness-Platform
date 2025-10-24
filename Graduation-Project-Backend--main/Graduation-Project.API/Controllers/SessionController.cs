using Graduation_Project.API.Utils;
using Graduation_Project.Application.DTOs.ReservationDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{

    namespace Graduation_Project.API.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]

        public class SessionController : ControllerBase
        {
            private readonly ISessionService _service;


            public SessionController(ISessionService service)
            {
                _service = service;
            }

            [HttpGet("GetMeetingSession/{id}")]
            public IActionResult Get(int id)
            {
                var reselt = _service.GetMeetingSession(id);
                return reselt.Fold((failure) => FailureIActionResult.FailureHandler(failure)
                , (success) => Ok(success));
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetSessionById(int id)
            {
                try
                {
                    var session = await _service.GetSessionById(id);
                    return Ok(session);
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

                    return StatusCode(500, "An error occurred while retrive the session.");
                }
            }

            [HttpPut("EditSession")]
            public async Task<IActionResult> EditSession([FromBody] EditSessionDto sessionDto)
            {
                if (sessionDto == null)
                {
                    return BadRequest("Session data is required.");
                }

                try
                {
                    await _service.EditSession(sessionDto);
                    return Ok(new { message = "Session updated successfully." });
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

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteSession(int id)
            {
                try
                {
                    await _service.DeleteSession(id);
                    return Ok(new { message = "Session Deleted Successfully" });
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
        }
    }
}
    