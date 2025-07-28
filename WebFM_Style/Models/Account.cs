using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFM_Style.Models;

public partial class Account
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Avartar { get; set; }

    public string? Email { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public int? Point { get; set; }

    public DateTime? Birthday { get; set; }

    public byte? Gender { get; set; }

    public int? RoleId { get; set; }

    public int? AccountTypeId { get; set; }

    public byte? Status { get; set; }
    [NotMapped]
    public string ResetToken { get; set; }

    [NotMapped]
    public DateTime? ResetTokenExpiry { get; set; }

    public virtual AccountType? AccountType { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Order> Oders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }
}
