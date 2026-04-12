using MangoFusion.API.Data;
using MangoFusion.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MangoFusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApiResponse _apiResponse;

    public OrderController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _apiResponse = new();
    }

    [HttpGet("[action]")]
    public ActionResult<ApiResponse> GetOrders(string userId = "")
    {
        IEnumerable<OrderHeader> orderHeaders = _dbContext.OrderHeaders.Include(o => o.OrderDetails).ThenInclude(o => o.MenuItem)
            .OrderByDescending(o => o.OrderHeaderId);

        if (userId is { Length: > 0 })
        {
            orderHeaders = orderHeaders.Where(o => o.ApplicationUserId == userId);
        }

        _apiResponse.Result = orderHeaders;
        _apiResponse.StatusCode = HttpStatusCode.OK;
        return Ok(_apiResponse);
    }

    [HttpGet("[action]/{orderId:int}")]
    public async Task<ActionResult<ApiResponse>> GetOrder(int orderId)
    {
        if (orderId is 0)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.ErrorMessage = ["Invalid order id"];
            return BadRequest(_apiResponse);
        }

        OrderHeader? order = await _dbContext.OrderHeaders.Include(o => o.OrderDetails).ThenInclude(o => o.MenuItem)
            .FirstOrDefaultAsync(o => o.OrderHeaderId == orderId);

        if (order is null)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.ErrorMessage = ["Order not found"];
            return BadRequest(_apiResponse);
        }

        _apiResponse.Result = order;
        _apiResponse.StatusCode = HttpStatusCode.OK;
        return Ok(_apiResponse);
    }
}
