using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

[Table("Order_Items")]
public partial class OrderItem
{
    [Key]
    [Column("order_item_id")]
    public int OrderItemId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    // [Column("product_id")]
    // public int ProductId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }
}
