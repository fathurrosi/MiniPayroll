using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[PrimaryKey("TransDate", "CatalogId")]
[Table("DailyGrossProfit")]
public partial class DailyGrossProfit
{
    [Key]
    [Column(TypeName = "datetime")]
    public DateTime TransDate { get; set; }

    [Key]
    [Column("CatalogID")]
    public int CatalogId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Purchase { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Sale { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? GrossProfit { get; set; }
}
