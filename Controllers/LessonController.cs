using AutoMapper;
using epp_be.DTOs;
using epp_be.Interfaces;
using epp_be.Data;
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
    public class LessonController : ControllerBase
    {
        private readonly ILesson lessonService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICourse courseService;
        public LessonController(ICourse cs, ILesson l, IMapper m, IWebHostEnvironment env)
        {
            this.mapper = m;
            this.lessonService = l;
            this.webHostEnvironment = env;
            this.courseService = cs;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(mapper.Map<List<LessonResponseDTO>>(await lessonService.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById([FromRoute] int id)
        {
            return Ok(mapper.Map<LessonResponseDTO>(await lessonService.GetLessonById(id)));
        }

        [HttpGet("getLessonsByCourse/{id}")]
        public async Task<IActionResult> GetLessonByCourse([FromRoute] int id)
        {
            return Ok(mapper.Map<List<LessonResponseDTO>>(await lessonService.GetLessonByCourseId(id)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromForm] LessonRequestDTO request)
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

            Course course = await courseService.GetCourseById(request.courseId);

            if (course == null) {
                return BadRequest();
            }
            if (int.Parse(userRole) == (int)UserRoles.Teacher)
            {
                var idToken = User.FindFirst("id")?.Value;
               
                if( int.Parse(idToken) != course.profesorId)
                {
                return Forbid();
                }
            }


            if (request.video == null || request.video.Length == 0)
                return BadRequest("No video uploaded.");

            string courseFolder = Path.Combine(webHostEnvironment.WebRootPath, "videos", request.courseId.ToString());

            if (!Directory.Exists(courseFolder))
                Directory.CreateDirectory(courseFolder);

            string fileName = Path.GetFileNameWithoutExtension(request.video.FileName);
            fileName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_") + Path.GetExtension(request.video.FileName);

            string filePath = Path.Combine(courseFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.video.CopyToAsync(stream);
            }

            var lesson = mapper.Map<Lesson>(request);
            lesson.VideoPath = Path.Combine("videos", request.courseId.ToString(), fileName);

            await lessonService.CreateLesson(lesson);

            return Ok(mapper.Map<LessonResponseDTO>(lesson));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson([FromRoute] int id)
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


            Lesson? lesson = await lessonService.GetLessonById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            Course course = await courseService.GetCourseById(lesson.courseId);

            if (course == null)
            {
                return BadRequest();
            }
            if (int.Parse(userRole) == (int)UserRoles.Teacher)
            {
                var idToken = User.FindFirst("id")?.Value;

                if (int.Parse(idToken) != course.profesorId)
                {
                    return Forbid();
                }
            }

            string videoPath = Path.Combine(webHostEnvironment.WebRootPath, lesson.VideoPath);
            if (System.IO.File.Exists(videoPath))
            {
                System.IO.File.Delete(videoPath);
            }

            await lessonService.DeleteLesson(id);

            return Ok(mapper.Map<LessonResponseDTO>(lesson));
        }
    }
}
