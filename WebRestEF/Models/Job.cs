using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("JOB")]
public partial class Job
{
    [Key]
    [Column("JOB_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string JobId { get; set; } = null!;

    [Column("JOB_CODE")]
    [StringLength(10)]
    [Unicode(false)]
    public string JobCode { get; set; } = null!;

    [Column("JOB_TITLE")]
    [StringLength(35)]
    [Unicode(false)]
    public string JobTitle { get; set; } = null!;

    [Column("MIN_SALARY")]
    [Precision(6)]
    public int MinSalary { get; set; }

    [Column("MAX_SALARY")]
    [Precision(6)]
    public int MaxSalary { get; set; }

    [InverseProperty("Job")]
    public virtual ICollection<Employment> Employments { get; set; } = new List<Employment>();
}
