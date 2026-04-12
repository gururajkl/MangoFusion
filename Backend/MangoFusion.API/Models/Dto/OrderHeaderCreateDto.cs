using System.ComponentModel.DataAnnotations;

namespace MangoFusion.API.Models.Dto;

public class OrderHeaderCreateDto
{
    [Required]
    public string PickUpName { get; set; } = string.Empty;
    [Required]
    public string PickUpPhoneNumber { get; set; } = string.Empty;
    [Required]
    public string PickUpEmail { get; set; } = string.Empty;
    public double OrderTotal { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public List<OrderDetailsCreateDto> OrderDetailsCreateDtos { get; set; } = [];
}
