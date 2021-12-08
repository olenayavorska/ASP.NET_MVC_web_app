using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_lab2.Abstractions;
using web_lab2.Models;
using web_lab2.Models.ViewModels;

namespace web_lab2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SagesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public SagesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Sages
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Sages.GetAllAsync());
        }

        // GET: Sages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sage = await _uow.Sages.GetByIdAsync((int) id);
            if (sage == null)
            {
                return NotFound();
            }

            return View(sage);
        }

        // GET: Sages/Create
        public async Task<IActionResult> Create()
        {
            await PopulateAssignedBooks(new Sage());
            return View();
        }

        // POST: Sages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Age,Photo,City")] SageViewModel svm,
            string[] selectedBooks)
        {
            if (ModelState.IsValid)
            {
                Sage sage = new Sage
                {
                    Name = svm.Name,
                    Age = svm.Age,
                    City = svm.City
                };
                CopyImageToSage(sage, svm.Photo);
                await UpdateSageBooks(selectedBooks, sage);

                await _uow.Sages.InsertAsync(sage);
                await _uow.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(svm);
        }

        // GET: Sages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sage = await _uow.Sages.GetByIdAsync((int) id);
            if (sage == null)
            {
                return NotFound();
            }

            var svm = new SageViewModel
            {
                Id = sage.Id,
                Age = sage.Age,
                City = sage.City,
                Name = sage.Name
            };
            await PopulateAssignedBooks(sage);
            return View(svm);
        }

        // POST: Sages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Photo,City")] SageViewModel svm,
            string[] selectedBooks)
        {
            if (id != svm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!await _uow.Sages.ExistsAsync(svm.Id))
                {
                    return NotFound();
                }

                Sage sage = await _uow.Sages.GetByIdAsync(svm.Id);
                sage.Name = svm.Name;
                sage.Age = svm.Age;
                sage.City = svm.City;
                CopyImageToSage(sage, svm.Photo);

                await UpdateSageBooks(selectedBooks, sage);
                await _uow.Sages.UpdateAsync(sage);
                await _uow.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(svm);
        }

        // GET: Sages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sage = await _uow.Sages.GetByIdAsync((int) id);
            if (sage == null)
            {
                return NotFound();
            }

            return View(sage);
        }

        // POST: Sages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _uow.Sages.DeleteAsync(id);
            await _uow.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        #region Helpers

        private void CopyImageToSage(Sage sage, IFormFile file)
        {
            if (file != null)
            {
                byte[] imageData;
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int) file.Length);
                }

                sage.Photo = imageData;
            }
        }

        private async Task PopulateAssignedBooks(Sage sage)
        {
            var allBooks = await _uow.Books.GetAllAsync();
            var sageBooks = new HashSet<int>(sage.Books.Select(b => b.Id));
            var viewModel = allBooks.Select(b => new AssignedBookViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Selected = sageBooks.Contains(b.Id)
            }).ToList();
            ViewData["Books"] = viewModel;
        }

        private async Task UpdateSageBooks(string[] selectedBooks, Sage sage)
        {
            var selectedBooksHs = new HashSet<int>(selectedBooks.Select(int.Parse));
            var sageBooks = sage.Books.Select(b => b.Id).ToHashSet();
            var books = await _uow.Books.GetAllAsync();
            foreach (var book in books)
            {
                if (selectedBooksHs.Contains(book.Id))
                {
                    if (!sageBooks.Contains(book.Id))
                    {
                        sage.Books.Add(book);
                    }
                }
                else if (sageBooks.Contains(book.Id))
                {
                    sage.Books.Remove(book);
                }
            }
        }

        #endregion
    }
}