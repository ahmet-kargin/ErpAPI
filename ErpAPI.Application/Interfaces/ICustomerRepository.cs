using ErpAPI.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpAPI.Application.Interfaces;

public  interface ICustomerRepository
{
    // Tüm müşterileri asenkron olarak getirir.
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    // Belirli bir müşteri kimliği (id) ile asenkron olarak müşteri bilgilerini getirir.
    Task<CustomerDto> GetCustomerByIdAsync(int id);
    // Yeni bir müşteri oluşturur ve asenkron olarak veritabanına ekler.
    Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto);
    // Mevcut bir müşteriyi günceller ve asenkron olarak veritabanında değişiklik yapar.
    Task<bool> UpdateCustomerAsync(CustomerDto customerDto);
    // Mevcut bir müşteriyi siler ve asenkron olarak veritabanında değişiklik yapar.
    Task<bool> DeleteCustomerAsync(int id);
}
