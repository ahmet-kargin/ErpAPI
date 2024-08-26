using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpAPI.Domain.Entities;

public class FinancialTransaction
{
    public int TransactionId { get; set; }
    public int OrderId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }

    public Order Order { get; set; } // Eğer Order ilişkisini kullanıyorsanız
}

