using epp_be.Data;
using epp_be.Interfaces;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace epp_be.Services
{
    public class UserService : IUser
    {
        private readonly DatabaseContext db;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public UserService(DatabaseContext Db, IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.db = Db;
            this.configuration = configuration;
            this.environment = environment;
        }

        public async Task<bool> AlterPassword(int id, string email, string password)
        {
            User? user = await db.Users.Where(user => user.Id == id && user.email == email).FirstOrDefaultAsync();
            if (user == null) {
                return false;
            }
            bool passwordCheck = this.TakenPassword(password);
            if (passwordCheck)
            {
                return false;
            }
            user.password = this.HashPassword(password);
            await db.SaveChangesAsync();
            return true;

        }

        public async Task<string> AlterUser(int userId, string email, string password)
        {
            User? user = await db.Users.Where( user => user.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return "User does not exist";
            }
            bool emailCheck = this.TakenEmail(email);
            if (emailCheck) {
                return "Email taken";
            }
            if (!string.IsNullOrEmpty(password))
            {
                bool passwordCheck = this.TakenPassword(password);
                if (passwordCheck) {
                    return "Password taken";
                }
            }
            user.email = email;
            if (!string.IsNullOrEmpty(password)) {
                user.password = HashPassword(password);
            }
            await db.SaveChangesAsync();
            return "User updated successfully";

        }



        public async Task<User?> DeleteUser(int id)
        {
            User? user = await db.Users.Where( user => user.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            List<UserCourse> enrollments = await db.UserCourses.Where( enrollment => enrollment.userId == user.Id).ToListAsync();
            List<Course> courses = await db.Courses.Where( course => course.profesorId == user.Id).ToListAsync();
            List<Appointment> appoinments = await db.Appointments.Where( appointment => appointment.userId == user.Id).ToListAsync();
            List<Payments> payments = await db.Payments.Where( payment => payment.email == user.email).ToListAsync();
            List<ChatMessage> chatMessages = await db.ChatMessages.Where( message => message.SenderId == id || message.ReceiverId == id).ToListAsync();
            db.ChatMessages.RemoveRange(chatMessages);
            db.Payments.RemoveRange(payments);
            db.Appointments.RemoveRange(appoinments);
            db.UserCourses.RemoveRange(enrollments);
            foreach (Course course in courses)
            {
                List<Lesson>? lessons = await db.Lessons.Where( lesson => lesson.courseId == course.Id).ToListAsync();
                db.RemoveRange(lessons);
                List<UserCourse>? enrollmentsList = await db.UserCourses.Where( enrollment => enrollment.courseId == course.Id).ToListAsync();
                db.UserCourses.RemoveRange(enrollmentsList);

                string courseFolder = Path.Combine(environment.WebRootPath, "videos", course.Id.ToString());
                if (Directory.Exists(courseFolder))
                {
                    Directory.Delete(courseFolder, true);
                }

                db.Courses.Remove(course);
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DemoteUser(int userId)
        {
            User? user = await db.Users.Where( user => user.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            user.role = 0;
            List<Course>? courses = await db.Courses.Where( course => course.profesorId == userId ).ToListAsync();
            foreach (Course course in courses)
            {
                string courseFolder = Path.Combine(environment.WebRootPath, "videos", course.Id.ToString());
                if (Directory.Exists(courseFolder))
                {
                    Directory.Delete(courseFolder, true);
                }
                List<Lesson>? lessons = await db.Lessons.Where( lesson => lesson.courseId == course.Id).ToListAsync();
                db.Lessons.RemoveRange(lessons);
                List<UserCourse?>? enrollments = await db.UserCourses.Where( user => user.courseId == course.Id).ToListAsync();
                db.UserCourses.RemoveRange(enrollments);
                db.Courses.Remove(course);
            }
            await db.SaveChangesAsync();
            return user;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Auth:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public async Task<List<User>> GetAll()
        {
            return await db.Users.Include( user => user.Appointments).Include( user => user.Payments).ToListAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await db.Users.Where( user => user.email == email).Include( user => user.Appointments).Include( user => user.Payments).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await db.Users.Where( user => user.Id == id).Include(user => user.Appointments).Include(user => user.Payments).FirstOrDefaultAsync();
        }

        public string HashPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }

        public async Task<User?> LoginUser(string email, string password)
        {
            User? user = await db.Users.Where(user => user.email == email).Include(user => user.Appointments).Include(user => user.Payments).FirstOrDefaultAsync();
            if( user == null)
            {
                return null;
            }
            if( user.password == HashPassword(password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<User?> PromoteToAdmin(int userId)
        {
            User? user = await db.Users.Where( user => user.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            user.role = 2;
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> PromoteUser(int userId)
        {
            User? user = await db.Users.Where( user => user.Id == userId).FirstOrDefaultAsync();
            if (user == null) {
                return null;
            }
            user.role = 1;
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> RegisterUser(User user)
        {
           if(!TakenEmail(user.email) && !TakenPassword(user.password))
            {
                user.password = HashPassword(user.password);

                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();

                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SendResetPasswordEmail(string email)
        {
            try
            {
                User? user = await db.Users.Where( user => user.email == email).FirstOrDefaultAsync();
                if (user == null)
                {
                    return false;
                }

                var token = GenerateToken(user);
                string resetLink = $"**/{user.email}/{token}";

           

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("**", "**"));
                message.To.Add(new MailboxAddress("", user.email));
                message.Subject = "Reset password";
                message.Body = new TextPart("plain") { Text = $"Reset link is : {resetLink}" };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("**", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("**", "**");
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
               catch (Exception ex)
            {
                Console.WriteLine($"Error sending password reset email: {ex.Message}");
                return false;
            }
        }



        public bool TakenEmail(string email)
        {
              User? user =  db.Users.Where( user => user.email == email).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            else
                return true; 
        }

        public  bool TakenPassword(string password)
        {
            string hashedPassword = HashPassword(password);
            User? user = db.Users.Where( user => user.password == hashedPassword).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            else
                return true;
            
        }
    }
}
