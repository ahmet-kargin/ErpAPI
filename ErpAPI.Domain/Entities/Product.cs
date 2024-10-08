﻿namespace ErpAPI.Domain.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    // Navigation Property
    public ICollection<OrderProduct> OrderProducts { get; set; }
}

