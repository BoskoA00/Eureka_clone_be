using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace epp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayments paymentsService;
        private readonly IUser userService;
        public PaymentsController(IPayments ps,IUser us)
        {
            this.paymentsService = ps;
            this.userService = us;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await paymentsService.GetPayments());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) {
            return Ok(await paymentsService.GetPaymentById(id));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteById(int id) {

            return Ok(await paymentsService.DeletePayment(id));
        }
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] Payments request)
        {
            if (!userService.TakenEmail(request.email))
            {
                return BadRequest("User not registered");
            }
            if(request.type == 1)
            {

                request.expirationDate = "";
                request.cardNumber = "";
                request.securityCode = "";
                request.country = "";

            }
            return Ok(await paymentsService.CreatePayment(request));
        }


    }
}
