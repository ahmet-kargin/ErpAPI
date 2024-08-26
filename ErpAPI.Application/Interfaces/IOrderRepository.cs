using ErpAPI.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpAPI.Application.Interfaces;

public interface IOrderRepository
{
    // Tüm siparişleri asenkron olarak getirir.
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    // Belirli bir sipariş kimliği (orderId) ile asenkron olarak sipariş bilgilerini getirir.
    Task<OrderDto> GetOrderByIdAsync(int orderId);
    // Yeni bir sipariş oluşturur ve asenkron olarak veritabanına ekler.
    Task<OrderDto> CreateOrderAsync(OrderDto orderDto);
    // Mevcut bir siparişi günceller ve asenkron olarak veritabanında değişiklik yapar.
    Task<bool> UpdateOrderAsync(OrderDto orderDto);
    // Belirli bir sipariş kimliği (orderId) ile asenkron olarak sipariş kaydını siler.
    Task<bool> DeleteOrderAsync(int orderId);
}
