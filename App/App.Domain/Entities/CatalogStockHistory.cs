using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Keyless]
[Table("CatalogStockHistory")]
public partial class CatalogStockHistory
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("CatalogID")]
    public int CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Stock { get; set; }

    public DateOnly? StockDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Created { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [Column("colly", TypeName = "decimal(18, 2)")]
    public decimal? Colly { get; set; }
}
