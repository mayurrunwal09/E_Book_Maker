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
    public class AuthorRepository : Repository<Author> , IAuthorRepository
    {
        public AuthorRepository(MainDBContext _context) : base(_context)
        { 

        }
    }

}
