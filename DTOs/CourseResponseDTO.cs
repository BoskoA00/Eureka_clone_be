using epp_be.Data;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace epp_be.DTOs
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public string title { get; set; }
        public List<Lesson> lessons { get; set; }
        public int profesorId { get; set; }
         
    }
}
