using MangoFusion.API.Data;
using MangoFusion.API.Models;
using MangoFusion.API.Models.Dto;
using MangoFusion.API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MangoFusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderHeaderController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApiResponse _apiResponse;

    public OrderHeaderController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _apiResponse = new();
    }

    [HttpGet]
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

    [HttpGet("{orderId:int}")]
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

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                OrderHeader orderHeader = new()
                {
                    PickUpName = dto.PickUpName,
                    PickUpPhoneNumber = dto.PickUpPhoneNumber,
                    PickUpEmail = dto.PickUpEmail,
                    OrderDate = DateTime.Now,
                    OrderTotal = dto.OrderTotal,
                    Status = StaticDetails.StatusConfirmed,
                    TotalItems = dto.TotalItems,
                    ApplicationUserId = dto.ApplicationUserId
                };

                _dbContext.OrderHeaders.Add(orderHeader);
                await _dbContext.SaveChangesAsync();

                // Add order details.
                foreach (OrderDetailsCreateDto orderDetailsDto in dto.OrderDetailsCreateDtos)
                {
                    OrderDetail orderDetail = new()
                    {
                        OrderHeaderId = orderHeader.OrderHeaderId,
                        MenuItemId = orderDetailsDto.MenuItemId,
                        Quantity = orderDetailsDto.Quantity,
                        ItemName = orderDetailsDto.ItemName,
                        Price = orderDetailsDto.Price,
                    };

                    _dbContext.OrderDetails.Add(orderDetail);
                }

                await _dbContext.SaveChangesAsync();

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = orderHeader;
                orderHeader.OrderDetails = await _dbContext.OrderDetails.Where(od => od.OrderHeaderId == orderHeader.OrderHeaderId).ToListAsync();
                return CreatedAtAction(nameof(GetOrder), new { orderId = orderHeader.OrderHeaderId }, _apiResponse);
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

    [HttpPut("{orderId:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateOrder(int orderId, [FromBody] OrderHeaderUpdateDto dto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                OrderHeader? orderHeaderFromDb = await _dbContext.OrderHeaders.FirstOrDefaultAsync(o => o.OrderHeaderId == orderId);

                if (orderHeaderFromDb is null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessage = ["Order not found"];
                    return BadRequest(_apiResponse);
                }

                if (dto.PickUpName is { Length: > 0 })
                {
                    orderHeaderFromDb.PickUpName = dto.PickUpName;
                }

                if (dto.PickUpPhoneNumber is { Length: > 0 })
                {
                    orderHeaderFromDb.PickUpPhoneNumber = dto.PickUpPhoneNumber;
                }

                if (dto.PickUpEmail is { Length: > 0 })
                {
                    orderHeaderFromDb.PickUpEmail = dto.PickUpEmail;
                }

                if (dto.Status is { Length: > 0 })
                {
                    if (orderHeaderFromDb.Status.Equals(StaticDetails.StatusConfirmed, StringComparison.OrdinalIgnoreCase) &&
                        dto.Status.Equals(StaticDetails.StatusReadyForPickUp, StringComparison.OrdinalIgnoreCase))
                    {
                        orderHeaderFromDb.Status = StaticDetails.StatusReadyForPickUp;
                    }

                    if (orderHeaderFromDb.Status.Equals(StaticDetails.StatusReadyForPickUp, StringComparison.OrdinalIgnoreCase) &&
                        dto.Status.Equals(StaticDetails.StatusCompleted, StringComparison.OrdinalIgnoreCase))
                    {
                        orderHeaderFromDb.Status = StaticDetails.StatusCompleted;
                    }

                    if (dto.Status.Equals(StaticDetails.StatusCancelled, StringComparison.OrdinalIgnoreCase))
                    {
                        orderHeaderFromDb.Status = StaticDetails.StatusCancelled;
                    }
                }

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
