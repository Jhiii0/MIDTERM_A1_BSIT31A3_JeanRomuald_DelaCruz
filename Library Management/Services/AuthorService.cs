using Library_Management.Models;
using Library_Management_Domain.Entities;

namespace Library_Management.Services
{
    public class AuthorService
    {
        private readonly ICollection<Author> _authors;

        private AuthorService()
        {
            _authors = BookService.Instance.GetAuthors();
        }

        public void AddAuthor(AddAuthorViewModel authorViewModel)
        {
            ArgumentNullException.ThrowIfNull(authorViewModel, nameof(authorViewModel));

            var newAuthor = new Author
            {
                Id = Guid.NewGuid(),
                Name = authorViewModel.Name,
                Biography = authorViewModel.Biography,
                BirthDate = authorViewModel.BirthDate,
                ProfileImageUrl = authorViewModel.ProfileImageUrl,
                IsArchived = false,
                Books = new List<Book>()
            };

            _authors.Add(newAuthor);
        }

        public IEnumerable<AuthorListViewModel> GetAuthors(bool includeArchived = false)
        {
            var query = _authors.AsEnumerable();
            
            if (!includeArchived)
            {
                query = query.Where(a => !a.IsArchived);
            }

            return query.Select(a => new AuthorListViewModel
            {
                AuthorId = a.Id,
                Name = a.Name,
                Biography = a.Biography,
                BirthDate = a.BirthDate,
                ProfileImageUrl = a.ProfileImageUrl,
                IsArchived = a.IsArchived,
                TotalBooks = a.Books.Count,
                ActiveBooks = a.Books.Count(b => !b.IsArchived)
            });
        }

        public IEnumerable<AuthorListViewModel> GetArchivedAuthors()
        {
            return GetAuthors(true).Where(a => a.IsArchived);
        }

        public AuthorDetailsViewModel? GetAuthorDetails(Guid id)
        {
            var author = _authors.FirstOrDefault(a => a.Id == id);
            if (author == null) return null;

            var bookService = BookService.Instance;
            var authorBooks = bookService.GetBooks().Where(b => b.AuthorName == author.Name && !author.IsArchived).ToList();

            return new AuthorDetailsViewModel
            {
                AuthorId = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate,
                ProfileImageUrl = author.ProfileImageUrl,
                IsArchived = author.IsArchived,
                Books = authorBooks.ToList()
            };
        }

        public EditAuthorViewModel? GetAuthorById(Guid id)
        {
            var author = _authors.FirstOrDefault(a => a.Id == id);
            if (author == null) return null;

            return new EditAuthorViewModel
            {
                AuthorId = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate,
                ProfileImageUrl = author.ProfileImageUrl
            };
        }

        public void UpdateAuthor(EditAuthorViewModel authorViewModel)
        {
            ArgumentNullException.ThrowIfNull(authorViewModel, nameof(authorViewModel));

            var author = _authors.FirstOrDefault(a => a.Id == authorViewModel.AuthorId);
            if (author == null) throw new KeyNotFoundException("Author not found");

            author.Name = authorViewModel.Name;
            author.Biography = authorViewModel.Biography;
            author.BirthDate = authorViewModel.BirthDate;
            author.ProfileImageUrl = authorViewModel.ProfileImageUrl;
        }

        public void DeleteAuthor(Guid id)
        {
            var author = _authors.FirstOrDefault(a => a.Id == id);
            if (author == null) throw new KeyNotFoundException("Author not found");

            // Also archive all books by this author
            foreach (var book in author.Books)
            {
                book.IsArchived = true;
            }

            _authors.Remove(author);
        }

        public void ArchiveAuthor(Guid id)
        {
            var author = _authors.FirstOrDefault(a => a.Id == id);
            if (author == null) throw new KeyNotFoundException("Author not found");

            author.IsArchived = true;
            
            // Also archive all books by this author
            foreach (var book in author.Books)
            {
                book.IsArchived = true;
            }
        }

        public void RestoreAuthor(Guid id)
        {
            var author = _authors.FirstOrDefault(a => a.Id == id);
            if (author == null) throw new KeyNotFoundException("Author not found");

            author.IsArchived = false;
            
            // Optionally restore all books by this author (commented out as it might not be desired behavior)
            // foreach (var book in author.Books)
            // {
            //     book.IsArchived = false;
            // }
        }

        // Singleton pattern
        private static AuthorService? _instance;
        public static AuthorService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AuthorService();
                }
                return _instance;
            }
        }
    }
}
