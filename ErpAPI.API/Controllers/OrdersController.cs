using ErpAPI.Application.Interfaces;
using ErpAPI.Domain.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ErpAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // Bağımlılık enjeksiyonu ile IOrderRepository'yi denetleyiciye dahil eder.
        private readonly IOrderRepository _orderRepository;

        // Constructor metodu, IOrderRepository örneğini alır ve sınıfın _orderRepository alanına atar.
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Tüm siparişleri getirir.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            // Tüm siparişleri veritabanından asenkron olarak alır.
            var orders = await _orderRepository.GetAllOrdersAsync();

            // 200 OK yanıtı ile birlikte sipariş listesini döner.
            return Ok(orders);
        }

        // Belirli bir ID'ye sahip siparişi getirir.
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            // Verilen ID'ye sahip siparişi veritabanından asenkron olarak alır.
            var order = await _orderRepository.GetOrderByIdAsync(id);

            // Eğer sipariş bulunamazsa 404 Not Found yanıtı döner.
            if (order == null)
            {
                return NotFound();
            }

            // 200 OK yanıtı ile birlikte siparişi döner.
            return Ok(order);
        }

        // Yeni bir sipariş oluşturur.
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto orderDto)
        {
            // DTO'dan Order nesnesi oluşturur ve veritabanına asenkron olarak ekler.
            var order = await _orderRepository.CreateOrderAsync(orderDto);

            // 201 Created yanıtı döner ve yeni oluşturulan siparişin URL'sini içerir.
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // Belirli bir ID'ye sahip siparişi günceller.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            // Gönderilen ID ile orderDto içerisindeki ID'nin eşleşip eşleşmediğini kontrol eder.
            if (id != orderDto.OrderId)
            {
                // Eşleşmezse 400 Bad Request yanıtı döner.
                return BadRequest();
            }

            // Sipariş bilgilerini veritabanında asenkron olarak günceller.
            var updated = await _orderRepository.UpdateOrderAsync(orderDto);

            // Eğer güncelleme başarısızsa 404 Not Found yanıtı döner.
            if (!updated)
            {
                return NotFound();
            }

            // Başarılıysa 204 No Content yanıtı döner (başarılı ancak dönecek içerik yok).
            return NoContent();
        }

        // Belirli bir ID'ye sahip siparişi siler.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Siparişi veritabanından asenkron olarak siler.
            var deleted = await _orderRepository.DeleteOrderAsync(id);

            // Eğer silme başarısızsa 404 Not Found yanıtı döner.
            if (!deleted)
            {
                return NotFound();
            }

            // Başarılıysa 204 No Content yanıtı döner.
            return NoContent();
        }
    }
}
