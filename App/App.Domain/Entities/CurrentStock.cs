using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Keyless]
[Table("CurrentStock")]
public partial class CurrentStock
{
    [Column("CatalogID")]
    public int? CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Stock { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Coli { get; set; }

    public DateOnly? StockDate { get; set; }
}
