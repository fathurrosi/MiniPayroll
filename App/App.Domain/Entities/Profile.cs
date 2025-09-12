using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("Profile")]
public partial class Profile
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Name { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Notes { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Subtitle { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Address { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Website { get; set; }

    [Column(TypeName = "image")]
    public byte[]? Logo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Created { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LogoExtension { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Updated { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }
}
