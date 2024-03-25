using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Order total is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Order total must be a positive number")]
        public double OrderTotal { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; } // Represents the products in the order
    }
}
