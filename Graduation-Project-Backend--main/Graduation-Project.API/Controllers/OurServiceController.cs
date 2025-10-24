using Graduation_Project.Application.DTOs.OurServiceDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OurServiceController : ControllerBase
    {
        private readonly IOurService _service;

        public OurServiceController(IOurService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            try
            {
                var services = await _service.GetAllAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result); 
        }


        [HttpPost]
        public async Task<IActionResult> AddService([FromBody] OurserviceDTO serviceDto)
        {
            try
            {
                if (serviceDto == null)
                {
                    return BadRequest("Service data cannot be null");
                }
                await _service.AddAsync(serviceDto);
                return Ok( new{message="added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OurserviceDTO serviceDto)
        {
            var result = await _service.UpdateAsync(id, serviceDto);
            if (!result)
                return StatusCode(500, "Something went wrong");


            return Ok(new { message = "Updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new {message= "Service deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
