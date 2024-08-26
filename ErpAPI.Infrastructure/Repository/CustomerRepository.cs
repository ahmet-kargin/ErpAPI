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

public class CustomerRepository : ICustomerRepository
{
    // Veritabanı işlemleri için gerekli olan DbContext nesnesi.
    private readonly ErpAPIDbContext _context;

    // Constructor, DbContext bağımlılığını enjekte eder.
    public CustomerRepository(ErpAPIDbContext context)
    {
        _context = context;
    }

    // Tüm müşterileri asenkron olarak getirir.
    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Address = c.Address,
                PhoneNumber = c.PhoneNumber
            })
            .ToListAsync();
    }

    // Belirli bir müşteri kimliği (id) ile asenkron olarak müşteri bilgilerini getirir.
    public async Task<CustomerDto> GetCustomerByIdAsync(int id)
    {
        // Veritabanında belirtilen id'ye sahip müşteri kaydını arar ve CustomerDto'ya dönüştürür.
        var customer = await _context.Customers
            .Where(c => c.CustomerId == id)
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Address = c.Address,
                PhoneNumber = c.PhoneNumber
            })
            .FirstOrDefaultAsync();

        return customer;
    }
    // Yeni bir müşteri oluşturur ve asenkron olarak veritabanına ekler.
    public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
    {
        // CustomerDto'dan yeni bir Customer nesnesi oluşturur.
        var customer = new Customer
        {
            Name = customerDto.Name,
            Email = customerDto.Email,
            Address = customerDto.Address,
            PhoneNumber = customerDto.PhoneNumber
        };

        // Yeni müşteri kaydını veritabanına ekler.
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Veritabanına eklenen müşteri kaydını CustomerDto'ya dönüştürerek geri döner.
        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Address = customer.Address,
            PhoneNumber = customer.PhoneNumber
        };
    }

    // Mevcut bir müşteriyi günceller ve asenkron olarak veritabanında değişiklik yapar.
    public async Task<bool> UpdateCustomerAsync(CustomerDto customerDto)
    {
        // Veritabanında belirtilen id'ye sahip müşteri kaydını arar.
        var customer = await _context.Customers.FindAsync(customerDto.CustomerId);
        if (customer == null)
        {
            return false;
        }

        // Müşteri bilgilerini günceller.
        customer.Name = customerDto.Name;
        customer.Email = customerDto.Email;
        customer.Address = customerDto.Address;
        customer.PhoneNumber = customerDto.PhoneNumber;

        // Güncellenen müşteri kaydını veritabanında günceller.
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();

        return true;
    }

    // Belirli bir müşteri kimliği (id) ile asenkron olarak müşteri kaydını siler.
    public async Task<bool> DeleteCustomerAsync(int id)
    {
        // Veritabanında belirtilen id'ye sahip müşteri kaydını arar.
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return false;
        }

        // Müşteri kaydını veritabanından siler.
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return true;
    }

}
