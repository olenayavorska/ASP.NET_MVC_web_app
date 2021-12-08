using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_lab2.Abstractions;

namespace web_lab2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _uow;

        public OrdersController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Orders.GetAllAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _uow.Orders.GetByIdAsync((int) id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
