using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

public partial class Product
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    // [Column("category_id")]
    // public int? CategoryId { get; set; }

    [Column("price", TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    [Column("quantity_in_stock")]
    public int? QuantityInStock { get; set; }

    // [Column("supplier_id")]
    // public int SupplierId { get; set; }

    [Column("date_when_added")]
    public DateOnly? DateWhenAdded { get; set; }
}
