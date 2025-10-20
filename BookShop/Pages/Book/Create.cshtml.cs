using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Pages.Book
{
    public class CreateModel : PageModel
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublisherRepository _publisherRepository;

        public CreateModel(IBookRepository bookRepository, IAuthorRepository authorRepository, 
            ICategoryRepository categoryRepository, IPublisherRepository publisherRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
        }

        [BindProperty]
        public Models.Book Book { get; set; } = new Models.Book();

        public SelectList Authors { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Publishers { get; set; }

        public IActionResult OnGet()
        {
            Authors = new SelectList(_authorRepository.GetAllAuthors(), "AuthorId", "Name");
            Categories = new SelectList(_categoryRepository.GetAllCategories(), "CategoryId", "Name");
            Publishers = new SelectList(_publisherRepository.GetAllPublishers(), "PublisherId", "Name");
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Authors = new SelectList(_authorRepository.GetAllAuthors(), "AuthorId", "Name");
                Categories = new SelectList(_categoryRepository.GetAllCategories(), "CategoryId", "Name");
                Publishers = new SelectList(_publisherRepository.GetAllPublishers(), "PublisherId", "Name");
                return Page();
            }

            _bookRepository.AddBook(Book);
            return RedirectToPage("./Index");
        }
    }
}
