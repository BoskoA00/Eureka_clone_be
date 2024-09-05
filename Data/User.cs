namespace epp_be.Data
{
    public class User
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int role { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Payments> Payments { get; set; }

    }
}
