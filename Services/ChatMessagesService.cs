using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace epp_be.Services
{
    public class ChatMessagesService : IChatMessages
    {
        private readonly DatabaseContext db;

        public ChatMessagesService(DatabaseContext database)
        {
            this.db = database;
        }

        public async Task<ChatMessage?> CreateMessage(ChatMessage message)
        {
             db.ChatMessages.Add(message);
            await db.SaveChangesAsync();
            return message;
        }

        public async Task<bool> DeleteByUserId(int userId)
        {
           List<ChatMessage>? messages = await db.ChatMessages.Where( chatMessage => chatMessage.SenderId == userId || chatMessage.ReceiverId == userId).ToListAsync();
            db.ChatMessages.RemoveRange(messages);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteById(int id)
        {
            ChatMessage? message = await db.ChatMessages.Where( chatMessage => chatMessage.Id == id).FirstOrDefaultAsync();
            if (message != null)
            {
                db.ChatMessages.Remove(message);
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<List<ChatMessage>?> GetAll()
        {
            return await db.ChatMessages.OrderBy( chatMessage => chatMessage.CreatedAt).ToListAsync();
        }

        public async Task<ChatMessage?> GetById(int id)
        {
            return await db.ChatMessages.Where( chatMessage => chatMessage.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ChatMessage>?> GetByReceiverId(int id)
        {
            return await db.ChatMessages.Where( chatMessage => chatMessage.ReceiverId == id).OrderBy( cm => cm.CreatedAt).ToListAsync();
        }

        public async Task<List<ChatMessage>?> GetBySenderId(int id)
        {
            return await db.ChatMessages.Where( chatMessage => chatMessage.SenderId == id).OrderBy( chatMessage => chatMessage.CreatedAt).ToListAsync();
        }

        public async Task<List<ChatMessage>?> GetByUserId(int id)
        {
            return await db.ChatMessages.Where( chatMessage => chatMessage.ReceiverId == id || chatMessage.SenderId == id).OrderBy( chatMessage => chatMessage.CreatedAt).ToListAsync();
        }

        public async Task<bool> ReadMessage(int id)
        {

            
            ChatMessage? chatMessage = await db.ChatMessages.Where( chatMessage => chatMessage.Id == id).FirstOrDefaultAsync();

            if( chatMessage == null)
            {
                return false;
            }
            chatMessage.Status = 1;
            await db.SaveChangesAsync();
            return true;


        }
    }
}
