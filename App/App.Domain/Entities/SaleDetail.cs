using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("SaleDetail")]
public partial class SaleDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("TransactionID")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TransactionId { get; set; }

    [Column("CatalogID")]
    public int? CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPrice { get; set; }

    public int? Sequence { get; set; }

    [Column("coli", TypeName = "decimal(18, 2)")]
    public decimal? Coli { get; set; }
}
