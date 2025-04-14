using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("DEPT_MGR")]
[Index("ManagerId", "DepartmentId", "EffDate", Name = "DEPT_MGR_UK1", IsUnique = true)]
public partial class DeptMgr
{
    [Key]
    [Column("DEPT_MGR_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string DeptMgrId { get; set; } = null!;

    [Column("MANAGER_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string ManagerId { get; set; } = null!;

    [Column("DEPARTMENT_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string DepartmentId { get; set; } = null!;

    [Column("EFF_DATE", TypeName = "DATE")]
    public DateTime EffDate { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("DeptMgrs")]
    public virtual Department Department { get; set; } = null!;

    [ForeignKey("ManagerId")]
    [InverseProperty("DeptMgrs")]
    public virtual Employee Manager { get; set; } = null!;
}
