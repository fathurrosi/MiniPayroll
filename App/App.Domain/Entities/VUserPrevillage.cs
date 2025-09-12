using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Keyless]
public partial class VUserPrevillage
{
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Role { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Menu { get; set; } = null!;

    [Column("MenuID")]
    public int MenuId { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    public bool? AllowCreate { get; set; }

    public bool? AllowRead { get; set; }

    public bool? AllowUpdate { get; set; }

    public bool? AllowDelete { get; set; }

    public bool? AllowPrint { get; set; }
}
