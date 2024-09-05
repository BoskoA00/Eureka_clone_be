using epp_be.Data;
using epp_be.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace epp_be.Services
{
    public class FunctionalSkillsMessagesService : IFunctionalSkillsMessages
    {
        private readonly DatabaseContext db;

        public FunctionalSkillsMessagesService( DatabaseContext Db) {

            this.db = Db;    
        }

        public async Task<Messages1?> CreateMessage(Messages1 message)
        {
            await db.FunctionalSkillsMessages.AddAsync(message);
            await db.SaveChangesAsync();
            return message;
        }

        public async Task<Messages1?> DeleteMessage(int id)
        {
              Messages1? messages = await db.FunctionalSkillsMessages.Where( message => message.Id == id).FirstOrDefaultAsync();

            if (messages != null) { 
                db.FunctionalSkillsMessages.Remove(messages);
                await db.SaveChangesAsync();
            return messages;
            }
            return null;
        
        
        }

        public async Task<List<Messages1>?> GetAll()
        {
            return await db.FunctionalSkillsMessages.ToListAsync();
        }
    }
}
