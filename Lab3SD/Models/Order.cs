using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

public partial class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("order_date")]
    public DateOnly? OrderDate { get; set; }

    [Column("status")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column("total_sum", TypeName = "decimal(10, 2)")]
    public decimal? TotalSum { get; set; }

    [Column("seller_user_id")]
    public int SellerUserId { get; set; }
    
}
