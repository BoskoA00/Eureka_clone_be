using epp_be.Data;

namespace epp_be.DTOs
{
    public class UserReponseDTO
    {
        public int Id { get; set; }
        public string email { get; set; }
        public int role { get; set; }
        public List<Appointment> Appointments { get; set; }

    }
}
