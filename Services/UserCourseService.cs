using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace epp_be.Services
{
    public class UserCourseService : IUserCourse
    {
        private readonly DatabaseContext db;

        public UserCourseService(DatabaseContext _db)
        {
            this.db = _db;
        }

        public async Task<bool> DeleteByCourseId(int id)
        {
            List<UserCourse?> enrollments = await db.UserCourses.Where( enrollment => enrollment.courseId == id).ToListAsync();
            if (enrollments != null) {
                db.UserCourses.RemoveRange(enrollments);
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }



        }

        public async Task<bool> DeleteByCourseUserId(int userId, int courseId)
        {
           UserCourse? enrollment = await db.UserCourses.Where( enrollment => enrollment.userId == userId && enrollment.courseId == courseId).FirstOrDefaultAsync();
            if (enrollment != null) {
                db.UserCourses.Remove(enrollment);
                await db.SaveChangesAsync();
                return true;
            }
            {
                return false;
            }



        }

        public async Task<bool> DeleteById(int id)
        {
            UserCourse? enrollment = await db.UserCourses.Where( enrollment => enrollment.Id == id).FirstOrDefaultAsync();
            if (enrollment != null)
            {
                db.UserCourses.Remove(enrollment);
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteByUserId(int id)
        {
            List<UserCourse?> enrollments = await db.UserCourses.Where( enrollment => enrollment.userId == id).ToListAsync();
            if (enrollments != null)
            {
                db.UserCourses.RemoveRange(enrollments);
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> Enroll(int userId, int courseId)
        {
           UserCourse course = new UserCourse() { 
            userId = userId,
            courseId = courseId
           };
            db.UserCourses.Add(course);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserCourse>?> GetAll()
        {
            return await db.UserCourses.ToListAsync();
        }

        public async Task<List<UserCourse?>> GetByCourseId(int id)
        {
            return await db.UserCourses.Where( enrollment => enrollment.courseId == id).ToListAsync();
        }

        public async Task<UserCourse?> GetById(int id)
        {
            return await db.UserCourses.Where( enrollment => enrollment.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> GetByUserCourseId(int userId, int courseId)
        {
            UserCourse? enrollment = await db.UserCourses.Where ( enrollment => enrollment.userId == userId && enrollment.courseId == courseId).FirstOrDefaultAsync();
            if (enrollment != null) {
                return true;
            }
            else
            {
                return false;
            }


        }

        public async Task<List<UserCourse?>> GetByUserId(int id)
        {
            return await db.UserCourses.Where( enrollment => enrollment.userId == id).ToListAsync();
        }
    }
}
