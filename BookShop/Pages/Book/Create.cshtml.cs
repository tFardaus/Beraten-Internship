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

        public async Task<IActionResult> OnGetAsync()
        {
            Authors = new SelectList(await _authorRepository.GetAllAuthorsAsync(), "AuthorId", "Name");
            Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "Name");
            Publishers = new SelectList(await _publisherRepository.GetAllPublishersAsync(), "PublisherId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                Authors = new SelectList(await _authorRepository.GetAllAuthorsAsync(), "AuthorId", "Name");
                Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "Name");
                Publishers = new SelectList(await _publisherRepository.GetAllPublishersAsync(), "PublisherId", "Name");
                return Page();
            }

            await _bookRepository.AddBookAsync(Book);
            TempData["SuccessMessage"] = "Book created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
