using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface IAppointment
    {
        public Task<Appointment?> CreateAppointment(Appointment appointment);
        public Task<Appointment?> DeleteAppointment(int id);
        public Task<List<Appointment>?> GetAll();
        public Task<Appointment?> GetById(int id);
        public Task<List<Appointment>?> GetByUserId(int userId);
        

    }
}
