using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("EMPLOYMENT_PAY")]
public partial class EmploymentPay
{
    [Key]
    [Column("EMPLOYMENT_PAY_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string EmploymentPayId { get; set; } = null!;

    [Column("EMPLOYMENT_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string EmploymentId { get; set; } = null!;

    [Column("START_DATE", TypeName = "DATE")]
    public DateTime? StartDate { get; set; }

    [Column("END_DATE", TypeName = "DATE")]
    public DateTime? EndDate { get; set; }

    [Column("SALARY", TypeName = "NUMBER(8,2)")]
    public decimal? Salary { get; set; }

    [Column("COMMISSION_PCT", TypeName = "NUMBER(2,2)")]
    public decimal? CommissionPct { get; set; }

    [ForeignKey("EmploymentId")]
    [InverseProperty("EmploymentPays")]
    public virtual Employment Employment { get; set; } = null!;
}
