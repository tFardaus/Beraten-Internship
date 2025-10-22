# BookShop Project - Implementation Guide (Part 3)

## PHASE 6: API CONTROLLERS (for AJAX)

### Step 6.1: Create API Controller

**File: BookShop/Controllers/BooksController.cs**

```csharp
using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { 
                    success = false, 
                    message = string.Join(", ", errors) 
                });
            }

            try
            {
                _bookRepository.AddBook(book);
                return Ok(new { 
                    success = true, 
                    message = "Book created successfully!" 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    success = false, 
                    message = ex.InnerException?.Message ?? ex.Message 
                });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            book.BookId = id;
            _bookRepository.UpdateBook(book);
            return Ok(new { 
                success = true, 
                message = "Book updated successfully!" 
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _bookRepository.DeleteBook(id);
            return Ok(new { 
                success = true, 
                message = "Book deleted successfully!" 
            });
        }
    }
}
```

**Logic**:
- `[Route("api/[controller]")]`: Maps to `/api/books`
- `[ApiController]`: Enables automatic model validation
- `[HttpPost]`: Handles POST requests
- `[FromBody]`: Deserializes JSON to object
- Returns JSON responses (Ok, BadRequest)

**Repeat for**: Authors, Categories, Publishers, Customers, Orders

---

## PHASE 7: AUTHENTICATION (ASP.NET Core Identity)

### Step 7.1: Create ApplicationUser Model

**File: BookShop.Models/ApplicationUser.cs**

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

**Logic**: Extends `IdentityUser` to add custom properties

---

### Step 7.2: Install Identity Package

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.10
```

---

### Step 7.3: Update AppDbContext

```csharp
public class AppDbContext : IdentityDbContext<ApplicationUser>  // Changed
{
    // ... existing code ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);  // CRITICAL - adds Identity tables
        
        // ... existing relationship configurations ...
    }
}
```

**Why `base.OnModelCreating()`?**
- Configures 7 Identity tables (AspNetUsers, AspNetRoles, etc.)
- Without it, Identity won't work

---

### Step 7.4: Create Migration

```bash
dotnet ef migrations add AddIdentity --project BookShop.Services --startup-project BookShop
dotnet ef database update --project BookShop.Services --startup-project BookShop
```

**What happens?**
- Creates migration file with SQL commands
- Applies migration to database
- Creates 7 Identity tables

---

### Step 7.5: Create Register Page

**File: BookShop/Pages/Account/Register.cshtml.cs**

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

        public RegisterModel(UserManager<ApplicationUser> userManager, 
                           SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                    FullName = Input.FullName,
                    UserName = Input.Email, 
                    Email = Input.Email 
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
            }
            
            return Page();
        }
    }
}
```

**Logic**:
- `UserManager`: Manages users (create, update, delete)
- `SignInManager`: Manages sign-in/sign-out
- `CreateAsync()`: Hashes password and saves to database
- `SignInAsync()`: Creates authentication cookie
- Nested `InputModel`: Separates form data from page logic

---

### Step 7.6: Create Login Page

**File: BookShop/Pages/Account/Login.cshtml.cs**

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using BookShop.Models;

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
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    Input.Email, 
                    Input.Password, 
                    Input.RememberMe, 
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return Page();
        }
    }
}
```

**Logic**:
- `PasswordSignInAsync()`: Verifies password hash
- `returnUrl`: Redirects back to original page after login
- `RememberMe`: Persistent cookie (stays after browser close)

---

### Step 7.7: Update _Layout.cshtml

**File: BookShop/Pages/Shared/_Layout.cshtml**

```razor
<ul class="navbar-nav">
    @if (User.Identity?.IsAuthenticated == true)
    {
        <li class="nav-item">
            <span class="nav-link text-dark">Hello, @User.Identity.Name</span>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" asp-page="/Account/Logout">Logout</a>
        </li>
    }
    else
    {
        <li class="nav-item">
            <a class="nav-link text-dark" asp-page="/Account/Login">Login</a>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" asp-page="/Account/Register">Register</a>
        </li>
    }
</ul>
```

**Logic**:
- `User.Identity.IsAuthenticated`: Populated by `UseAuthentication()` middleware
- Shows Login/Register when not authenticated
- Shows username/Logout when authenticated

---

### Step 7.8: Add TempData Messages to _Layout

```razor
<div class="container">
    @if (TempData["SuccessMessage"] != null)
    {
        <div class="alert alert-success alert-dismissible fade show mt-3" role="alert">
            <strong>Success!</strong> @TempData["SuccessMessage"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }
    @if (TempData["ErrorMessage"] != null)
    {
        <div class="alert alert-danger alert-dismissible fade show mt-3" role="alert">
            <strong>Error!</strong> @TempData["ErrorMessage"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }
    <main role="main" class="pb-3">
        @RenderBody()
    </main>
</div>
```

**Logic**:
- TempData persists for ONE redirect
- Automatically cleared after display
- Shows success/error messages after form submission

---

## PHASE 8: ADVANCED FEATURES

### Step 8.1: Stored Procedure Implementation

**Create in SSMS:**

```sql
CREATE PROCEDURE GetAuthorWithBooks
    @AuthorId INT
AS
BEGIN
    SELECT 
        a.AuthorId,
        a.Name AS AuthorName,
        a.Biography,
        b.BookId,
        b.BookTitle,
        b.BookDescription,
        b.Price,
        b.Stock
    FROM Authors a
    LEFT JOIN Books b ON a.AuthorId = b.AuthorId
    WHERE a.AuthorId = @AuthorId
END
```

**Create DTO:**

```csharp
public class AuthorWithBooksDto
{
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public int? BookId { get; set; }
    public string? BookTitle { get; set; }
    public string? BookDescription { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
}
```

**Update AppDbContext:**

```csharp
public DbSet<AuthorWithBooksDto> AuthorWithBooksResults { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // ... existing configurations ...
    
    modelBuilder.Entity<AuthorWithBooksDto>().HasNoKey();
}
```

**Add to Repository:**

```csharp
public IEnumerable<AuthorWithBooksDto> GetAuthorWithBooks(int authorId)
{
    return _context.AuthorWithBooksResults
        .FromSqlRaw("EXEC GetAuthorWithBooks @p0", authorId)
        .ToList();
}
```

**Logic**:
- Stored procedures are pre-compiled SQL
- Better performance for complex queries
- `FromSqlRaw()`: Executes raw SQL
- `HasNoKey()`: DTO doesn't have primary key

---

### Step 8.2: Partial Views (Reusable UI)

**File: BookShop/Pages/Shared/_AuthorFormPartial.cshtml**

```razor
@model BookShop.Models.Author

<div class="form-group mb-3">
    <label asp-for="Name" class="control-label"></label>
    <input asp-for="Name" class="form-control" />
    <span asp-validation-for="Name" class="text-danger"></span>
</div>

<div class="form-group mb-3">
    <label asp-for="Biography" class="control-label"></label>
    <textarea asp-for="Biography" class="form-control" rows="4"></textarea>
    <span asp-validation-for="Biography" class="text-danger"></span>
</div>
```

**Usage in Create Page:**

```razor
<form id="createAuthorForm" method="post">
    <partial name="_AuthorFormPartial" model="Model.Author" />
    <button type="submit">Create</button>
</form>
```

**Logic**:
- DRY principle: Define once, use everywhere
- Consistent UI across pages
- Easy maintenance

**Note**: Partials work for Create (AJAX) but NOT Edit (model binding needs prefix)

---

### Step 8.3: View Components (Dynamic Widgets)

**File: BookShop/ViewComponents/CategoryStatsViewComponent.cs**

```csharp
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ViewComponents
{
    public class CategoryStatsViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryStatsViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _categoryRepository.GetAllCategories()
                .Select(c => new
                {
                    CategoryName = c.Name,
                    BookCount = c.Books.Count
                })
                .OrderByDescending(c => c.BookCount)
                .ToList();
            
            return View(categories);
        }
    }
}
```

**File: BookShop/Pages/Shared/Components/CategoryStats/Default.cshtml**

```razor
@model IEnumerable<dynamic>

<div class="card">
    <div class="card-header bg-success text-white">
        <h5 class="mb-0">Books by Category</h5>
    </div>
    <ul class="list-group list-group-flush">
        @foreach (var item in Model)
        {
            <li class="list-group-item d-flex justify-content-between">
                @item.CategoryName
                <span class="badge bg-primary">@item.BookCount</span>
            </li>
        }
    </ul>
</div>
```

**Usage:**

```razor
<div class="col-md-3">
    @await Component.InvokeAsync("CategoryStats")
</div>
```

**Logic**:
- Self-contained component with logic + view
- Reusable across pages
- Dependency injection works
- `Invoke()` method called automatically

---

## PHASE 9: KEY CONCEPTS SUMMARY

### 1. Repository Pattern
**Why?**
- Abstracts data access
- Easy to test (mock repositories)
- Centralized database logic

### 2. Dependency Injection
**Why?**
- Loose coupling
- Easy to swap implementations
- Automatic lifetime management

### 3. AJAX vs Form Submission
**AJAX (Create pages):**
- No page reload
- Better UX
- Inline messages

**Form Submission (Edit pages):**
- Page reload
- TempData messages
- Proper model binding

### 4. TempData
**Why?**
- Persists for ONE redirect
- Perfect for success/error messages
- Automatically cleared

### 5. Authentication Flow
```
1. User registers → Password hashed → Saved to AspNetUsers
2. User logs in → Password verified → Cookie created
3. User accesses page → Cookie read → User.Identity populated
4. Authorization checks → Allow/Deny access
```

### 6. Model Binding
**How it works:**
```
Form: <input name="Author.Name" value="John" />
      ↓
POST: Author.Name=John
      ↓
PageModel: [BindProperty] public Author Author { get; set; }
      ↓
Result: Author.Name = "John"
```

---

## TESTING GUIDE

### Test Authentication:
1. Navigate to `/Book/Index` → Redirected to `/Account/Login`
2. Register new user → Redirected to home
3. Logout → Login/Register links appear
4. Login → Username appears in nav

### Test CRUD Operations:
1. **Create**: Fill form → Submit → See inline success message
2. **Edit**: Click Edit → Modify → Save → See TempData success message
3. **Delete**: Click Delete → Confirm → See TempData success message
4. **Search**: Enter term → Click Search → See filtered results

### Test View Components:
1. Navigate to `/Category/Index`
2. See "Books by Category" widget in sidebar
3. Verify book counts are correct

### Test Stored Procedure:
1. Navigate to `/Author/Details/1`
2. See author info and books list
3. Data pulled from stored procedure

---

## CONCLUSION

This project demonstrates:
- ✅ 3-Layer Architecture
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ CRUD Operations
- ✅ AJAX Implementation
- ✅ Authentication & Authorization
- ✅ TempData Messages
- ✅ Stored Procedures
- ✅ Partial Views
- ✅ View Components
- ✅ Search Functionality
- ✅ Relational Database Design

**Total Files**: ~60 files
**Total Lines of Code**: ~3000+ lines
**Technologies**: ASP.NET Core 8.0, EF Core, Identity, Bootstrap, jQuery

