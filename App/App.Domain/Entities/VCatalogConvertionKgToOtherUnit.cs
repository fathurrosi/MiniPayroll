using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Keyless]
public partial class VCatalogConvertionKgToOtherUnit
{
    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column("CatalogQty_Kg", TypeName = "decimal(18, 2)")]
    public decimal? CatalogQtyKg { get; set; }

    [Column("Product_KgPerUnit", TypeName = "decimal(18, 2)")]
    public decimal? ProductKgPerUnit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ProductQty { get; set; }

    [Column("Convertion_Type")]
    [StringLength(25)]
    [Unicode(false)]
    public string ConvertionType { get; set; } = null!;

    [Column("convertion_date")]
    public DateOnly? ConvertionDate { get; set; }
}
