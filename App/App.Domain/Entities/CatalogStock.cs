using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Keyless]
[Table("CatalogStock")]
public partial class CatalogStock
{
    [Column("CatalogID")]
    public int? CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Stock { get; set; }

    public DateOnly? StockDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    public int IsActive { get; set; }

    [Column("colly", TypeName = "decimal(18, 2)")]
    public decimal? Colly { get; set; }
}
