using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface IContactMessages
    {
        public Task<Messages2?> CreateMessage(Messages2 message);
        public Task<Messages2?> DeleteMessage(int id);
        public Task<List<Messages2>?> GetAll();
    }
}
