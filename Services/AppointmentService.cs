using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace epp_be.Services
{
    public class AppointmentService : IAppointment
    {
        private readonly DatabaseContext db;

        public AppointmentService(DatabaseContext Db)
        {
            this.db = Db;
        }


        public async Task<Appointment?> CreateAppointment(Appointment appointment)
        {
            await db.Appointments.AddAsync(appointment);
            await db.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment?> DeleteAppointment(int id)
        {
            Appointment? appointment = await db.Appointments.Where( appointment => appointment.Id == id ).FirstOrDefaultAsync();
            if (appointment == null) {
                return null;
               }
            db.Appointments.Remove(appointment);
            await db.SaveChangesAsync();
            return appointment;
        }

        public async Task<List<Appointment>?> GetAll()
        {
            return await db.Appointments.ToListAsync();
        }

        public async Task<Appointment?> GetById(int id)
        {
            return await db.Appointments.Where( appointemt => appointemt.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Appointment>?> GetByUserId(int userId)
        {
            return await db.Appointments.Where( appointment => appointment.userId == userId ).ToListAsync();
        }
    }
}
