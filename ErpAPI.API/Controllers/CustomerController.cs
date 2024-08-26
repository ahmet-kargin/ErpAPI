using ErpAPI.Application.Interfaces;
using ErpAPI.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ErpAPI.API.Controllers
{
    [Route("api/[controller]")] //Bu denetleyicinin route yapısını belirler.
    [ApiController] //Bu öznitelik, denetleyiciye API davranışlarını kazandırır (otomatik model doğrulama, kötü istek yanıtları vs.).
    public class CustomersController : ControllerBase
    {
        // Bağımlılık enjeksiyonu ile ICustomerRepository'yi denetleyiciye dahil eder.
        private readonly ICustomerRepository _customerRepository;

        // Constructor metodu, ICustomerRepository örneğini alır ve sınıfın _customerRepository alanına atar.
        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Tüm müşterileri getirir.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            // Tüm müşterileri veritabanından asenkron olarak alır.
            var customers = await _customerRepository.GetAllCustomersAsync();

            // 200 OK yanıtı ile birlikte müşteri listesini döner.
            return Ok(customers);
        }

        // Belirli bir ID'ye sahip müşteriyi getirir.
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            // Verilen ID'ye sahip müşteriyi veritabanından asenkron olarak alır.
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            // Eğer müşteri bulunamazsa 404 Not Found yanıtı döner.
            if (customer == null)
            {
                return NotFound();
            }

            // 200 OK yanıtı ile birlikte müşteriyi döner.
            return Ok(customer);
        }

        // Yeni bir müşteri oluşturur.
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            // Yeni müşteri kaydını veritabanına asenkron olarak ekler.
            var customer = await _customerRepository.CreateCustomerAsync(customerDto);

            //Created yanıtı döner ve yeni oluşturulan müşterinin URL'sini içerir.
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }


        // Belirli bir ID'ye sahip müşteriyi günceller.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            // Gönderilen ID ile müşteriDto içerisindeki ID'nin eşleşip eşleşmediğini kontrol eder.
            if (id != customerDto.CustomerId)
            {
                // Eşleşmezse 400 Bad Request yanıtı döner.
                return BadRequest();
            }

            // Müşteri bilgilerini veritabanında asenkron olarak günceller.
            var updated = await _customerRepository.UpdateCustomerAsync(customerDto);
            // Eğer müşteri bulunamazsa 404 Not Found yanıtı döner.
            if (!updated)
            {
                return NotFound();
            }

            // Başarılıysa 204 No Content yanıtı döner (başarılı ancak dönecek içerik yok).
            return NoContent();
        }

        // Belirli bir ID'ye sahip müşteriyi siler.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            // Müşteri kaydını veritabanından asenkron olarak siler.
            var deleted = await _customerRepository.DeleteCustomerAsync(id);
            // Eğer müşteri bulunamazsa 404 Not Found yanıtı döner.
            if (!deleted)
            {
                return NotFound();
            }

            // Başarılıysa 204 No Content yanıtı döner.
            return NoContent();
        }
    }
}
