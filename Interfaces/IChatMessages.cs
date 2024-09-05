using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface IChatMessages
    {
        public Task<List<ChatMessage>?> GetAll();
        public Task<ChatMessage?> GetById(int id);
        public Task<List<ChatMessage>?> GetBySenderId(int id);
        public Task<List<ChatMessage>?> GetByReceiverId(int id);
        public Task<List<ChatMessage>?> GetByUserId(int id);
        public Task<bool> DeleteByUserId(int userId);
        public Task<bool> DeleteById(int id);
        public Task<ChatMessage?> CreateMessage(ChatMessage message);
        public Task<bool> ReadMessage(int id);
    }
}
