using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace epp_be.Services
{
    public class LessonService : ILesson
    {
        private readonly DatabaseContext db;

        public LessonService(DatabaseContext _db)
        {
            this.db = _db;
        }

        public async Task<Lesson?> CreateLesson(Lesson lesson)
        {
            db.Lessons.Add(lesson);
            await db.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson?> DeleteLesson(int id)
        {
           Lesson? lesson = await db.Lessons.Where( lesson  => lesson.Id == id).FirstOrDefaultAsync();
            if (lesson != null) { 
                db.Lessons.Remove(lesson);
                await db.SaveChangesAsync();
            }
            return lesson;
        }

        public async Task<List<Lesson>?> GetAll()
        {
            return await db.Lessons.ToListAsync();
        }

        public async Task<List<Lesson>?> GetLessonByCourseId(int id)
        {
            return await db.Lessons.Where( lesson => lesson.courseId == id).ToListAsync();
        }

        public async Task<Lesson?> GetLessonById(int id)
        {
            return await db.Lessons.Where( lesson => lesson.Id == id).FirstOrDefaultAsync();
        }
    }
}
