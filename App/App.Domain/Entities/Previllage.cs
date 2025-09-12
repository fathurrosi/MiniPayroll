using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[PrimaryKey("MenuId", "RoleId")]
[Table("Previllage")]
public partial class Previllage
{
    [Key]
    [Column("MenuID")]
    public int MenuId { get; set; }

    [Key]
    [Column("RoleID")]
    public int RoleId { get; set; }

    public bool? AllowCreate { get; set; }

    public bool? AllowRead { get; set; }

    public bool? AllowUpdate { get; set; }

    public bool? AllowDelete { get; set; }

    public bool? AllowPrint { get; set; }
}
