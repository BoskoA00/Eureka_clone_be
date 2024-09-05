using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace epp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessagesController : ControllerBase
    {
        private readonly IContactMessages contactMessages;

        public ContactMessagesController(IContactMessages cm)
        {
            this.contactMessages = cm;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await contactMessages.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(Messages2 message)
        {
            await contactMessages.CreateMessage(message);
            return Ok(message);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int id)
        {
            var message = await contactMessages.DeleteMessage(id);
            return Ok(message);

        }



    }
}
