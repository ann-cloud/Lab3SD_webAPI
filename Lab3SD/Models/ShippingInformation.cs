using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

[Table("Shipping_Information")]
public partial class ShippingInformation
{
    [Key]
    [Column("shipping_id")]
    public int ShippingId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("shipping_address", TypeName = "text")]
    public string? ShippingAddress { get; set; }

    [Column("shipping_method")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ShippingMethod { get; set; }

    [Column("tracking_number")]
    [StringLength(100)]
    [Unicode(false)]
    public string? TrackingNumber { get; set; }
}
