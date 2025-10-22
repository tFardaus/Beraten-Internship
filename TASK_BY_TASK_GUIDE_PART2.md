# BookShop Project - Task-by-Task Guide (Part 2)

## TASK 14: ADD SEARCH FUNCTIONALITY

### Update IBookRepository.cs

```csharp
IEnumerable<Book> SearchBooks(string searchTerm);
```

### Update SqlBookRepository.cs

```csharp
public IEnumerable<Book> SearchBooks(string searchTerm)
{
    return _context.Books
        .Include(b => b.Author)
        .Include(b => b.Category)
        .Include(b => b.Publisher)
        .Where(b => b.BookTitle.Contains(searchTerm))
        .ToList();
}
```

### Update Book Index.cshtml.cs

```csharp
public string SearchTerm { get; set; } = string.Empty;

public void OnGet(string searchTerm)
{
    SearchTerm = searchTerm ?? string.Empty;
    
    if (!string.IsNullOrEmpty(searchTerm))
    {
        Books = _bookRepository.SearchBooks(searchTerm);
    }
    else
    {
        Books = _bookRepository.GetAllBook();
    }
}
```

### Update Book Index.cshtml

```html
@page
@model BookShop.Pages.Book.IndexModel

<h2>Books</h2>

<form method="get" class="mb-3">
    <div class="row">
        <div class="col-md-6">
            <input type="text" name="searchTerm" value="@Model.SearchTerm" class="form-control" placeholder="Search books..." />
        </div>
        <div class="col-md-3">
            <button type="submit" class="btn btn-primary">Search</button>
            <a href="/Book/Index" class="btn btn-secondary">Clear</a>
        </div>
    </div>
</form>

<a href="/Book/Create" class="btn btn-primary mb-3">Add New Book</a>

<table class="table table-striped">
    <!-- Same table as before -->
</table>
```

### Add Search to Other Entities

Add similar search functionality to:
- Author (search by Name)
- Category (search by Name)
- Publisher (search by Name)
- Customer (search by Name)

---

## TASK 15: ADD AJAX TO CREATE PAGES

### Create API Controller for Books

**BookShop/Controllers/BookApiController.cs**

```csharp
using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookApiController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            _bookRepository.AddBook(book);
            return Ok(new { success = true, message = "Book created successfully!" });
        }
    }
}
```

### Update Book Create.cshtml

```html
@page
@model BookShop.Pages.Book.CreateModel

<h2>Create Book</h2>

<div id="message" class="alert" style="display:none;"></div>

<form id="bookForm">
    <div class="mb-3">
        <label for="BookTitle" class="form-label">Title</label>
        <input type="text" id="BookTitle" class="form-control" />
        <span id="BookTitle-error" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label for="BookDescription" class="form-label">Description</label>
        <textarea id="BookDescription" class="form-control"></textarea>
    </div>

    <div class="mb-3">
        <label for="Price" class="form-label">Price</label>
        <input type="number" id="Price" class="form-control" step="0.01" />
        <span id="Price-error" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label for="Stock" class="form-label">Stock</label>
        <input type="number" id="Stock" class="form-control" />
    </div>

    <div class="mb-3">
        <label for="AuthorId" class="form-label">Author</label>
        <select id="AuthorId" class="form-control">
            <option value="">Select Author</option>
            @foreach (var author in Model.Authors)
            {
                <option value="@author.AuthorId">@author.Name</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label for="CategoryId" class="form-label">Category</label>
        <select id="CategoryId" class="form-control">
            <option value="">Select Category</option>
            @foreach (var category in Model.Categories)
            {
                <option value="@category.CategoryId">@category.Name</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label for="PublisherId" class="form-label">Publisher</label>
        <select id="PublisherId" class="form-control">
            <option value="">Select Publisher</option>
            @foreach (var publisher in Model.Publishers)
            {
                <option value="@publisher.PublisherId">@publisher.Name</option>
            }
        </select>
    </div>

    <button type="submit" class="btn btn-primary">Create</button>
    <a href="/Book/Index" class="btn btn-secondary">Cancel</a>
</form>

@section Scripts {
    <script>
        document.getElementById('bookForm').addEventListener('submit', function (e) {
            e.preventDefault();

            const book = {
                bookTitle: document.getElementById('BookTitle').value,
                bookDescription: document.getElementById('BookDescription').value,
                price: parseFloat(document.getElementById('Price').value),
                stock: parseInt(document.getElementById('Stock').value),
                authorId: parseInt(document.getElementById('AuthorId').value),
                categoryId: parseInt(document.getElementById('CategoryId').value),
                publisherId: parseInt(document.getElementById('PublisherId').value)
            };

            fetch('/api/BookApi', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(book)
            })
            .then(response => response.json())
            .then(data => {
                const messageDiv = document.getElementById('message');
                if (data.success) {
                    messageDiv.className = 'alert alert-success';
                    messageDiv.textContent = data.message;
                    messageDiv.style.display = 'block';
                    document.getElementById('bookForm').reset();
                } else {
                    messageDiv.className = 'alert alert-danger';
                    messageDiv.textContent = data.message;
                    messageDiv.style.display = 'block';
                }
            });
        });
    </script>
}
```

### Add AJAX to Other Create Pages

Create API controllers and update Create pages for:
- Author
- Category
- Publisher
- Customer

---

## TASK 16: ADD TEMPDATA MESSAGES

### Update _Layout.cshtml

**BookShop/Pages/Shared/_Layout.cshtml**

Add after opening `<body>` tag:

```html
<body>
    @if (TempData["SuccessMessage"] != null)
    {
        <div class="alert alert-success alert-dismissible fade show" role="alert">
            @TempData["SuccessMessage"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }

    @if (TempData["ErrorMessage"] != null)
    {
        <div class="alert alert-danger alert-dismissible fade show" role="alert">
            @TempData["ErrorMessage"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }

    <!-- Rest of layout -->
</body>
```

### Update Edit Pages with TempData

**Book Edit.cshtml.cs**

```csharp
public IActionResult OnPost()
{
    if (!ModelState.IsValid)
    {
        TempData["ErrorMessage"] = "Please fix the validation errors.";
        Authors = _authorRepository.GetAllAuthor();
        Categories = _categoryRepository.GetAllCategory();
        Publishers = _publisherRepository.GetAllPublisher();
        return Page();
    }

    _bookRepository.UpdateBook(Book);
    TempData["SuccessMessage"] = "Book updated successfully!";
    return RedirectToPage("./Index");
}
```

### Update Delete Pages with TempData

**Book Delete.cshtml.cs**

```csharp
public IActionResult OnPost(int id)
{
    _bookRepository.DeleteBook(id);
    TempData["SuccessMessage"] = "Book deleted successfully!";
    return RedirectToPage("./Index");
}
```

### Add TempData to All Edit and Delete Pages

Update Edit and Delete pages for:
- Author
- Category
- Publisher
- Customer
- Order

---

## TASK 17: ADD AUTHENTICATION

### Install Identity Package

```bash
cd BookShop
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

### Create ApplicationUser Model

**BookShop.Models/ApplicationUser.cs**

```csharp
using Microsoft.AspNetCore.Identity;

namespace BookShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
```

### Update AppDbContext

**BookShop.Services/AppDbContext.cs**

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // CRITICAL for Identity

        // Relationship configurations...
    }
}
```

### Update Program.cs

```csharp
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToFolder("/Account");
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Repository registrations...

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // BEFORE UseAuthorization
app.UseAuthorization();
app.MapRazorPages();

app.Run();
```

### Create Identity Migration

```bash
dotnet ef migrations add AddIdentity
dotnet ef database update
```

---

## TASK 18: CREATE REGISTER PAGE

### BookShop/Pages/Account/Register.cshtml.cs

```csharp
using BookShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required]
            public string FullName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
```

### BookShop/Pages/Account/Register.cshtml

```html
@page
@model BookShop.Pages.Account.RegisterModel

<h2>Register</h2>

<form method="post">
    <div asp-validation-summary="All" class="text-danger"></div>

    <div class="mb-3">
        <label asp-for="Input.FullName" class="form-label"></label>
        <input asp-for="Input.FullName" class="form-control" />
        <span asp-validation-for="Input.FullName" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Input.Email" class="form-label"></label>
        <input asp-for="Input.Email" class="form-control" />
        <span asp-validation-for="Input.Email" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Input.Password" class="form-label"></label>
        <input asp-for="Input.Password" class="form-control" />
        <span asp-validation-for="Input.Password" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Input.ConfirmPassword" class="form-label"></label>
        <input asp-for="Input.ConfirmPassword" class="form-control" />
        <span asp-validation-for="Input.ConfirmPassword" class="text-danger"></span>
    </div>

    <button type="submit" class="btn btn-primary">Register</button>
</form>
```

---

## TASK 19: CREATE LOGIN PAGE

### BookShop/Pages/Account/Login.cshtml.cs

```csharp
using BookShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string ReturnUrl { get; set; } = string.Empty;

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
```

### BookShop/Pages/Account/Login.cshtml

```html
@page
@model BookShop.Pages.Account.LoginModel

<h2>Login</h2>

<form method="post">
    <div asp-validation-summary="All" class="text-danger"></div>

    <div class="mb-3">
        <label asp-for="Input.Email" class="form-label"></label>
        <input asp-for="Input.Email" class="form-control" />
        <span asp-validation-for="Input.Email" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Input.Password" class="form-label"></label>
        <input asp-for="Input.Password" class="form-control" />
        <span asp-validation-for="Input.Password" class="text-danger"></span>
    </div>

    <button type="submit" class="btn btn-primary">Login</button>
</form>
```

---

## TASK 20: CREATE LOGOUT PAGE

### BookShop/Pages/Account/Logout.cshtml.cs

```csharp
using BookShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }
    }
}
```

### BookShop/Pages/Account/Logout.cshtml

```html
@page
@model BookShop.Pages.Account.LogoutModel

<h2>Logout</h2>

<form method="post">
    <p>Are you sure you want to logout?</p>
    <button type="submit" class="btn btn-danger">Logout</button>
    <a href="/" class="btn btn-secondary">Cancel</a>
</form>
```

---

## TASK 21: UPDATE NAVIGATION MENU

### Update _Layout.cshtml Navigation

```html
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="/">BookShop</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav me-auto">
                <li class="nav-item">
                    <a class="nav-link" href="/Book/Index">Books</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/Author/Index">Authors</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/Category/Index">Categories</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/Publisher/Index">Publishers</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/Customer/Index">Customers</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/Order/Index">Orders</a>
                </li>
            </ul>
            <ul class="navbar-nav">
                @if (User.Identity.IsAuthenticated)
                {
                    <li class="nav-item">
                        <span class="nav-link">Hello, @User.Identity.Name</span>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Account/Logout">Logout</a>
                    </li>
                }
                else
                {
                    <li class="nav-item">
                        <a class="nav-link" href="/Account/Login">Login</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Account/Register">Register</a>
                    </li>
                }
            </ul>
        </div>
    </div>
</nav>
```

