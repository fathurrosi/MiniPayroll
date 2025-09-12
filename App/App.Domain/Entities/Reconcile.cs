using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("Reconcile")]
public partial class Reconcile
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProccessDate { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("purchaseno")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Purchaseno { get; set; }

    [Column("transactionid")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Transactionid { get; set; }
}
