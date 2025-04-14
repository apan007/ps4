using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("OFFICE")]
public partial class Office
{
    [Key]
    [Column("OFFICE_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string OfficeId { get; set; } = null!;

    [Column("OFFICE_CD", TypeName = "NUMBER")]
    public decimal? OfficeCd { get; set; }

    [Column("STREET_ADDRESS")]
    [StringLength(40)]
    [Unicode(false)]
    public string? StreetAddress { get; set; }

    [Column("POSTAL_CODE")]
    [StringLength(12)]
    [Unicode(false)]
    public string? PostalCode { get; set; }

    [Column("CITY")]
    [StringLength(30)]
    [Unicode(false)]
    public string? City { get; set; }

    [Column("STATE_PROVINCE")]
    [StringLength(25)]
    [Unicode(false)]
    public string? StateProvince { get; set; }

    [Column("COUNTRY_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string CountryId { get; set; } = null!;

    [ForeignKey("CountryId")]
    [InverseProperty("Offices")]
    public virtual Country Country { get; set; } = null!;

    [InverseProperty("Office")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
