using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("REGION")]
public partial class Region
{
    [Key]
    [Column("REGION_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string RegionId { get; set; } = null!;

    [Column("REGION_NAME")]
    [StringLength(25)]
    [Unicode(false)]
    public string? RegionName { get; set; }

    [InverseProperty("Region")]
    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
