using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_lab2.Abstractions;
using web_lab2.Models;

namespace web_lab2.Repositories
{
    public class SageRepository : BaseRepository<Sage, int>, ISageRepository
    {
        public SageRepository(DatabaseContext context) : base(context)
        {
        }

        protected override IQueryable<Sage> ComplexEntities =>
            Context.Sages
                .Include(s => s.Books)
                .AsTracking();
    }
}