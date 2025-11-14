using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Pages.Book
{
    public class EditModel : PageModel
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublisherRepository _publisherRepository;

        public EditModel(IBookRepository bookRepository, IAuthorRepository authorRepository,
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var book = await _bookRepository.GetBookByIdAsync(id.Value);
            if (book == null)
            {
                return RedirectToPage("./Index");
            }

            Book = book;
            Authors = new SelectList(await _authorRepository.GetAllAuthorsAsync(), "AuthorId", "Name");
            Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "Name");
            Publishers = new SelectList(await _publisherRepository.GetAllPublishersAsync(), "PublisherId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Book did not update.";
                Authors = new SelectList(await _authorRepository.GetAllAuthorsAsync(), "AuthorId", "Name");
                Categories = new SelectList(await _categoryRepository.GetAllCategoriesAsync(), "CategoryId", "Name");
                Publishers = new SelectList(await _publisherRepository.GetAllPublishersAsync(), "PublisherId", "Name");
                return Page();
            }

            await _bookRepository.UpdateBookAsync(Book);
            TempData["SuccessMessage"] = "Book updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
