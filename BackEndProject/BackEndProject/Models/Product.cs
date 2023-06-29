﻿using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Count { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public List<Image>? Images { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
