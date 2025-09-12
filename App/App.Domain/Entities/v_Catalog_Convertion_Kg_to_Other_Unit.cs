using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Keyless]
public partial class v_Catalog_Convertion_Kg_to_Other_Unit
{
    public int? ProductID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CatalogQty_Kg { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Product_KgPerUnit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ProductQty { get; set; }

    [Required]
    [StringLength(25)]
    [Unicode(false)]
    public string Convertion_Type { get; set; }

    public DateOnly? convertion_date { get; set; }
}
