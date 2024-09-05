using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PaymentService : IPayments
{
    private readonly DatabaseContext db;

    public PaymentService(DatabaseContext Db)
    {
        this.db = Db;
    }

    public async Task<Payments?> CreatePayment(Payments payment)
    {
        User? user = await db.Users.Where( user => user.email == payment.email).FirstOrDefaultAsync();
        if (user == null)
        {
            return null;
        }

        payment.appointment.userId = user.Id;
        await db.Appointments.AddAsync(payment.appointment);
        await db.SaveChangesAsync();

        payment.appointemtId = payment.appointment.Id;

        await db.Payments.AddAsync(payment);
        await db.SaveChangesAsync();
        return payment;
    }

    public async Task<Payments?> DeletePayment(int id)
    {
        Payments? payment = await db.Payments.Where( payment => payment.Id == id).FirstOrDefaultAsync();
        if (payment == null)
        {
            return null;
        }
        db.Payments.Remove(payment);
        await db.SaveChangesAsync();
        return payment;
    }

    public async Task<Payments?> GetPaymentById(int id)
    {
        return await db.Payments.Where( payment => payment.Id == id).Include( payment => payment.appointment).FirstOrDefaultAsync();
    }

    public async Task<List<Payments>?> GetPayments()
    {
        return await db.Payments.Include( payment => payment.appointment).ToListAsync();
    }
}
