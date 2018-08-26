using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Framework.Models;

namespace WebApplication3.Unit
{
    public class ObjectEquality
    {
 
    }
    public class UserRoleListEquality : IEqualityComparer<UserRole>
    {
        public bool Equals(UserRole x, UserRole y)
        {
            bool flag = false;
            if (x.UserId == y.UserId && x.RoleId == y.RoleId)
            {
                flag = true;
            }
            return flag;
        }


        public int GetHashCode(UserRole obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.ToString().GetHashCode();
        }
    }
}
