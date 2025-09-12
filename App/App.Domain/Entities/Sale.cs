using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("Sale")]
public partial class Sale
{
    [Key]
    [Column("TransactionID")]
    [StringLength(50)]
    [Unicode(false)]
    public string TransactionId { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPrice { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? TotalQty { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TransactionDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Username { get; set; }

    [Column("MemberID")]
    public int? MemberId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Terminal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPayment { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPaymentReturn { get; set; }

    [Unicode(false)]
    public string? Notes { get; set; }

    public int? PaymentType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiredDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Created { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    public long Counter { get; set; }
}
