using ErpAPI.Application.Interfaces;
using ErpAPI.Domain.Dtos;
using ErpAPI.Domain.Entities;
using ErpAPI.Infrastructure.Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpAPI.Infrastructure.Repository;

public class FinancialTransactionRepository : IFinancialTransactionRepository
{
    // Veritabanı işlemleri için gerekli olan DbContext nesnesi.
    private readonly ErpAPIDbContext _context;

    // Constructor, DbContext bağımlılığını enjekte eder.
    public FinancialTransactionRepository(ErpAPIDbContext context)
    {
        _context = context;
    }

    // Tüm finansal işlemleri asenkron olarak getirir.
    public async Task<IEnumerable<FinancialTransaction>> GetAllFinancialTransactionsAsync()
    {
        // Veritabanından tüm finansal işlem kayıtlarını alır ve liste olarak döner.
        return await _context.FinancialTransactions.ToListAsync();
    }

    // Belirli bir işlem kimliği (id) ile asenkron olarak finansal işlem bilgilerini getirir.
    public async Task<FinancialTransaction> GetFinancialTransactionByIdAsync(int id)
    {
        // Veritabanında belirtilen id'ye sahip finansal işlem kaydını arar.
        return await _context.FinancialTransactions
            .FirstOrDefaultAsync(ft => ft.TransactionId == id);
    }

    // Yeni bir finansal işlem oluşturur ve asenkron olarak veritabanına ekler.
    public async Task<FinancialTransaction> CreateFinancialTransactionAsync(FinancialTransaction financialTransaction)
    {
        // Yeni finansal işlem kaydını veritabanına ekler.
        _context.FinancialTransactions.Add(financialTransaction);
        await _context.SaveChangesAsync();

        // Veritabanına eklenen finansal işlem kaydını geri döner.
        return financialTransaction;
    }

    // Mevcut bir finansal işlemi günceller ve asenkron olarak veritabanında değişiklik yapar.
    public async Task<bool> UpdateFinancialTransactionAsync(FinancialTransaction financialTransaction)
    {
        // Veritabanında belirtilen id'ye sahip finansal işlem kaydını arar.
        var existingTransaction = await _context.FinancialTransactions
            .FirstOrDefaultAsync(ft => ft.TransactionId == financialTransaction.TransactionId);

        if (existingTransaction == null)
        {
            // Eğer işlem bulunamazsa false döner.
            return false;
        }

        // Finansal işlem bilgilerini günceller.
        existingTransaction.OrderId = financialTransaction.OrderId;
        existingTransaction.TransactionDate = financialTransaction.TransactionDate;
        existingTransaction.TransactionType = financialTransaction.TransactionType;
        existingTransaction.Amount = financialTransaction.Amount;

        // Güncellenen finansal işlem kaydını veritabanında kaydeder.
        await _context.SaveChangesAsync();
        return true;
    }

    // Belirli bir işlem kimliği (id) ile asenkron olarak finansal işlem kaydını siler.
    public async Task<bool> DeleteFinancialTransactionAsync(int id)
    {
        // Veritabanında belirtilen id'ye sahip finansal işlem kaydını arar.
        var transaction = await _context.FinancialTransactions
            .FirstOrDefaultAsync(ft => ft.TransactionId == id);

        if (transaction == null)
        {
            // Eğer işlem bulunamazsa false döner.
            return false;
        }

        // Finansal işlem kaydını veritabanından siler.
        _context.FinancialTransactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }
}
