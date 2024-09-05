using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface IFunctionalSkillsMessages
    {
        public Task<Messages1?> CreateMessage(Messages1 message);
        public Task<Messages1?> DeleteMessage(int id);
        public Task<List<Messages1>?> GetAll();
    }
}
