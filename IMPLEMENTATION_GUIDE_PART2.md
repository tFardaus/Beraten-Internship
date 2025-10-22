# BookShop Project - Implementation Guide (Part 2)

## PHASE 5: RAZOR PAGES IMPLEMENTATION

### Step 5.1: Create Index Page (List View)

**File: BookShop/Pages/Book/Index.cshtml.cs**

```csharp
using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IBookRepository _bookRepository;

        public IndexModel(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public List<Models.Book> Books { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;

        public void OnGet(string searchTerm)
        {
            SearchTerm = searchTerm ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Books = _bookRepository.GetAllBook().ToList();
            }
            else
            {
                Books = _bookRepository.SearchBooks(searchTerm).ToList();
            }
        }
    }
}
```

**Logic**:
- Constructor Injection: `IBookRepository` injected by DI container
- `OnGet()`: Handles GET requests
- Search logic: If term empty → show all, else → filter

---

**File: BookShop/Pages/Book/Index.cshtml**

```razor
@page
@model BookShop.Pages.Book.IndexModel
@{
    ViewData["Title"] = "Books";
}

<h1>Books List</h1>
<hr />

<div class="row">
    <div class="col-md-9">
        <div class="row mb-3">
            <div class="col-md-6">
                <form method="get">
                    <div class="input-group">
                        <input type="text" name="searchTerm" value="@Model.SearchTerm" 
                               class="form-control" placeholder="Search by title..." />
                        <button type="submit" class="btn btn-primary">Search</button>
                        <a asp-page="./Index" class="btn btn-secondary">Clear</a>
                    </div>
                </form>
            </div>
            <div class="col-md-6 text-end">
                <a asp-page="Create" class="btn btn-success">Create</a>
            </div>
        </div>
        
        <table class="table table-striped table-hover table-responsive">
            <thead>
                <tr class="text-center">
                    <th>Title</th>
                    <th>Author</th>
                    <th>Category</th>
                    <th>Publisher</th>
                    <th>Price</th>
                    <th>Stock</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var book in Model.Books)
                {
                    <tr class="text-center">
                        <td>@book.BookTitle</td>
                        <td>@book.Author?.Name</td>
                        <td>@book.Category?.Name</td>
                        <td>@book.Publisher?.Name</td>
                        <td>@book.Price tk</td>
                        <td>@book.Stock</td>
                        <td>
                            <a asp-page="./Edit" asp-route-id="@book.BookId" 
                               class="btn btn-sm btn-primary">Edit</a>
                            <a asp-page="./Detail" asp-route-id="@book.BookId" 
                               class="btn btn-sm btn-success">Details</a>
                            <a asp-page="./Delete" asp-route-id="@book.BookId" 
                               class="btn btn-sm btn-danger">Delete</a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
    
    <div class="col-md-3">
        @await Component.InvokeAsync("RecentBooks", new { count = 5 })
    </div>
</div>
```

**Razor Syntax**:
- `@page`: Marks as Razor Page
- `@model`: Specifies PageModel type
- `@Model.Property`: Access PageModel properties
- `asp-page`: Tag helper for page routing
- `asp-route-id`: Passes route parameter

---

### Step 5.2: Create Edit Page (with TempData)

**File: BookShop/Pages/Book/Edit.cshtml.cs**

```csharp
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

        public EditModel(IBookRepository bookRepository, 
                        IAuthorRepository authorRepository,
                        ICategoryRepository categoryRepository, 
                        IPublisherRepository publisherRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
        }

        [BindProperty]
        public Models.Book Book { get; set; } = new();

        public SelectList Authors { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Publishers { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null) return RedirectToPage("./Index");

            var book = _bookRepository.GetBookById(id.Value);
            if (book == null) return RedirectToPage("./Index");

            Book = book;
            Authors = new SelectList(_authorRepository.GetAllAuthors(), "AuthorId", "Name");
            Categories = new SelectList(_categoryRepository.GetAllCategories(), "CategoryId", "Name");
            Publishers = new SelectList(_publisherRepository.GetAllPublishers(), "PublisherId", "Name");
            
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the errors in the form.";
                Authors = new SelectList(_authorRepository.GetAllAuthors(), "AuthorId", "Name");
                Categories = new SelectList(_categoryRepository.GetAllCategories(), "CategoryId", "Name");
                Publishers = new SelectList(_publisherRepository.GetAllPublishers(), "PublisherId", "Name");
                return Page();
            }

            _bookRepository.UpdateBook(Book);
            TempData["SuccessMessage"] = "Book updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
```

**Logic**:
- `[BindProperty]`: Binds form data to property
- `OnGet()`: Loads book and dropdown data
- `OnPost()`: Validates and saves
- `TempData`: Persists data for ONE redirect
- `RedirectToPage()`: Redirects after save (PRG pattern)

---

**File: BookShop/Pages/Book/Edit.cshtml**

```razor
@page
@model BookShop.Pages.Book.EditModel
@{
    ViewData["Title"] = "Edit Book";
}

<h1>Edit Book</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <form method="post">
            <input type="hidden" asp-for="Book.BookId" />
            
            <div class="form-group mb-3">
                <label asp-for="Book.BookTitle" class="control-label"></label>
                <input asp-for="Book.BookTitle" class="form-control" />
                <span asp-validation-for="Book.BookTitle" class="text-danger"></span>
            </div>
            
            <div class="form-group mb-3">
                <label asp-for="Book.BookDescription" class="control-label"></label>
                <textarea asp-for="Book.BookDescription" class="form-control"></textarea>
                <span asp-validation-for="Book.BookDescription" class="text-danger"></span>
            </div>
            
            <div class="form-group mb-3">
                <label asp-for="Book.AuthorId" class="control-label">Author</label>
                <select asp-for="Book.AuthorId" class="form-control" asp-items="Model.Authors"></select>
            </div>
            
            <div class="form-group mb-3">
                <label asp-for="Book.CategoryId" class="control-label">Category</label>
                <select asp-for="Book.CategoryId" class="form-control" asp-items="Model.Categories"></select>
            </div>
            
            <div class="form-group mb-3">
                <label asp-for="Book.PublisherId" class="control-label">Publisher</label>
                <select asp-for="Book.PublisherId" class="form-control" asp-items="Model.Publishers"></select>
            </div>
            
            <div class="form-group mb-3">
                <label asp-for="Book.Price" class="control-label"></label>
                <input asp-for="Book.Price" class="form-control" type="number" step="0.01" />
            </div>
            
            <div class="form-group mb-3">
                <label asp-for="Book.Stock" class="control-label"></label>
                <input asp-for="Book.Stock" class="form-control" type="number" />
            </div>
            
            <div class="form-group mt-3">
                <input type="submit" value="Save" class="btn btn-primary" />
                <a asp-page="Index" class="btn btn-secondary">Back to List</a>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

**Tag Helpers**:
- `asp-for`: Binds to model property (generates name, id, value)
- `asp-validation-for`: Shows validation errors
- `asp-items`: Populates dropdown from SelectList

---

### Step 5.3: Create Page (with AJAX)

**File: BookShop/Pages/Book/Create.cshtml.cs**

```csharp
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

        public CreateModel(IBookRepository bookRepository, 
                          IAuthorRepository authorRepository,
                          ICategoryRepository categoryRepository, 
                          IPublisherRepository publisherRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _publisherRepository = publisherRepository;
        }

        [BindProperty]
        public Models.Book Book { get; set; } = new();

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
```

---

**File: BookShop/Pages/Book/Create.cshtml**

```razor
@page
@model BookShop.Pages.Book.CreateModel
@{
    ViewData["Title"] = "Create Book";
}

<h1>Create Book</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <div id="message" class="alert" style="display:none;"></div>
        <form id="createBookForm" method="post">
            <partial name="_BookFormPartial" model="Model.Book" />
            
            <div class="form-group mt-3">
                <input type="submit" value="Create" class="btn btn-primary" />
                <a asp-page="Index" class="btn btn-secondary">Back to List</a>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    <script>
        $(document).ready(function() {
            $('#createBookForm').on('submit', function(e) {
                e.preventDefault();
                
                var bookData = {
                    bookTitle: $('#BookTitle').val(),
                    bookDescription: $('#BookDescription').val(),
                    authorId: parseInt($('#AuthorId').val()) || 0,
                    categoryId: parseInt($('#CategoryId').val()) || 0,
                    publisherId: parseInt($('#PublisherId').val()) || 0,
                    price: parseFloat($('#Price').val()) || 0,
                    stock: parseInt($('#Stock').val()) || 0
                };
                
                $.ajax({
                    url: '/api/books',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(bookData),
                    success: function(response) {
                        $('#message').removeClass('alert-danger')
                                    .addClass('alert-success')
                                    .text(response.message).show();
                        $('#createBookForm')[0].reset();
                    },
                    error: function(xhr) {
                        var errorMsg = 'Failed to create book.';
                        if (xhr.responseJSON && xhr.responseJSON.message) {
                            errorMsg = xhr.responseJSON.message;
                        }
                        $('#message').removeClass('alert-success')
                                    .addClass('alert-danger')
                                    .text(errorMsg).show();
                    }
                });
            });
        });
    </script>
}
```

**AJAX Logic**:
- `e.preventDefault()`: Stops form submission
- Collects form data into JSON object
- `$.ajax()`: Sends POST request to API
- Success: Shows message, resets form (no page reload)
- Error: Shows error message

---

