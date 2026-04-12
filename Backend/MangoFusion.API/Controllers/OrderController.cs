using MangoFusion.API.Data;
using MangoFusion.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MangoFusion.API.Controllers;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApiResponse _apiResponse;

    public OrderController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _apiResponse = new();
    }
}
