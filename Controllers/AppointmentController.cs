using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace epp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointment appointmentService;

        public AppointmentController( IAppointment appointment)
        {
            this.appointmentService = appointment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await appointmentService.GetAll());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) { 
            return Ok(await appointmentService.GetById(id));
        }
        [HttpGet("getByUserId/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId) { 
            return Ok(await appointmentService.GetByUserId(userId));
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody]Appointment appointment)
        {
            return Ok(await appointmentService.CreateAppointment(appointment));
        }




    }
}
