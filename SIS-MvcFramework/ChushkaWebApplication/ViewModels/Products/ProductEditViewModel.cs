﻿using ChushkaWebApplication.Models.Enums;

namespace ChushkaWebApplication.ViewModels.Products
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }
}
