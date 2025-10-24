using Graduation_Project.Application.DTOs.ClientDTOs;
using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientController(IClientService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clients = await _service.GetAllClients();
                if (clients == null || !clients.Any())
                {
                    return NotFound("No clients found.");
                }
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving clients.");
            }
        }
        [HttpGet("GetClientById/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            try
            {
                var client = await _service.GetById(id);
                if (client == null)
                {
                    return NotFound("Client not found.");
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the client.");
            }
        }

        [HttpGet("GetClientSessions/{clientId}")]
        public IActionResult GetClientSessions(int clientId)
        {
            try
            {
                var sessions = _service.GetClientSessions(clientId);
                if (sessions == null || !sessions.Any())
                {
                    return NotFound("No sessions found for the specified client.");
                }
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving client sessions.");
            }
        }

        [HttpPost("HandleSessionForClient")]

        public async Task<IActionResult> HandleSessionForClient([FromBody] ClientRequestForSessionDTO request)
        {
            Console.WriteLine(request.ActionType);
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //await _service.HandleSessionForClient(request, int.Parse(userId));
                await _service.HandleSessionForClient(request, request.ClientId);

                return Ok(new { message = "Session handled successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetDoctorsForClient(int id)
        //{
        //    try
        //    {
        //        var providers =  _service.GetDoctorsForClient(id);
        //        if (providers == null)
        //        {
        //            return NotFound("No providers found for the specified client.");
        //        }
        //        return Ok(providers);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving client providers.");
        //    }
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) });
                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            var result = await _service.UpdateClientAsync(id, dto);
            if (!result)
                return NotFound();

           
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                await _service.DeleteClient(id);
                return Ok(new { message = "Client deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the client.");
            }
        }


    }
}
