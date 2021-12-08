using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_lab2.Abstractions;
using web_lab2.Models;

namespace web_lab2.Repositories
{
    public class RoleRepository : BaseRepository<Role, int>, IRoleRepository
    {
        public RoleRepository(DatabaseContext context) : base(context)
        {
        }

        protected override IQueryable<Role> ComplexEntities =>
            Context.Roles
                .Include(r => r.Users)
                .AsTracking();
    }
}