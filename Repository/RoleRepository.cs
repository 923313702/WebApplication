

using System;
using System.Collections.Generic;
using System.Text;
using WebApplication3.Framework.IRepositorys;
using WebApplication3.Framework.Models;

namespace WebApplication3.Framework.Repositorys
{
    public class RoleRepository:BaseRepository<Role>,IRoleRepository
    {
       // protected ApplicationDb db;
        public RoleRepository(ApplicationDb _db) : base(_db)
        {
            //this.db = _db;
        }
    }
}
