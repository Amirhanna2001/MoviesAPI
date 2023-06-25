using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        // private readonly Jwt _jwt;
        public AuthenticationController(UserManager<IdentityUser> userManager, 
        IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            //  _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterationResultDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (_userManager.FindByEmailAsync(dto.Email).Result != null)
            {
                return BadRequest(new AuthResult
                {
                    Succeeded = false,
                    Errors = new() { "Email is Already Exsits"}
                });
            }
                

            if (_userManager.FindByNameAsync(dto.Username).Result != null )
            {
                return BadRequest(new AuthResult
                {
                    Succeeded = false,
                    Errors = new() { "Username is Already Exsits" }
                });
            }

            IdentityUser user = new() { Email = dto.Email, UserName = dto.Username };
            IdentityResult result = await _userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded)
            {
                AuthResult authResult = new() { Succeeded =false,Errors = new() { "Error"} };
                
                return BadRequest(authResult);
            }
            return Ok(new AuthResult() { Succeeded = true,Token = GenerateToken(user)});
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult>Login(UserLoginDto dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest();

            IdentityUser user =await _userManager.FindByEmailAsync(dto.Email);

            if(user == null || !_userManager.CheckPasswordAsync(user,dto.Password).Result)
                return BadRequest(new AuthResult() { Errors = new() { "Invalid Email Or Password"},Succeeded = false });

            return Ok(new AuthResult() { Token = GenerateToken(user),Succeeded = true});
        }
        private string GenerateToken(IdentityUser user)
        {

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:key").Value);
            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim ("id",user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,new Guid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString()),

                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
            };
            return jwtSecurityTokenHandler.WriteToken(jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor));
        }


    }
}
