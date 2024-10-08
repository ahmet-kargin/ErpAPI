﻿namespace ErpAPI.Domain.Entities;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public Customer Customer { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
}

