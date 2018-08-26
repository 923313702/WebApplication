
using LinqKit;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WebApplication3.Framework.IRepositorys;
using WebApplication3.Framework.Models;

namespace WebApplication3.Framework.Repositorys
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected ApplicationDb db;
        public BaseRepository(ApplicationDb _db)
        {
            this.db = _db;
        }
        public void Add(TEntity model)
        {
            db.Set<TEntity>().Add(model);
        }

        public void Add(IEnumerable<TEntity> models)
        {
            db.Set<TEntity>().AddRange(models);
        }

        public IQueryable<TEntity> AsExpandable<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool IsQueryOrderBy)
        {
            if (IsQueryOrderBy)
            {
                return db.Set<TEntity>().AsExpandable().Where(predicate).OrderBy(keySelector);
            }
            return db.Set<TEntity>().AsExpandable().Where(predicate).OrderByDescending(keySelector);
        }

        public void Delete(TEntity model, bool isadded)
        {
            if (isadded)
                db.Set<TEntity>().Attach(model);
            db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(TEntity model, string[] propertys)
        {
            if (propertys.Any())
            {
                foreach (var i in propertys)
                {
                    db.Entry(model).Property(i).IsModified = true;
                }
              
            }
        }

        public void Edit(TEntity model)
        {
            db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public IQueryable<TEntity> Query()
        {
            return db.Set<TEntity>();
        }

        public IQueryable<TEntity> QueryWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return db.Set<TEntity>().Where(predicate);
        }

        public int SaverChanges()
        {
           return  db.SaveChanges();
        }
    }
}
