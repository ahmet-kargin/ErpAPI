using ErpAPI.Domain.Entities;

namespace ErpAPI.Application.Interfaces;

public interface IProductRepository
{
    // Tüm ürünleri asenkron olarak getirir.
    Task<IEnumerable<Product>> GetAllProductsAsync();
    // Belirli bir ürün kimliği (id) ile asenkron olarak ürün bilgilerini getirir.
    Task<Product> GetProductByIdAsync(int id);
    // Yeni bir ürün oluşturur ve asenkron olarak veritabanına ekler.
    Task<Product> CreateProductAsync(Product product);
    // Mevcut bir ürünü günceller ve asenkron olarak veritabanında değişiklik yapar.
    Task<bool> UpdateProductAsync(Product product);
    // Belirli bir ürün kimliği (id) ile asenkron olarak ürün kaydını siler.
    Task<bool> DeleteProductAsync(int id);
}
