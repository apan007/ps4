using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("DEPARTMENT")]
public partial class Department
{
    [Key]
    [Column("DEPARTMENT_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string DepartmentId { get; set; } = null!;

    [Column("DEPARTMENT_NAME")]
    [StringLength(30)]
    [Unicode(false)]
    public string DepartmentName { get; set; } = null!;

    [Column("OFFICE_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string OfficeId { get; set; } = null!;

    [InverseProperty("Department")]
    public virtual ICollection<DeptMgr> DeptMgrs { get; set; } = new List<DeptMgr>();

    [ForeignKey("OfficeId")]
    [InverseProperty("Departments")]
    public virtual Office Office { get; set; } = null!;
}
