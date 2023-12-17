using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

[Table("Payment_Transactions")]
public partial class PaymentTransaction
{
    [Key]
    [Column("transaction_id")]
    public int TransactionId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("amount", TypeName = "decimal(10, 2)")]
    public decimal? Amount { get; set; }

    [Column("payment_method")]
    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentMethod { get; set; }

    [Column("transaction_date")]
    public DateOnly? TransactionDate { get; set; }
}
