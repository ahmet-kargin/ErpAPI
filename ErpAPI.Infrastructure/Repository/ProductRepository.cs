using ErpAPI.Application.Interfaces;
using ErpAPI.Domain.Entities;
using ErpAPI.Infrastructure.Connection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpAPI.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    // Entity Framework Core DbContext sınıfının bir örneğini tutar.
    private readonly ErpAPIDbContext _context;

    // Constructor metodu, DbContext örneğini alır ve sınıfın _context alanına atar.
    public ProductRepository(ErpAPIDbContext context)
    {
        _context = context;
    }

    // Veritabanındaki tüm ürünleri asenkron olarak getirir.
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        // Tüm ürünleri veritabanından getirir ve bir liste olarak döner.
        return await _context.Products.ToListAsync();
    }

    // Belirtilen ID'ye sahip bir ürünü asenkron olarak getirir.
    public async Task<Product> GetProductByIdAsync(int id)
    {
        // Verilen ID'ye sahip ürünü bulur ve döner.
        return await _context.Products.FindAsync(id);
    }

    // Yeni bir ürün oluşturur ve veritabanına ekler.
    public async Task<Product> CreateProductAsync(Product product)
    {
        // Ürün nesnesini veritabanına ekler.
        _context.Products.Add(product);

        // Değişiklikleri veritabanına kaydeder.
        await _context.SaveChangesAsync();
        return product;
    }

    // Mevcut bir ürünü günceller.
    public async Task<bool> UpdateProductAsync(Product product)
    {
        // Verilen ID'ye sahip mevcut ürünü veritabanından bulur.
        var existingProduct = await _context.Products.FindAsync(product.ProductId);
        if (existingProduct == null)
        {
            // Eğer ürün bulunamazsa, false döner.
            return false;
        }

        // Bulunan ürünün özelliklerini günceller.
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockQuantity = product.StockQuantity;

        // Ürünü veritabanında günceller.
        _context.Products.Update(existingProduct);

        // Değişiklikleri veritabanına kaydeder.
        await _context.SaveChangesAsync();

        return true;
    }

    // Belirtilen ID'ye sahip bir ürünü siler.
    public async Task<bool> DeleteProductAsync(int id)
    {
        // Verilen ID'ye sahip ürünü veritabanından bulur.
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            // Eğer ürün bulunamazsa, false döner.
            return false;
        }

        // Ürünü veritabanından kaldırır.
        _context.Products.Remove(product);
        // Değişiklikleri veritabanına kaydeder.
        await _context.SaveChangesAsync();

        // Silme işlemi başarılıysa true döner.
        return true;
    }
}
