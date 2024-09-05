using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface IUserCourse
    {
        public Task<List<UserCourse>?> GetAll();
        public Task<UserCourse?> GetById(int id);
        public Task<List<UserCourse?>> GetByUserId(int id);
        public Task<List<UserCourse?>> GetByCourseId(int id);
        public Task<bool> DeleteByUserId(int id);
        public Task<bool> DeleteById(int id);
        public Task<bool> DeleteByCourseId(int id);
        public Task<bool> DeleteByCourseUserId(int userId, int courseId);
        public Task<bool> Enroll(int userId, int courseId);
        public Task<bool> GetByUserCourseId(int userId, int courseId);
    }
}
