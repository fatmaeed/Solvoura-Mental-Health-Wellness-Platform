using System.Threading.Tasks;
using Graduation_Project.Application.DTOs.PaymentDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Graduation_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentDTO paymentDTO)
        {
            if(paymentDTO == null || !ModelState.IsValid) 
                return BadRequest(ModelState);
            int paymentId = await _paymentService.CreatePayment(paymentDTO);
            return Ok(new { paymentId});
        }

    }
}
