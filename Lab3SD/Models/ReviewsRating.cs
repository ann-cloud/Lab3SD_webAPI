using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

[Table("Reviews_Ratings")]
public partial class ReviewsRating
{
    [Key]
    [Column("review_id")]
    public int ReviewId { get; set; }

    // [Column("product_id")]
    // public int ProductId { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("rating")]
    public int? Rating { get; set; }

    [Column("comment", TypeName = "text")]
    public string? Comment { get; set; }

    [Column("date_when_posted")]
    public DateOnly? DateWhenPosted { get; set; }
}
