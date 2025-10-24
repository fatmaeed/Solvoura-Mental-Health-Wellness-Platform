using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class IllnessController : ControllerBase {
        private readonly IIllnessService illnessService;

        public IllnessController(IIllnessService illnessService) {
            this.illnessService = illnessService;
        }

        [HttpGet]
        public IActionResult GetAll() {
            var result = illnessService.GetAllIllnesses();
            return result.Fold(
                (error) => BadRequest(error),
                (illnesses) => Ok(illnesses)
                );
        }
    }
}