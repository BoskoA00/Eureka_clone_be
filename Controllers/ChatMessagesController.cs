using AutoMapper;
using epp_be.Data;
using epp_be.DTOs;
using epp_be.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace epp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessages chatMessagesService;
        private readonly IMapper mapper;

        public ChatMessagesController(IMapper m, IChatMessages cms)
        {
            chatMessagesService = cms;
            mapper = m;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(mapper.Map<List<ChatMessagesResponseDTO>>(await chatMessagesService.GetAll()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(mapper.Map<ChatMessagesResponseDTO>(await chatMessagesService.GetById(id)));
        }
        [HttpGet("bySenderId/{senderId}")]
        public async Task<IActionResult> GetBySenderId([FromRoute] int senderId)
        {
            return Ok(mapper.Map<List<ChatMessagesResponseDTO>>(await chatMessagesService.GetBySenderId(senderId)));
        }
        [HttpGet("byReceiverId/{receiverId}")]
        public async Task<IActionResult> GetByReceiverId([FromRoute] int receiverId)
        {
            return Ok(mapper.Map<List<ChatMessagesResponseDTO>>(await chatMessagesService.GetByReceiverId(receiverId)));
        }
        [HttpGet("byUserId/{userId}")]
        public async Task<IActionResult> GetByUserId([FromRoute] int userId)
        {
            return Ok(mapper.Map<List<ChatMessagesResponseDTO>>(await chatMessagesService.GetByUserId(userId)));
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] ChatMessagesRequestDTO request)
        {
            if (!User.Claims.Any()) {
                return Forbid();
            }

            ChatMessage? chatMessage = await chatMessagesService.CreateMessage(mapper.Map<ChatMessage>(request));

            return Ok(mapper.Map<ChatMessagesResponseDTO>(chatMessage));

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            bool status = await chatMessagesService.DeleteById(id);
            if (status)
            {
                return Ok(status);
            }
            else
            {
                return BadRequest(status);
            }
        }
        [HttpDelete("deleteByUserId/{userId}")]
        public async Task<IActionResult> DeleteByUserId([FromRoute] int userId)
        {
            bool status = await chatMessagesService.DeleteByUserId(userId);
            if (status)
            {
                return Ok(status);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ReadMessage([FromRoute] int id)
        {
            var idToken = User.FindFirst("id")?.Value;

            if(idToken == null)
            {
                return Forbid();
            }
            

            ChatMessage? chatMessage = await chatMessagesService.GetById(id);

            if( int.Parse(idToken) != chatMessage.ReceiverId)
            {
                return Forbid();
            }


            if (chatMessage == null)
            {
                return NotFound();
            }
            bool s = await chatMessagesService.ReadMessage(chatMessage.Id);

            return Ok(s);

        }

    }
}
