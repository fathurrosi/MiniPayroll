using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Entities;

[Table("Log")]
public partial class Log
{
    [Key]
    [Column(TypeName = "datetime")]
    public DateTime LogDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ComputerName { get; set; }

    [Column("IPAddress")]
    [StringLength(15)]
    [Unicode(false)]
    public string? Ipaddress { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LogType { get; set; }

    [Unicode(false)]
    public string? LogMessage { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Username { get; set; }
}
