using MangoFusion.API.Data;
using MangoFusion.API.Models;
using MangoFusion.API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MangoFusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailsController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApiResponse _apiResponse;

    public OrderDetailsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _apiResponse = new();
    }

    [HttpPut("[action]/{orderDetailsId:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateOrderDetails(int orderDetailsId, [FromBody] OrderDetailsUpdateDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                OrderDetail? orderDetailsFromDb = await _dbContext.OrderDetails.FirstOrDefaultAsync(o => o.OrderDetailId == orderDetailsId);

                if (orderDetailsFromDb is null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessage = ["Order details not found"];
                    return BadRequest(_apiResponse);
                }

                orderDetailsFromDb.Rating = dto.Rating;

                await _dbContext.SaveChangesAsync();

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return Ok(_apiResponse);
            }
            else
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessage = [.. ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)];
                return BadRequest(_apiResponse);
            }
        }
        catch (Exception ex)
        {
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessage = [ex.Message];
            return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
        }
    }
}
