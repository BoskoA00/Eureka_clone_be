using System.ComponentModel.DataAnnotations.Schema;

namespace epp_be.Data
{
    public class Appointment
    {
        public int Id { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        [ForeignKey(nameof(userId))]
        public int userId { get; set; }
    }
}
