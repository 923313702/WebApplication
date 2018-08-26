using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WebApplication3.Framework.IRepositorys
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        #region 编辑
        /// <summary>
        /// 通过传入的model加需要修改的字段来更改数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertys"></param>
        void Edit(TEntity model, string[] propertys);

        /// <summary>
        /// 直接查询之后再修改
        /// </summary>
        /// <param name="model"></param>
        void Edit(TEntity model);
        #endregion

        #region 删除
        void Delete(TEntity model, bool isadded);


        #endregion

        #region 新增
        void Add(TEntity model);
        void Add(IEnumerable<TEntity> models);
        #endregion

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> QueryWhere(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Query();
        /// <summary>
        /// 动态多条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> AsExpandable<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> keySelector, bool IsQueryOrderBy);

        #region 统一提交
        int SaverChanges();
        #endregion
    }
}
