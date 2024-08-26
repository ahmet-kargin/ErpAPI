using ErpAPI.Application.Interfaces;
using ErpAPI.Domain.Dtos;
using ErpAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ErpAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Bağımlılık enjeksiyonu ile IProductRepository'yi denetleyiciye dahil eder.
        private readonly IProductRepository _productRepository;

        // Constructor metodu, IProductRepository örneğini alır ve sınıfın _productRepository alanına atar.
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Tüm ürünleri getirir.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            // Tüm ürünleri veritabanından asenkron olarak alır.
            var products = await _productRepository.GetAllProductsAsync();

            // 200 OK yanıtı ile birlikte ürün listesini döner.
            return Ok(products);
        }

        // Belirli bir ID'ye sahip ürünü getirir.
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            // Verilen ID'ye sahip ürünü veritabanından asenkron olarak alır.
            var product = await _productRepository.GetProductByIdAsync(id);

            // Eğer ürün bulunamazsa 404 Not Found yanıtı döner.
            if (product == null)
            {
                return NotFound();
            }

            // 200 OK yanıtı ile birlikte ürünü döner.
            return Ok(product);
        }

        // Yeni bir ürün oluşturur.
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productDto)
        {


            // DTO'dan Product nesnesi oluşturur.
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.Quantity
            };

            // Ürünü veritabanına asenkron olarak ekler.
            var createdProduct = await _productRepository.CreateProductAsync(product);

            // Oluşturulan ürünün DTO nesnesini hazırlar.
            var createdProductDto = new ProductDto
            {
                ProductId = createdProduct.ProductId,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                Quantity = createdProduct.StockQuantity
            };

            // 201 Created yanıtı döner ve yeni oluşturulan ürünün URL'sini içerir.
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProductDto);
        }

        // Belirli bir ID'ye sahip ürünü günceller.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            // Gönderilen ID ile productDto içerisindeki ID'nin eşleşip eşleşmediğini kontrol eder.
            if (id != productDto.ProductId)
            {
                // Eşleşmezse 400 Bad Request yanıtı döner.
                return BadRequest();
            }

            // DTO'dan Product nesnesi oluşturur.
            var product = new Product
            {
                ProductId = productDto.ProductId,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.Quantity
            };

            // Ürün bilgilerini veritabanında asenkron olarak günceller.
            var updated = await _productRepository.UpdateProductAsync(product);

            // Eğer güncelleme başarısızsa 404 Not Found yanıtı döner.
            if (!updated)
            {
                return NotFound();
            }

            // Başarılıysa 204 No Content yanıtı döner (başarılı ancak dönecek içerik yok).
            return NoContent();
        }

        // Belirli bir ID'ye sahip ürünü siler.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Ürünü veritabanından asenkron olarak siler.
            var deleted = await _productRepository.DeleteProductAsync(id);

            // Eğer silme işlemi başarısızsa 404 Not Found yanıtı döner.
            if (!deleted)
            {
                return NotFound();
            }

            // Başarılıysa 204 No Content yanıtı döner.
            return NoContent();
        }
    }
}

