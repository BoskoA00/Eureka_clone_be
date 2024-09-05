using epp_be.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace epp_be.Data
{
    public class UserCourse
    {
        public int Id { get; set; }

        [ForeignKey(nameof(courseId))]
        public int courseId { get; set; }
        public Course course { get; set; }
        [ForeignKey(nameof(userId))]
        public int userId { get; set; }
        public User user { get; set; }
    }
}
