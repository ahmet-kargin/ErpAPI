using ErpAPI.Domain.Dtos;
using ErpAPI.Domain.Entities;

namespace ErpAPI.Application.Interfaces;

public interface IFinancialTransactionRepository
{
    // Tüm finansal işlemleri asenkron olarak getirir.
    Task<IEnumerable<FinancialTransaction>> GetAllFinancialTransactionsAsync();
    // Belirli bir finansal işlem kimliği (id) ile asenkron olarak finansal işlem bilgisini getirir.
    Task<FinancialTransaction> GetFinancialTransactionByIdAsync(int id);
    // Yeni bir finansal işlem oluşturur ve asenkron olarak veritabanına ekler.
    Task<FinancialTransaction> CreateFinancialTransactionAsync(FinancialTransaction financialTransaction);
    // Mevcut bir finansal işlemi günceller ve asenkron olarak veritabanında değişiklik yapar.
    Task<bool> UpdateFinancialTransactionAsync(FinancialTransaction financialTransaction);
    // Belirli bir finansal işlem kimliği (id) ile asenkron olarak finansal işlem kaydını siler.
    Task<bool> DeleteFinancialTransactionAsync(int id);
}
