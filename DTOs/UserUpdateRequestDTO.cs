using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace epp_be.DTOs
{
    public class UserUpdateRequestDTO
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
