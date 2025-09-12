using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("ReconcileDetail")]
public partial class ReconcileDetail
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("CatalogID")]
    public int? CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CatalogQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CatalogPrice { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ProductPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ProductQty { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("ReconcileID")]
    public long? ReconcileId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CatalogPriceDate { get; set; }
}
