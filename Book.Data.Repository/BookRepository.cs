using Book.Data.Context;
using Book.Data.Contract.RepositoryInterfaces;
using Book.Data.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Data.Repository
{
    public class BookRepository : Repository<Books>, IBookRepository
    {
        public BookRepository(MainDBContext context) : base(context)
        {

        }
    }

}
