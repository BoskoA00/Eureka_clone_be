using AutoMapper;
using epp_be.Data;
using epp_be.DTOs;
using epp_be.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace epp_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser userService;
        private readonly IMapper mapper;

        public UserController(IUser iu, IMapper mp)
        {
            this.userService = iu;
            this.mapper = mp;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(mapper.Map<List<UserReponseDTO>>(await userService.GetAll()));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(mapper.Map<UserReponseDTO>(await userService.GetUserById(id)));
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            return Ok(mapper.Map<UserReponseDTO>(await userService.GetUserByEmail(email)));
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO request)
        {
            if (userService.TakenEmail(request.email))
            {
                return BadRequest("Email taken");
            }
            if (userService.TakenPassword(request.password))
            {
                return BadRequest("Password taken");
            }

            return Ok(mapper.Map<UserReponseDTO>(await userService.RegisterUser(mapper.Map<User>(request))));
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDTO request)
        {
            if (!userService.TakenEmail(request.email))
            {
                return BadRequest("This user doesnt exist");
            }
            User? user = await userService.LoginUser(request.email, request.password);
            if (user == null)
            {
                return BadRequest("Password not correct");
            }
            var token = userService.GenerateToken(user);

            return Ok(new
            {
                user = mapper.Map<UserReponseDTO>(user),
                token = token

            });
        }
        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!User.Claims.Any())
            {
                return Forbid(); 
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.Parse(userRole) != (int)UserRoles.Admin)
            {
                return Forbid();
            }
            return Ok(mapper.Map<UserReponseDTO>(await userService.DeleteUser(id)));
        }
        [HttpPut("promoteUser/{id}")]
        public async Task<IActionResult> PromoteUser([FromRoute] int id)
        {
            if (!User.Claims.Any())
            {
                return Forbid();
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.Parse(userRole) != (int)UserRoles.Admin)
            {
                return Forbid();
            }


            return Ok(mapper.Map<UserReponseDTO>(await userService.PromoteUser(id)));
        }
        [HttpPut("demoteUser/{id}")]
        public async Task<IActionResult> DemoteUser([FromRoute] int id)
        {
            if (!User.Claims.Any())
            {
                return Forbid();
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.Parse(userRole) != (int)UserRoles.Admin)
            {
                return Forbid();
            }
            return Ok(mapper.Map<UserReponseDTO>(await userService.DemoteUser(id)));
        }
        [HttpPut("promoteToAdmin/{id}")]
        public async Task<IActionResult> PromoteToAdmin([FromRoute] int id)
        {
            if (!User.Claims.Any())
            {
                return Forbid();
            }
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (int.Parse(userRole) != (int)UserRoles.Admin)
            {
                return Forbid();
            }
            return Ok(mapper.Map<UserReponseDTO>(await userService.PromoteToAdmin(id)));
        }
        [HttpPut("alterUser")]
        public async Task<IActionResult> AlterUser([FromBody] UserUpdateRequestDTO request)
        {
            User? user = await userService.GetUserById(request.Id);
            if (user == null)
            {
                return BadRequest("Korisnik ne postoji");
            }
            string userAltered = await userService.AlterUser(request.Id, request.email, request.password);

            return Ok(userAltered);
        }
        [HttpPost("sendPasswordReset")]
        public async Task<IActionResult> SendPasswordReset([FromBody] PasswordResetDTO request)
        {
            var result = await userService.SendResetPasswordEmail(request.Email);
            if (!result)
            {
                return BadRequest("User not found");
            }

            return Ok("Password reset email sent");
        }
        [HttpPut("passwordReset")]
        public async Task<IActionResult> PasswordReset([FromBody] UserUpdateRequestDTO request)
        {
            User? user = await userService.GetUserByEmail(request.email);
            if (user == null) {
                return BadRequest("No such user");
            }
            await userService.AlterPassword(request.Id,request.email, request.password);

            return Ok("Password changed");
        }


    }
}
