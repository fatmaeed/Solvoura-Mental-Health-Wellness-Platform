using Graduation_Project.Application.DTOs.UserLikesDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLikesController : ControllerBase
    {
        private IUserLikesService _userLikesService;
        public UserLikesController(IUserLikesService userLikesService)
        {
            _userLikesService = userLikesService;
            
        }

        [HttpPut] 
        public async Task<IActionResult> PutUserLike(UserLikeDTO userLikeDTO)
        {

            if (userLikeDTO is null || !ModelState.IsValid)
                return BadRequest(ModelState);

           await _userLikesService.PutUserLike(userLikeDTO);
            return NoContent(); 

        }
    } 

}
