using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Keyless]
[Table("Menu_backup")]
public partial class MenuBackup
{
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Code { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Name { get; set; }

    [Column("ParentID")]
    public int? ParentId { get; set; }

    public int? Sequence { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Ico { get; set; }

    [Unicode(false)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Shortcut { get; set; }

    public int? Ctrl { get; set; }

    public int? Shift { get; set; }

    public int? Alt { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Route { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Action { get; set; }
}
