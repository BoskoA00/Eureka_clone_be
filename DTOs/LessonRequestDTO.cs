using System.Reflection.Metadata.Ecma335;

namespace epp_be.DTOs
{
    public class LessonRequestDTO
    {
        public string title { get; set; }
        public IFormFile video { get; set; }
        public int courseId { get; set; }
    }
}
