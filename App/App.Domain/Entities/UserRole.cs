using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("UserRole")]
public partial class UserRole
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Username { get; set; }

    [Column("RoleID")]
    public int? RoleId { get; set; }
}
