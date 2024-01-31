using Book.Data.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Contract.RepositoryInterfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
    }
}
