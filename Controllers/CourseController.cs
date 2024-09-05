using AutoMapper;
using epp_be.Data;
using epp_be.DTOs;
using epp_be.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace epp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourse courseService;
        private readonly ILesson lessonService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CourseController(ICourse cs, ILesson ls, IMapper m, IWebHostEnvironment env)
        {
            this.courseService = cs;
            this.lessonService = ls;
            this.mapper = m;
            this.webHostEnvironment = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(mapper.Map<List<CourseResponseDTO>>(await courseService.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById([FromRoute] int id)
        {
            return Ok(mapper.Map<CourseResponseDTO?>(await courseService.GetCourseById(id)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseRequestDTO request)
        {
            if (!User.Claims.Any())
            {
                return Forbid();
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.Parse(userRole) == (int)UserRoles.Student)
            {
                return Forbid();
            }

            Course? course = await courseService.CreateCourse(mapper.Map<Course>(request));
            return Ok(mapper.Map<CourseResponseDTO>(course));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {

            if (!User.Claims.Any())
            {
                return Forbid();
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.Parse(userRole) == (int)UserRoles.Student)
            {
                return Forbid();
            }



            Course? course = await courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            var idToken = User.FindFirst("id")?.Value;
            if(int.Parse(userRole) == (int)UserRoles.Teacher)
            {
                if (int.Parse(idToken) != course.profesorId)
                {
                    return Forbid();
                }
            }

            string courseFolder = Path.Combine(webHostEnvironment.WebRootPath, "videos", id.ToString());
            if (Directory.Exists(courseFolder))
            {
                Directory.Delete(courseFolder, true);
            }

            await courseService.DeleteCourse(id);

            return Ok(mapper.Map<CourseResponseDTO>(course));
        }

        [HttpGet("getByProfessor/{id}")]
        public async Task<IActionResult> GetCoursesByProfessor([FromRoute] int id)
        {
            return Ok(mapper.Map<List<CourseResponseDTO>>(await courseService.GetCoursesByProfesorId(id)));
        }
        [HttpPut]
        public async Task<IActionResult> AlterCourse([FromBody] AlterCourseRequestDTO request)
        {
            if (!User.Claims.Any())
            {
                return Forbid();
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.Parse(userRole) == (int)UserRoles.Student)
            {
                return Forbid();
            }

            Course? course = await courseService.GetCourseById(request.Id);
            if(course == null)
            {
                return BadRequest("Nonexistent course");
            }
            var idToken = User.FindFirst("id")?.Value;
            if (int.Parse(userRole) == (int)UserRoles.Teacher)
            {
                if (int.Parse(idToken) != course.profesorId)
                {
                    return Forbid();
                }
            }
            Course? alteredCourse = await courseService.AlterCourse(request.Id, request.title);

            return Ok(mapper.Map<CourseResponseDTO?>(alteredCourse));
        }
    }
}
