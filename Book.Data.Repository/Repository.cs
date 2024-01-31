using Book.Data.Context;
using Book.Data.Contract.RepositoryInterfaces;
using Book.Data.DBModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected MainDBContext _context = null!;
        public Repository(MainDBContext context)
        {
            this._context = context;
        }
        protected DbSet<T> DbSet
        {
            get
            {
                return _context.Set<T>();
            }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public virtual T GetById(long id)
        {
            var record = DbSet.Find(id);
            if (record == null)
            {
                return null!;
            }
            return record;
        }

        public virtual IQueryable<T> All()
        {
            return DbSet.AsQueryable();
        }

        public async Task<int> SaveChangesAysnc()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<T> FindOneAysnc(Expression<Func<T, bool>> predicate)
        {
            var record = await DbSet.FirstOrDefaultAsync(predicate);
            if (record == null)
            {
                return null!;
            }
            return record;
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public virtual T Add(T T)
        {
            var newEntry = DbSet.Add(T);
            return newEntry.Entity;
        }
        public virtual int Update(T T)
        {
            var entry = _context.Entry(T);
            DbSet.Attach(T);
            entry.State = EntityState.Modified;
            return 0;
        }

        public virtual int Delete(T T)
        {
            DbSet.Remove(T);
            //Commit();
            return 0;
        }
    }
}
