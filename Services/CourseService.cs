using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace epp_be.Services
{
    public class CourseService : ICourse
    {
        private readonly DatabaseContext db;
        private readonly ILesson lessonService;
        public CourseService(DatabaseContext _db,ILesson ls)
        {
            this.db = _db;
            this.lessonService = ls;
        }

        public async Task<Course?> AlterCourse(int id,string newTitle)
        {
            Course? course = await db.Courses.Where( course => course.Id == id).FirstOrDefaultAsync();
            if (course == null) {
                return null;
            }
            course.Title = newTitle;
            await db.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> CreateCourse(Course course)
        {
           db.Courses.Add(course);
           await db.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> DeleteCourse(int id)
        {
            Course? course = await db.Courses.Where( course => course.Id == id).FirstOrDefaultAsync();
            if(course == null)
            {
                return null;    
            }
            List<Lesson>? lessons = await db.Lessons.Where( lesson => lesson.courseId == id).ToListAsync();
            List<UserCourse>? enrollments = await db.UserCourses.Where( enrollment => enrollment.courseId == id).ToListAsync();
            foreach (Lesson lesson in lessons) {
                await lessonService.DeleteLesson(lesson.Id);
            }
            db.UserCourses.RemoveRange(enrollments);
            db.Courses.Remove(course);
            await db.SaveChangesAsync();
            return course;
        }

        public async Task<List<Course>?> GetAll()
        {
            return await db.Courses.Include( course => course.lessons).ToListAsync();
        }

        public async Task<Course?> GetCourseById(int id)
        {
            return await db.Courses.Where( course => course.Id == id).Include(c => c.lessons).FirstOrDefaultAsync();
        }

        public async Task<List<Course>?> GetCoursesByProfesorId(int profesorId)
        {
            return await db.Courses.Where( course => course.profesorId == profesorId).Include( course => course.lessons).ToListAsync();
        }
    }
}
