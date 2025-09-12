using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[PrimaryKey("TransDate", "CatalogId")]
[Table("HPP")]
public partial class Hpp
{
    [Key]
    public DateOnly TransDate { get; set; }

    [Column("TotalHPP", TypeName = "decimal(18, 2)")]
    public decimal? TotalHpp { get; set; }

    [Key]
    public long CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrevStock { get; set; }

    [Column("PrevHPP", TypeName = "decimal(18, 2)")]
    public decimal? PrevHpp { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPrice { get; set; }
}
