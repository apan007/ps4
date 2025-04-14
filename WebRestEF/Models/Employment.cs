using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("EMPLOYMENT")]
public partial class Employment
{
    [Key]
    [Column("EMPLOYMENT_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string EmploymentId { get; set; } = null!;

    [Column("EMPLOYEE_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string EmployeeId { get; set; } = null!;

    [Column("JOB_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string JobId { get; set; } = null!;

    [Column("START_DATE", TypeName = "DATE")]
    public DateTime StartDate { get; set; }

    [Column("END_DATE", TypeName = "DATE")]
    public DateTime EndDate { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Employments")]
    public virtual Employee Employee { get; set; } = null!;

    [InverseProperty("Employment")]
    public virtual ICollection<EmploymentPay> EmploymentPays { get; set; } = new List<EmploymentPay>();

    [ForeignKey("JobId")]
    [InverseProperty("Employments")]
    public virtual Job Job { get; set; } = null!;
}
