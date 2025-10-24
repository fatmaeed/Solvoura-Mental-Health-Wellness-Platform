using Graduation_Project.Application.DTOs.CommentDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        [HttpGet("postId")]
        public async Task<IActionResult> GetAll(int postId)
        {
            try
            {
                var comments = await commentService.GetAllAsync(postId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving comments: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var comment = await commentService.GetByIdAsync(id);
                if (comment == null)
                    return NotFound($"Comment with ID {id} not found.");
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving comment: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCommentDTO commentDto)
        {
            try
            {
              var addedComment=  await commentService.AddAsync(commentDto);
                return CreatedAtAction(nameof(GetById), new { id = addedComment.Id }, addedComment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding comment: {ex.Message}");
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CommentDTO commentDto)
        {
            try
            {
                await commentService.UpdateAsync(commentDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating comment: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await commentService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting comment: {ex.Message}");
            }
        }


    }
}
