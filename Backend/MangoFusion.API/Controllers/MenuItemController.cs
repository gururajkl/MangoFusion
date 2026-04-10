using MangoFusion.API.Data;
using MangoFusion.API.Models;
using MangoFusion.API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using IOFile = System.IO.File;

namespace MangoFusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApiResponse _apiResponse;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MenuItemController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
        _apiResponse = new ApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetMenuItems()
    {
        _apiResponse.Result = await _dbContext.MenuItems.ToListAsync();
        _apiResponse.StatusCode = HttpStatusCode.OK;
        return Ok(_apiResponse);
    }

    [HttpGet("{id:int}", Name = "GetMenuItem")]
    public async Task<IActionResult> GetMenuItem(int id)
    {
        if (id <= 0)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.ErrorMessage = ["Invalid id"];
            return BadRequest(_apiResponse);
        }

        _apiResponse.Result = await _dbContext.MenuItems.FirstOrDefaultAsync(m => m.Id == id);
        _apiResponse.StatusCode = HttpStatusCode.OK;
        return Ok(_apiResponse);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDto menuItemCreateDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (menuItemCreateDto.File is { Length: <= 0 })
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessage = ["File is required"];
                    return BadRequest(_apiResponse);
                }

                var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var filePath = Path.Combine(imagesPath, menuItemCreateDto.File.FileName);

                Directory.CreateDirectory(imagesPath);

                if (IOFile.Exists(filePath))
                {
                    IOFile.Delete(filePath);
                }

                // Upload the images.
                using FileStream fileStream = new(filePath, FileMode.Create);
                await menuItemCreateDto.File.CopyToAsync(fileStream);

                MenuItem menuItem = new()
                {
                    Name = menuItemCreateDto.Name,
                    Category = menuItemCreateDto.Category,
                    Description = menuItemCreateDto.Description,
                    Price = menuItemCreateDto.Price,
                    SpecialTag = menuItemCreateDto.SpecialTag,
                    Image = $"images/{menuItemCreateDto.File.FileName}"
                };

                _dbContext.MenuItems.Add(menuItem);
                await _dbContext.SaveChangesAsync();

                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = menuItemCreateDto;
                return CreatedAtRoute("GetMenuItem", new { id = menuItem.Id }, _apiResponse);
            }
            else
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = ["Invalid data passed"];
            }
        }
        catch (Exception ex)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessage = [ex.ToString()];
        }

        return BadRequest(_apiResponse);
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDto menuItemUpdateDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (menuItemUpdateDto is null || menuItemUpdateDto.Id != id)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessage = ["Invalid data passed"];
                    return BadRequest(_apiResponse);
                }

                MenuItem? menuItemFromDb = await _dbContext.MenuItems.FirstOrDefaultAsync(m => m.Id == id);

                if (menuItemFromDb is null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.ErrorMessage = ["Invalid id passed"];
                    return NotFound(_apiResponse);
                }

                menuItemFromDb.Name = menuItemUpdateDto.Name;
                menuItemFromDb.Description = menuItemUpdateDto.Description;
                menuItemFromDb.Price = menuItemUpdateDto.Price;
                menuItemFromDb.SpecialTag = menuItemUpdateDto.SpecialTag;

                if (menuItemUpdateDto.File is { Length: > 0 })
                {
                    var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var filePath = Path.Combine(imagesPath, menuItemUpdateDto.File.FileName);

                    Directory.CreateDirectory(imagesPath);

                    if (IOFile.Exists(filePath))
                    {
                        IOFile.Delete(filePath);
                    }

                    // Also delete the old image stored in db.
                    var fileStoredInDb = Path.Combine(_webHostEnvironment.WebRootPath, menuItemFromDb.Image);
                    if (IOFile.Exists(fileStoredInDb))
                    {
                        IOFile.Delete(fileStoredInDb);
                    }

                    // Upload the images.
                    using FileStream fileStream = new(filePath, FileMode.Create);
                    await menuItemUpdateDto.File.CopyToAsync(fileStream);

                    menuItemFromDb.Image = $"images/{menuItemUpdateDto.File.FileName}";

                    _dbContext.Update(menuItemFromDb);
                    await _dbContext.SaveChangesAsync();

                    _apiResponse.StatusCode = HttpStatusCode.NoContent;
                    _apiResponse.IsSuccess = true;

                    return Ok(_apiResponse);
                }
            }
            else
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = ["Invalid data passed"];
            }
        }
        catch (Exception ex)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessage = [ex.ToString()];
        }

        return BadRequest(_apiResponse);
    }
}
