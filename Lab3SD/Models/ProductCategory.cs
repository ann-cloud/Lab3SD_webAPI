using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

[Table("Product_Categories")]
public partial class ProductCategory
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Name { get; set; }

}
