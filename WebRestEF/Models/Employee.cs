using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebRest.EF.Models;

[Table("EMPLOYEE")]
public partial class Employee
{
    [Key]
    [Column("EMPLOYEE_ID")]
    [StringLength(38)]
    [Unicode(false)]
    public string EmployeeId { get; set; } = null!;

    [Column("FIRST_NAME")]
    [StringLength(20)]
    [Unicode(false)]
    public string? FirstName { get; set; }

    [Column("LAST_NAME")]
    [StringLength(25)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(25)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("PHONE_NUMBER")]
    [StringLength(20)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [InverseProperty("Manager")]
    public virtual ICollection<DeptMgr> DeptMgrs { get; set; } = new List<DeptMgr>();

    [InverseProperty("Employee")]
    public virtual ICollection<Employment> Employments { get; set; } = new List<Employment>();
}
