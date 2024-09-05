using System.ComponentModel.DataAnnotations.Schema;

namespace epp_be.Data
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Lesson> lessons { get; set; }
        public int profesorId { get; set; }
        [ForeignKey(nameof(profesorId))]
        public User Profesor { get; set; }

    }
}
