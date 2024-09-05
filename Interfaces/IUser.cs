using epp_be.Data;

namespace epp_be.Interfaces
{
    public interface IUser
    {
        public Task<User?> RegisterUser(User user);
        public Task<User?> LoginUser(string email,string password);
        public Task<List<User>> GetAll();
        public Task<User?> GetUserById(int id);
        public Task<User?> GetUserByEmail(string email);
        public string HashPassword(string password);
        public string GenerateToken(User user);
        public bool TakenEmail(string email);
        public bool TakenPassword(string password);
        public Task<User?> DeleteUser(int id);
        public Task<User?> PromoteUser(int userId);
        public Task<User?> DemoteUser(int userId);
        public Task<User?> PromoteToAdmin(int userId);
        public Task<string> AlterUser(int userId, string email, string password);
        public Task<bool> SendResetPasswordEmail(string email);
        public Task<bool> AlterPassword(int id,string email,string password);
    }
}
