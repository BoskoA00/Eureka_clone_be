using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface ILesson
    {
        public Task<Lesson?> GetLessonById(int id);
        public Task<List<Lesson>?> GetLessonByCourseId(int id);
        public Task<List<Lesson>?> GetAll();
        public Task<Lesson?> CreateLesson(Lesson lesson);
        public Task<Lesson?> DeleteLesson(int id);
    }
}
