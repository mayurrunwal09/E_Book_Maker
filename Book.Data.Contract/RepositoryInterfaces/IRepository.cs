using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Contract.RepositoryInterfaces
{
      public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> All();
        T GetById(long id);
        Task<int> SaveChangesAysnc();
        int Delete(T T);
        Task<T> FindOneAysnc(Expression<Func<T, bool>> predicate);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        T Add(T t);
        int Update(T t);
    }
}
