using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_lab2.Abstractions;
using web_lab2.Helpers;
using web_lab2.Models.ViewModels;

namespace web_lab2.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrderBooksController : Controller
    {
        private readonly IUnitOfWork _uow;
        private ISession Session => HttpContext.Session;

        private Dictionary<int, int> Cart
        {
            get => Session.Get<Dictionary<int, int>>("cart") ?? new Dictionary<int, int>();
            set => Session.Set("cart", value);
        }

        public OrderBooksController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            var cart = Cart;
            var allAsync = (await _uow.Books.GetAllAsync()).Select(b => new BookCartItem
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Quantity = cart.ContainsKey(b.Id) ? cart[b.Id] : 0,
                Sages = b.Sages
            });
            return View(allAsync);
        }

        public IActionResult AddToCart(int id)
        {
            var cart = Cart;
            var quantity = cart.ContainsKey(id) ? cart[id] : 0;
            cart[id] = ++quantity;
            Cart = cart;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ReduceInCart(int id)
        {
            var cart = Cart;
            if (!cart.ContainsKey(id))
            {
                return RedirectToAction(nameof(Index));
            }

            var quantity = cart[id] - 1;
            cart[id] = quantity;
            if (quantity <= 0)
            {
                cart.Remove(id);
            }
            Cart = cart;
            return RedirectToAction(nameof(Index));
        }
    }
}