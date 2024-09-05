using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace epp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionalSkillsMessagesController : ControllerBase
    {
        private readonly IFunctionalSkillsMessages functionalSkillMessages;

        public FunctionalSkillsMessagesController(IFunctionalSkillsMessages fs)
        {
            this.functionalSkillMessages = fs;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await functionalSkillMessages.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody]Messages1 message)
        {
            await functionalSkillMessages.CreateMessage(message);

            return Ok(message);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int id)
        {
            var m=await functionalSkillMessages.DeleteMessage(id);
            return Ok(m);
        }




    }
}
