using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext dbContext;
        protected DbSet<TEntity> dbSet;

        public Repository(TravelnessContext context)
        {
            this.dbContext = context;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public void SetDbContext(DbContext context)
        {
            this.dbContext = context;
        }

        public virtual int Count()
        {
            return dbSet.Count();
        }
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Count(predicate);
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            var count = query.Count();

            foreach (string includeProperty in includeProperties.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null) query = orderBy(query);

            return query;
        }
        public virtual TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbContext.Attach(entityToUpdate);
            dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            TEntity entityToDelete = dbSet.Find(id);

            if (entityToDelete == null) throw new InvalidOperationException("There is no records with such id");
            Delete(entityToDelete);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null) throw new ArgumentNullException(nameof(entityToDelete));

            if (dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null) dbSet.RemoveRange(dbSet.Where(predicate));
            else dbSet.RemoveRange(dbSet);
        }
    }
}
