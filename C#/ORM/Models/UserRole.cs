using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ORM.Models;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
[Table("USER_ROLE")]
public class UserRole
{
    [Column("user_id", TypeName = "binary(16)")]
    public byte[] UserId { get; set; } = new byte[16];

    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [ForeignKey("RoleId")]
    public Role Role { get; set; } = null!;
}