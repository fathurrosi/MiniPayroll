using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("CatalogPrice")]
public partial class CatalogPrice
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("CatalogID")]
    public int? CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? BuyPricePerunit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SellPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PriceDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SupplierCode { get; set; }
}
