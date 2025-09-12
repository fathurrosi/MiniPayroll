using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("Purchase")]
public partial class Purchase
{
    [StringLength(100)]
    [Unicode(false)]
    public string PurchaseNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime PurchaseDate { get; set; }

    [Unicode(false)]
    public string? Notes { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SupplierCode { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalPrice { get; set; }

    [Key]
    [Column("ID")]
    public int Id { get; set; }
}
