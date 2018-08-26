using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApplication3.Framework.Models
{
    [Table("Users")]
  public class User
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(20)]
        [Column(TypeName="varchar(20)")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birth { get; set; }
        public int Gender { get; set; }
        [MaxLength(30)]
        [Column(TypeName="varchar(20)")]
        public string Password { get; set; }
        public List<UserRole> UserRoles { get; set; }

    }

    [Table("Role")]
    public class Role
    {
        //树状结构是我抄的所以表设计的就有点怪了
        [Key]
        public int RoleId { get; set; }
        [MaxLength(20)]

        [Column(TypeName ="nvarchar(20)")]
        public string RoleName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Url { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string MenuId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Pid { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }

    [Table("UserRole")]
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual  User User { get; set; }
        public virtual  Role Role { get; set; }
    }

    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int id { get; set; }
        public string menId { get; set; }
        public string menuName { get; set; }
        public string pid { get; set; }
    }
}
