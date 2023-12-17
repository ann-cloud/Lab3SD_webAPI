using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

public partial class Supplier
{
    [Key]
    [Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("contact_info", TypeName = "text")]
    public string? ContactInfo { get; set; }

    [Column("address", TypeName = "text")]
    public string? Address { get; set; }
}
