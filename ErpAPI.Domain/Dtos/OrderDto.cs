using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpAPI.Domain.Dtos;

public class OrderDto
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<ProductDto> Products { get; set; }
    public decimal TotalAmount { get; set; }
}
