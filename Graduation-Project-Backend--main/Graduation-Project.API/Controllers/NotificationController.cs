using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase {
        private readonly INotifiService _service;

        public NotificationController(INotifiService service) {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            var noti = _service.GetNotification(id);
            return Ok(noti);
        }

        [HttpGet("unread/{userid}")]
        public IActionResult GetAllUnReadNotifications(int userid) {
            return Ok(_service.GetAllUnread(userid));
        }

        [HttpGet("GetNotiForUser/{userId}")]
        public IActionResult GetAllNotifications(int userId) {
            var result = _service.GetAllNotificationsForUser(userId);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddNoti(CreateNotificationDTO notification) {
            if (notification == null) {
                return BadRequest();
            }
            var noti = _service.Add(notification);
            return Ok(noti);
        }

        [HttpPut]
        public async Task<IActionResult> Update(NotificationUpdateDto notification) {
            await _service.Update(notification);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            await _service.Delete(id);
            return NoContent();
        }
    }
}