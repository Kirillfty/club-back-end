using System.ComponentModel.DataAnnotations;
using ClubsBack.Entities;
using ClubsBack.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace TwitterBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtCreator _jwtCreator;
        private readonly IRepository<Users> _userRepository;

        public AuthController(JwtCreator jwtCreator, IRepository<Users> userRepository)
        {
            _jwtCreator = jwtCreator;
            _userRepository = userRepository;
        }

        public record LoginRequest([Required] string NickName, [Required] string Password);

        public record RegisterRequest(
            [Required] string FirstName,
            [Required] string LastName,
            [Required] string NickName,
            [Required] string Password);
        
        public record TokenPair(string AccessToken, string RefreshToken);

        [HttpPost("login")]
        public ActionResult<TokenPair> Login([FromBody] LoginRequest loginRequest)
        {
            var users = _userRepository.Get();

            var user = users.FirstOrDefault(u => u.nickName == loginRequest.NickName);

            if (user == null)
                return Unauthorized("Username is incorrect");

            if (user.password != loginRequest.Password)
                return Unauthorized("Password is incorrect");

            string refreshToken = Guid.NewGuid().ToString();
            
            _userRepository.SetRefreshToken1(refreshToken, user.nickName);

            string authToken = _jwtCreator.Create(user.nickName, user.Role);
            
            return new TokenPair(authToken, refreshToken);
        }

        [HttpPost("register")]
        public ActionResult<TokenPair> Register([FromBody] RegisterRequest registerRequest)
        {
            Users newUser = new()
            {
                nickName = registerRequest.NickName,
                firstName = registerRequest.FirstName,
                lastName = registerRequest.LastName,
                password = registerRequest.Password,
                Role = "user"
            };

            if (_userRepository.Post(newUser) == false)
                return BadRequest("User cannot be created");

            string refreshToken = Guid.NewGuid().ToString();
            
            _userRepository.SetRefreshToken1(refreshToken, newUser.nickName);

            string authToken = _jwtCreator.Create(newUser.nickName, newUser.Role);
            
            return new TokenPair(authToken, refreshToken);
        }

        [HttpGet("refresh/{refreshToken}")]
        public ActionResult<TokenPair> Refresh([FromRoute] string refreshToken)
        {
            var users = _userRepository.Get();

            var user = users.FirstOrDefault(u => u.RefreshToken == refreshToken);
            
            if(user == null)
                return Unauthorized("Refresh token is incorrect");
            
            string newRefreshToken = Guid.NewGuid().ToString();
            
            _userRepository.SetRefreshToken1(newRefreshToken, user.nickName);

            string authToken = _jwtCreator.Create(user.nickName, user.Role);
            
            return new TokenPair(authToken, newRefreshToken);
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<Users>> GetAllUsers()
        {
            return _userRepository.Get().ToList();
        }
        
        [Authorize(Roles = "admin")]
        [HttpDelete("{userId:int}")]
        public ActionResult DeleteUser([FromRoute] int userId)
        {
            if (_userRepository.Delete(userId))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}