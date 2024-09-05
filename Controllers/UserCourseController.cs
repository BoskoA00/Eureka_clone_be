using epp_be.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace epp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCourseController : ControllerBase
    {
        private readonly IUserCourse ucService;

        
        public UserCourseController(IUserCourse uc)
        {
            this.ucService = uc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await ucService.GetAll();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await ucService.GetById(id);
            return Ok(result);
        }
        [HttpGet("getByCourseId/{courseId}")]
        public async Task<IActionResult> GetByCourseId([FromRoute] int courseId)
        {
            var result = await ucService.GetByCourseId(courseId);
            return Ok(result);
        }
        [HttpGet("getByUserId/{id}")]
        public async Task<IActionResult> GetByUserId([FromRoute] int id)
        {
            var result = await ucService.GetByUserId(id);
            return Ok(result);
        }
        [HttpGet("getByUserCourseId")]
        public async Task<IActionResult> GetByUserCourseId([FromQuery] int userId, [FromQuery] int courseId)
        {
            var result = await ucService.GetByUserCourseId(userId, courseId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Enroll([FromQuery] int userId, [FromQuery] int courseId)
        {
            bool enrolled = await ucService.Enroll(userId, courseId);
            if (enrolled)
            {
                return Ok();
            }
            return BadRequest("Failed enrollment");
        }
        [HttpDelete("deleteByUserId/{id}")]
        public async Task<IActionResult> DeleteByUserId([FromRoute] int id)
        {
            bool deleted = await ucService.DeleteByUserId(id);
            if (deleted)
            {
                return Ok();
            }
            return BadRequest("Deletion failed");
        }
        [HttpDelete("deleteByCourseId/{id}")]
        public async Task<IActionResult> DeleteByCourseId([FromRoute] int id)
        {
            bool deleted = await ucService.DeleteByCourseId(id);
            if (deleted)
            {
                return Ok();
            }
            return BadRequest("Deletion failed");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            bool deleted = await ucService.DeleteById(id);
            if (deleted)
            {
                return Ok();
            }
            return BadRequest("Deletion failed");
        }
        [HttpDelete("deleteByUserCourseId")]
        public async Task<IActionResult> DeleteByUserCourseId([FromQuery] int   userId, [FromQuery] int courseId)
        {
            bool deleted = await ucService.DeleteByCourseUserId(userId, courseId);
            if (deleted)
            {
                return Ok();
            }
            return BadRequest("Deletion failed");
        }
    }
}
