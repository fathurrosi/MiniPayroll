using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("User")]
public partial class User
{
    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? Password { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    public bool? IsLogin { get; set; }

    [Column("IPAddress")]
    [StringLength(15)]
    [Unicode(false)]
    public string? Ipaddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? MachineName { get; set; }

    public bool? IsActive { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? RefreshToken { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RefreshTokenExpires { get; set; }
}
