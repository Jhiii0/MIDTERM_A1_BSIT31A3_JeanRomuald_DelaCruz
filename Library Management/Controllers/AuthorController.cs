using Library_Management.Models;
using Library_Management.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            var authors = AuthorService.Instance.GetAuthors();
            return View(authors);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddAuthorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AuthorService.Instance.AddAuthor(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(Guid id)
        {
            var author = AuthorService.Instance.GetAuthorDetails(id);
            if (author == null) return NotFound();

            return View(author);
        }

        public IActionResult EditModal(Guid id)
        {
            var editAuthorViewModel = AuthorService.Instance.GetAuthorById(id);
            if (editAuthorViewModel == null) return NotFound();

            return PartialView("_EditAuthorPartial", editAuthorViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditAuthorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                AuthorService.Instance.UpdateAuthor(vm);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public IActionResult DeleteModal(Guid id)
        {
            var author = AuthorService.Instance.GetAuthors().FirstOrDefault(a => a.AuthorId == id);
            if (author == null) return NotFound();

            return PartialView("_DeleteAuthorPartial", author);
        }

        [HttpDelete]
        [Route("Author/Delete/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                AuthorService.Instance.DeleteAuthor(id);
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
                AuthorService.Instance.ArchiveAuthor(id);
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
                AuthorService.Instance.RestoreAuthor(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        public IActionResult Archives()
        {
            var archivedAuthors = AuthorService.Instance.GetArchivedAuthors();
            ViewData["Title"] = "Archived Authors";
            return View("Index", archivedAuthors);
        }
    }
}
