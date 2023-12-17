using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Models;

[Table("User_Roles")]
public partial class UserRole
{
    [Key]
    [Range(1, 10, ErrorMessage = "User Role id is out of range")]
    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("role")]
    [StringLength(45 , ErrorMessage = "User Role is too long")]
    [Text(ErrorMessage = "The field must be a non-empty text.")]
    [Unicode(false)]
    public string? Role { get; set; }
}
