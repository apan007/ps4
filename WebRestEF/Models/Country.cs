using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("COUNTRY")]
public partial class Country
{
    [Key]
    [Column("COUNTRY_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string CountryId { get; set; } = null!;

    [Column("COUNTRY_CODE")]
    [StringLength(5)]
    [Unicode(false)]
    public string? CountryCode { get; set; }

    [Column("COUNTRY_NAME")]
    [StringLength(40)]
    [Unicode(false)]
    public string? CountryName { get; set; }

    [Column("REGION_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string RegionId { get; set; } = null!;

    [InverseProperty("Country")]
    public virtual ICollection<Office> Offices { get; set; } = new List<Office>();

    [ForeignKey("RegionId")]
    [InverseProperty("Countries")]
    public virtual Region Region { get; set; } = null!;
}
