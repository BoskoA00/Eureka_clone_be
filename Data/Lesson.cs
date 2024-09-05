using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace epp_be.Data
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoPath { get; set; }
        [ForeignKey(nameof(courseId))]
        public int courseId { get; set; }
    }
}
