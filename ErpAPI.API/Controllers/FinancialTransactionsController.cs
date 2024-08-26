using ErpAPI.Application.Interfaces;
using ErpAPI.Domain.Dtos;
using ErpAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErpAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialTransactionsController : ControllerBase
    {
        // Bağımlılık enjeksiyonu ile IFinancialTransactionRepository'yi denetleyiciye dahil eder.
        private readonly IFinancialTransactionRepository _financialTransactionRepository;

        // Constructor metodu, IFinancialTransactionRepository örneğini alır ve sınıfın _financialTransactionRepository alanına atar.
        public FinancialTransactionsController(IFinancialTransactionRepository financialTransactionRepository)
        {
            _financialTransactionRepository = financialTransactionRepository;
        }

        // Tüm finansal işlemleri getirir.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialTransactionDto>>> GetFinancialTransactions()
        {
            // Tüm finansal işlemleri veritabanından asenkron olarak alır.
            var transactions = await _financialTransactionRepository.GetAllFinancialTransactionsAsync();

            // 200 OK yanıtı ile birlikte işlem listesini döner.
            return Ok(transactions);
        }

        // Belirli bir ID'ye sahip finansal işlemi getirir.
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialTransactionDto>> GetFinancialTransaction(int id)
        {
            // Verilen ID'ye sahip finansal işlemi veritabanından asenkron olarak alır.
            var transaction = await _financialTransactionRepository.GetFinancialTransactionByIdAsync(id);

            // Eğer işlem bulunamazsa 404 Not Found yanıtı döner.
            if (transaction == null)
            {
                return NotFound();
            }

            // 200 OK yanıtı ile birlikte işlemi döner.
            return Ok(transaction);
        }

        // Yeni bir finansal işlem oluşturur.
        [HttpPost]
        public async Task<ActionResult<FinancialTransactionDto>> CreateFinancialTransaction([FromBody] FinancialTransactionDto transactionDto)
        {
            // Gönderilen DTO'nun null olup olmadığını kontrol eder. Eğer null ise 400 Bad Request döner.
            if (transactionDto == null)
            {
                return BadRequest();
            }

            // DTO'dan FinancialTransaction nesnesi oluşturur.
            var financialTransaction = new FinancialTransaction
            {
                OrderId = transactionDto.OrderId,
                TransactionDate = transactionDto.TransactionDate,
                TransactionType = transactionDto.TransactionType,
                Amount = transactionDto.Amount
            };

            // Yeni finansal işlemi veritabanına asenkron olarak ekler.
            var createdTransaction = await _financialTransactionRepository.CreateFinancialTransactionAsync(financialTransaction);

            // Oluşturulan işlemden yeni bir DTO oluşturur.
            var createdTransactionDto = new FinancialTransactionDto
            {
                TransactionId = createdTransaction.TransactionId,
                OrderId = createdTransaction.OrderId,
                TransactionDate = createdTransaction.TransactionDate,
                TransactionType = createdTransaction.TransactionType,
                Amount = createdTransaction.Amount
            };

            // 201 Created yanıtı döner ve yeni oluşturulan işlemin URL'sini içerir.
            return CreatedAtAction(nameof(GetFinancialTransaction), new { id = createdTransaction.TransactionId }, createdTransactionDto);
        }

        // Belirli bir ID'ye sahip finansal işlemi günceller.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFinancialTransaction(int id, [FromBody] FinancialTransactionDto transactionDto)
        {
            // Gönderilen ID ile transactionDto içerisindeki ID'nin eşleşip eşleşmediğini kontrol eder.
            if (id != transactionDto.TransactionId)
            {
                // Eşleşmezse 400 Bad Request yanıtı döner.
                return BadRequest();
            }

            // DTO'dan FinancialTransaction nesnesi oluşturur.
            var financialTransaction = new FinancialTransaction
            {
                TransactionId = transactionDto.TransactionId,
                OrderId = transactionDto.OrderId,
                TransactionDate = transactionDto.TransactionDate,
                TransactionType = transactionDto.TransactionType,
                Amount = transactionDto.Amount
            };

            // Finansal işlem bilgilerini veritabanında asenkron olarak günceller.
            var updated = await _financialTransactionRepository.UpdateFinancialTransactionAsync(financialTransaction);
            // Eğer işlem bulunamazsa 404 Not Found yanıtı döner.
            if (!updated)
            {
                return NotFound();
            }

            // Başarılıysa 204 No Content yanıtı döner (başarılı ancak dönecek içerik yok).
            return NoContent();
        }

        // Belirli bir ID'ye sahip finansal işlemi siler.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancialTransaction(int id)
        {
            // Finansal işlem kaydını veritabanından asenkron olarak siler.
            var deleted = await _financialTransactionRepository.DeleteFinancialTransactionAsync(id);
            // Eğer işlem bulunamazsa 404 Not Found yanıtı döner.
            if (!deleted)
            {
                return NotFound();
            }

            // Başarılıysa 204 No Content yanıtı döner.
            return NoContent();
        }
    }
}
