using MangoFusion.API.Models;
using MangoFusion.API.Models.Dto;
using MangoFusion.API.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
