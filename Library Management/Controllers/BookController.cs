using Library_Management.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Library_Management.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            var books = BookService.Instance.GetBooks();
            return View(books);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            BookService.Instance.AddBook(model);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult EditModal(Guid id)
        {
            var editBookViewModel = BookService.Instance.GetBookById(id);
            if (editBookViewModel == null) return NotFound();


            return PartialView("_EditBookPartial", editBookViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditBookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // If model state is not valid, you can return a view with validation errors
                return BadRequest(ModelState);
            }

            // Assuming BookService has a method to update the book
            BookService.Instance.UpdateBook(vm);

            return Ok();
        }

        public IActionResult DeleteModal(Guid id)
        {
            var book = BookService.Instance.GetBooks().FirstOrDefault(b => b.BookId == id);
            if (book == null) return NotFound();

            return PartialView("_DeleteBookPartial", book);
        }

        [HttpDelete]
        [Route("Book/Delete/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            // Assuming BookService has a method to delete the book
            BookService.Instance.DeleteBook(id);
            return Ok();
        }

        public IActionResult Details(Guid id)
        {
            var book = BookService.Instance.GetBooks().First(b => b.BookId == id);
            var bookCopies = BookService.Instance.GetBookCopies(id);
            ViewBag.BookCopies = bookCopies;
            return View(book);
        }

        [HttpPost]
        public IActionResult PulloutCopy(Guid bookCopyId, string reason)
        {
            try
            {
                BookService.Instance.PulloutBookCopy(bookCopyId, reason);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Archive(Guid id)
        {
            try
            {
                BookService.Instance.ArchiveBook(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Restore(Guid id)
        {
            try
            {
                BookService.Instance.RestoreBook(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public IActionResult Archives()
        {
            var archivedBooks = BookService.Instance.GetArchivedBooks();
            ViewData["Title"] = "Archived Books";
            return View("Index", archivedBooks);
        }

    }
}
