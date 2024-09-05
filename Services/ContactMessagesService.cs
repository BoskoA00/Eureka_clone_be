using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace epp_be.Services
{
    public class ContactMessagesService : IContactMessages
    {
        private readonly DatabaseContext _dbContext;

        public ContactMessagesService( DatabaseContext db)
        {
            this._dbContext = db;
        }

        public async Task<Messages2?> CreateMessage(Messages2 message)
        {

            await _dbContext.ContactMessages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            return message;
        }

        public async Task<Messages2?> DeleteMessage(int id)
        {
            Messages2? message = await _dbContext.ContactMessages.Where( message => message.Id == id).FirstOrDefaultAsync();
            if (message != null) {
                _dbContext.ContactMessages.Remove(message);
                await _dbContext.SaveChangesAsync();
                return message;
            }
            return null;
        }

        public async Task<List<Messages2>?> GetAll()
        {
            return await _dbContext.ContactMessages.ToListAsync();
        }
    }
}
