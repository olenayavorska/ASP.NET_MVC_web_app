using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_lab2.Abstractions;
using web_lab2.Models;

namespace web_lab2.Repositories
{
    public class BookRepository : BaseRepository<Book, int>, IBookRepository
    {
        public BookRepository(DatabaseContext context) : base(context)
        {
        }

        protected override IQueryable<Book> ComplexEntities =>
            Context.Books
                .Include(b => b.Sages)
                .Include(b => b.Orders)
                .Include(b => b.OrdersDetails)
                .AsTracking();
    }
}