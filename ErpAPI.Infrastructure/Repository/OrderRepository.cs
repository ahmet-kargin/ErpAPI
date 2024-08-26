using ErpAPI.Application.Interfaces;
using ErpAPI.Domain.Dtos;
using ErpAPI.Domain.Entities;
using ErpAPI.Infrastructure.Connection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpAPI.Infrastructure.Repository;

public class OrderRepository : IOrderRepository
{
    // Veritabanı işlemleri için gerekli olan DbContext nesnesi.
    private readonly ErpAPIDbContext _context;

    // Constructor, DbContext bağımlılığını enjekte eder.
    public OrderRepository(ErpAPIDbContext context)
    {
        _context = context;
    }

    // Tüm siparişleri asenkron olarak getirir.
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        // Tüm siparişleri, müşteri ve siparişe ait ürünlerle birlikte veritabanından alır.
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();

        // Sipariş verilerini OrderDto'ya dönüştürerek geri döner.
        return orders.Select(order => new OrderDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            Products = order.OrderProducts.Select(op => new ProductDto
            {
                ProductId = op.ProductId,
                Name = op.Product.Name,
                Price = op.Product.Price
            }).ToList(),
            TotalAmount = order.TotalAmount
        });
    }

    // Belirli bir sipariş kimliği (orderId) ile asenkron olarak sipariş bilgilerini getirir.
    public async Task<OrderDto> GetOrderByIdAsync(int orderId)
    {
        // Veritabanında belirtilen orderId'ye sahip sipariş kaydını, müşteri ve siparişe ait ürünlerle birlikte arar
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
        {
            // Eğer sipariş bulunamazsa null döner.
            return null;
        }

        // Sipariş verilerini OrderDto'ya dönüştürerek geri döner.
        return new OrderDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            Products = order.OrderProducts.Select(op => new ProductDto
            {
                ProductId = op.ProductId,
                Name = op.Product.Name,
                Price = op.Product.Price
            }).ToList(),
            TotalAmount = order.TotalAmount
        };
    }

    // Yeni bir sipariş oluşturur ve asenkron olarak veritabanına ekler.
    public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
    {
        // Yeni sipariş nesnesi oluşturur ve DTO'dan gelen verileri bu nesneye atar.
        var order = new Order
        {
            CustomerId = orderDto.CustomerId,
            OrderDate = orderDto.OrderDate,
            OrderProducts = orderDto.Products.Select(p => new OrderProduct
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList(),
            TotalAmount = orderDto.Products.Sum(p => p.Price * p.Quantity)
        };

        // Yeni siparişi veritabanına ekler.
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Sipariş kimliğini (OrderId) DTO'ya atar ve geri döner.
        orderDto.OrderId = order.OrderId;
        return orderDto;
    }

    // Mevcut bir siparişi günceller ve asenkron olarak veritabanında değişiklik yapar.
    public async Task<bool> UpdateOrderAsync(OrderDto orderDto)
    {
        // Veritabanında belirtilen orderId'ye sahip sipariş kaydını arar.
        var order = await _context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.OrderId == orderDto.OrderId);

        if (order == null)
        {
            // Eğer sipariş bulunamazsa false döner.
            return false;
        }

        // Sipariş verilerini DTO'dan gelen verilerle günceller.
        order.CustomerId = orderDto.CustomerId;
        order.OrderDate = orderDto.OrderDate;
        order.TotalAmount = orderDto.Products.Sum(p => p.Price * p.Quantity);

        // Mevcut sipariş ürünlerini veritabanından kaldırır.
        _context.OrderProducts.RemoveRange(order.OrderProducts);

        // Yeni sipariş ürünlerini ekler.
        order.OrderProducts = orderDto.Products.Select(p => new OrderProduct
        {
            ProductId = p.ProductId,
            Quantity = p.Quantity
        }).ToList();

        // Değişiklikleri veritabanında kaydeder.
        await _context.SaveChangesAsync();
        return true;
    }

    // Belirli bir sipariş kimliği (orderId) ile asenkron olarak sipariş kaydını siler.
    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        // Veritabanında belirtilen orderId'ye sahip sipariş kaydını arar.
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            // Eğer sipariş bulunamazsa false döner.
            return false;
        }

        // Sipariş kaydını veritabanından siler.
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}
