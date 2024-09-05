using epp_be.Data;
using System.Reflection.Metadata.Ecma335;

namespace epp_be.DTOs
{
    public class UserCourseCourse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int profesorId { get; set; }
        public List<Lesson> lessons { get; set; }
    }
}
