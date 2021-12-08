using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_lab2.Abstractions;
using web_lab2.Models;

namespace web_lab2.Repositories
{
    public class UserRepository : BaseRepository<User, int>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        protected override IQueryable<User> ComplexEntities =>
            Context.Users
                .Include(u => u.Roles)
                .Include(u => u.Orders)
                .AsTracking();
    }
}