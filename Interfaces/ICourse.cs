using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface ICourse
    {
        public Task<Course?> GetCourseById(int id);
        public Task<List<Course>?> GetAll();
        public Task<Course?> CreateCourse(Course course);
        public Task<Course?> DeleteCourse(int id);
        public Task<List<Course>?> GetCoursesByProfesorId(int profesorId);
        public Task<Course?> AlterCourse(int id,string newTitle);
    }
}
