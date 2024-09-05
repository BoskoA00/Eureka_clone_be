using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface IPayments
    {
        public Task<Payments?> CreatePayment(Payments payment);
        public Task<Payments?> DeletePayment(int id);
        public Task<Payments?> GetPaymentById(int id);
        public Task<List<Payments>?> GetPayments();


    }
}
