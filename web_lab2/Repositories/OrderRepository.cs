using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_lab2.Abstractions;
using web_lab2.Models;

namespace web_lab2.Repositories
{
    public class OrderRepository: BaseRepository<Order, int>, IOrderRepository
    {
        public OrderRepository(DatabaseContext context) : base(context)
        {

        }

        protected override IQueryable<Order> ComplexEntities =>
            Context.Orders
                .Include(o => o.Books)
                .Include(o => o.OrdersDetails)
                .Include(o => o.Customer)
                .AsTracking();
    }
}