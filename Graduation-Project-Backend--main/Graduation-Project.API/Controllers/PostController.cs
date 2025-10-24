using Graduation_Project.Application.DTOs.PostDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await postService.GetAllAsync();
            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    var result = await postService.GetByIdAsync(id);
        //    if (result == null) return NotFound();
        //    return Ok(result);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDTO dto)
        {
            await postService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PostDTO dto)
        {
            await postService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await postService.DeleteAsync(id);
            return Ok();
        }
    }
}
