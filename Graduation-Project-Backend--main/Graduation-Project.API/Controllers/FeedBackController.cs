using Graduation_Project.Application.DTOs.FeedBackDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private IFeedBackService _feedBackService;
        public FeedBackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;               
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedBack(CreateFeedBackDTO feedBackDTO)
        {
            if(feedBackDTO is null || !ModelState.IsValid)
                return BadRequest(ModelState);
            await _feedBackService.CreateFeedBack(feedBackDTO);
            return Created(); 

        } 
    }
}
