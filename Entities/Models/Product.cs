using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Supplier { get; set; }
        public double Price { get; set; }
        public required string ImageUrl { get; set; }
    }
}
