using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

[Table("Discounts_Promotions")]
public partial class DiscountsPromotion
{
    [Key]
    [Column("discount_id")]
    public int DiscountId { get; set; }

    // [Column("product_id")]
    // public int ProductId { get; set; }

    [Column("discount_percentage")]
    public int? DiscountPercentage { get; set; }

    [Column("start_date")]
    public DateOnly? StartDate { get; set; }

    [Column("end_date")]
    public DateOnly? EndDate { get; set; }
}
