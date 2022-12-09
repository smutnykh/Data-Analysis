using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int Count();
        int Count(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetById(int id);

        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);

        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Delete(Expression<Func<TEntity, bool>> predicate);
    }
}
