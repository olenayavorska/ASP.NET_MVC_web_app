using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_lab2.Abstractions;
using web_lab2.Models;
using web_lab2.Models.ViewModels;

namespace web_lab2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _uow;

        public BooksController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Books.GetAllAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _uow.Books.GetByIdAsync((int) id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            await PopulateAssignedSages(new Book());
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Book book,
            string[] selectedSages)
        {
            if (ModelState.IsValid)
            {
                await UpdateBookSages(selectedSages, book);
                await _uow.Books.InsertAsync(book);
                await _uow.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _uow.Books.GetByIdAsync((int) id);
            if (book == null)
            {
                return NotFound();
            }

            await PopulateAssignedSages(book);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name,Description")] Book book,
            string[] selectedSages)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!await _uow.Books.ExistsAsync(book.Id))
                {
                    return NotFound();
                }

                Book existing = await _uow.Books.GetByIdAsync(book.Id);
                existing.Name = book.Name;
                existing.Description = book.Description;
                await UpdateBookSages(selectedSages, existing);
                await _uow.Books.UpdateAsync(existing);

                await _uow.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _uow.Books.GetByIdAsync((int) id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _uow.Books.DeleteAsync(id);
            await _uow.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateAssignedSages(Book book)
        {
            var allSages = await _uow.Sages.GetAllAsync();
            var booksSages = new HashSet<int>(book.Sages.Select(s => s.Id));
            var viewModel = allSages.Select(s => new AssignedSageViewModel()
            {
                Id = s.Id,
                Age = s.Age,
                Name = s.Name,
                City = s.City,
                Selected = booksSages.Contains(s.Id)
            }).ToList();
            ViewData["Sages"] = viewModel;
        }

        private async Task UpdateBookSages(string[] selectedSages, Book book)
        {
            var selectedSagesHs = new HashSet<int>(selectedSages.Select(int.Parse));
            var booksSages = book.Sages.Select(s => s.Id).ToHashSet();
            var sages = await _uow.Sages.GetAllAsync();
            foreach (var sage in sages)
            {
                if (selectedSagesHs.Contains(sage.Id))
                {
                    if (!booksSages.Contains(sage.Id))
                    {
                        book.Sages.Add(sage);
                    }
                }
                else if (booksSages.Contains(sage.Id))
                {
                    book.Sages.Remove(sage);
                }
            }
        }
    }
}