using MangoFusion.API.Models;
using MangoFusion.API.Models.Dto;
using MangoFusion.API.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MangoFusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly ApiResponse _apiResponse;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly string _secretKey;

    public AuthController(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _apiResponse = new();
        _userManager = userManager;
        _roleManager = roleManager;
        _secretKey = configuration.GetValue<string>("ApiSettings:Secret")!;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser newUser = new()
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                NormalizedEmail = model.Email.ToUpper()
            };

            // Create new user using UserManager.
            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                // Check if role exists, if not create that role.
                await CheckAndCreateRoles();

                // Now let's assign the role for the user.
                await AssignRoleToTheUser(model, newUser);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            else
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                foreach (var error in result.Errors)
                {
                    _apiResponse.ErrorMessage.Add(error.Description);
                }

                return BadRequest(_apiResponse);
            }
        }
        else
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;

            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    _apiResponse.ErrorMessage.Add(error.ErrorMessage);
                }
            }

            return BadRequest(_apiResponse);
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto model)
    {
        if (ModelState.IsValid)
        {
            var userFromDb = await _userManager.FindByEmailAsync(model.Email);

            if (userFromDb is not null)
            {
                bool isValidUser = await _userManager.CheckPasswordAsync(userFromDb, model.Password);

                if (!isValidUser)
                {
                    _apiResponse.Result = new LoginResponseDto();
                    _apiResponse.IsSuccess = false;
                    _apiResponse.ErrorMessage = ["Invalid credentials"];
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                // Generate JWT token.
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(_secretKey);
                var rolesOfUser = await _userManager.GetRolesAsync(userFromDb);
                var firstRoleOfUser = rolesOfUser.FirstOrDefault();

                SecurityTokenDescriptor securityTokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity
                    ([
                        new Claim("fullName", userFromDb.Name),
                        new Claim("id", userFromDb.Id),
                        new Claim(ClaimTypes.Email, userFromDb.Email!),
                        new Claim(ClaimTypes.Role, firstRoleOfUser!),
                    ]),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenHandler.CreateToken(securityTokenDescriptor);

                LoginResponseDto responseDto = new()
                {
                    Token = tokenHandler.WriteToken(token),
                    Email = userFromDb.Email!,
                    Role = firstRoleOfUser!,
                };

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = responseDto;
                return Ok(_apiResponse);
            }

            _apiResponse.Result = new LoginResponseDto();
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessage = ["Invalid credentials"];
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_apiResponse);
        }
        else
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;

            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    _apiResponse.ErrorMessage.Add(error.ErrorMessage);
                }
            }

            return BadRequest(_apiResponse);
        }
    }

    private async Task CheckAndCreateRoles()
    {
        if (!await _roleManager.RoleExistsAsync(StaticDetails.RoleAdmin))
        {
            await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleAdmin));
            await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleCustomer));
        }
    }

    private async Task AssignRoleToTheUser(RegisterRequestDto model, ApplicationUser newUser)
    {
        if (model.Role.Equals(StaticDetails.RoleAdmin, StringComparison.OrdinalIgnoreCase))
        {
            await _userManager.AddToRoleAsync(newUser, StaticDetails.RoleAdmin);
        }
        else
        {
            await _userManager.AddToRoleAsync(newUser, StaticDetails.RoleCustomer);
        }
    }
}
