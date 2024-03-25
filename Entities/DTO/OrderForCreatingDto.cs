using Entities.Models;
using System.ComponentModel.DataAnnotations;

public class OrderForCreatingDto
{

    [Required(ErrorMessage = "Customer name is required")]
    public string CustomerName { get; set; }

    [Required(ErrorMessage = "Order date is required")]
    public DateTime OrderDate { get; set; }
    public ICollection<OrderProductForCreatingDto> OrderProducts { get; set; }
}
