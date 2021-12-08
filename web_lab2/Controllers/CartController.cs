using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_lab2.Abstractions;
using web_lab2.Helpers;
using web_lab2.Models;
using web_lab2.Models.ViewModels;

namespace web_lab2.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _uow;

        private ISession Session => HttpContext.Session;

        private Dictionary<int, int> Cart
        {
            get => Session.Get<Dictionary<int, int>>("cart") ?? new Dictionary<int, int>();
            set => Session.Set("cart", value);
        }

        public CartController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var cart = Cart;
            var allAsync = (await _uow.Books.GetAllAsync())
                .Where(b => cart.ContainsKey(b.Id))
                .Select(b => new BookCartItem
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Quantity = cart[b.Id],
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

        public async Task<IActionResult> Order()
        {
            var cart = Cart;
            var username = User.Claims.First(c => c.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            var currentUser = await _uow.Users.GetFirstAsync(u => u.Username == username);

            var order = new Order
            {
                Customer = currentUser
            };

            var orderDetails = cart.Select(item => new OrdersBooks
            {
                BookId = item.Key,
                Number = item.Value,
                Order = order
            });
            order.OrdersDetails = orderDetails.ToList();

            await _uow.Orders.InsertAsync(order);
            await _uow.SaveAsync();

            Cart = null;

            return RedirectToAction("Index", "OrderBooks");
        }
    }
}