using System.ComponentModel.DataAnnotations.Schema;

namespace epp_be.Data
{
    public class Payments
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public int type { get; set; }
        public string cardNumber { get; set; }
        public string expirationDate { get; set; }
        public string securityCode  { get; set; }
        public string country { get; set; }
        [ForeignKey(nameof(appointemtId))]
        public int appointemtId { get; set; }
        public Appointment appointment { get; set; }
    }
}
