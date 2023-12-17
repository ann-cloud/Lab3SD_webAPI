using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

public partial class Customer
{
    [Key]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("first_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? FirstName { get; set; }

    [Column("last_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? LastName { get; set; }

    [Column("phone")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("address", TypeName = "text")]
    public string? Address { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }
}
