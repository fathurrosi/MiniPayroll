using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("PurchaseDetail")]
public partial class PurchaseDetail
{
    [StringLength(100)]
    [Unicode(false)]
    public string PurchaseNo { get; set; } = null!;

    [Column("CatalogID")]
    public int CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PricePerUnit { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Unit { get; set; }

    [Column("coli", TypeName = "decimal(18, 2)")]
    public decimal? Coli { get; set; }
}
