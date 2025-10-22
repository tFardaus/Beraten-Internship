# BookShop Project - Task-by-Task Guide (Part 3)

## TASK 22: CREATE STORED PROCEDURE

### Create Stored Procedure in SQL Server

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
        b.Price,
        b.Stock
    FROM Authors a
    LEFT JOIN Books b ON a.AuthorId = b.AuthorId
    WHERE a.AuthorId = @AuthorId
END
```

### Create DTO for Stored Procedure

**BookShop.Models/AuthorWithBooksDto.cs**

```csharp
namespace BookShop.Models
{
    public class AuthorWithBooksDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public int? BookId { get; set; }
        public string? BookTitle { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
    }
}
```

### Update AppDbContext

```csharp
public DbSet<AuthorWithBooksDto> AuthorWithBooksResults { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Existing configurations...

    modelBuilder.Entity<AuthorWithBooksDto>().HasNoKey();
}
```

### Update IAuthorRepository

```csharp
IEnumerable<AuthorWithBooksDto> GetAuthorWithBooks(int authorId);
```

### Update SqlAuthorRepository

```csharp
using Microsoft.EntityFrameworkCore;

public IEnumerable<AuthorWithBooksDto> GetAuthorWithBooks(int authorId)
{
    return _context.AuthorWithBooksResults
        .FromSqlRaw("EXEC GetAuthorWithBooks @p0", authorId)
        .ToList();
}
```

---

## TASK 23: CREATE AUTHOR DETAILS PAGE

### BookShop/Pages/Author/Details.cshtml.cs

```csharp
using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Author
{
    public class DetailsModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;

        public DetailsModel(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public AuthorWithBooksDto AuthorInfo { get; set; } = new AuthorWithBooksDto();
        public List<AuthorWithBooksDto> Books { get; set; } = new List<AuthorWithBooksDto>();

        public IActionResult OnGet(int id)
        {
            var results = _authorRepository.GetAuthorWithBooks(id).ToList();

            if (!results.Any())
            {
                return NotFound();
            }

            AuthorInfo = results.First();
            Books = results.Where(r => r.BookId.HasValue).ToList();

            return Page();
        }
    }
}
```

### BookShop/Pages/Author/Details.cshtml

```html
@page
@model BookShop.Pages.Author.DetailsModel

<h2>Author Details</h2>

<div class="card mb-4">
    <div class="card-body">
        <h3>@Model.AuthorInfo.AuthorName</h3>
        <p><strong>Biography:</strong> @Model.AuthorInfo.Biography</p>
    </div>
</div>

<h3>Books by this Author</h3>

@if (Model.Books.Any())
{
    <table class="table table-striped">
        <thead>
            <tr>
                <th>Title</th>
                <th>Price</th>
                <th>Stock</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var book in Model.Books)
            {
                <tr>
                    <td>@book.BookTitle</td>
                    <td>@book.Price tk</td>
                    <td>@book.Stock</td>
                </tr>
            }
        </tbody>
    </table>
}
else
{
    <p>No books found for this author.</p>
}

<a href="/Author/Index" class="btn btn-secondary">Back to List</a>
```

### Update Author Index.cshtml

Add Details link in the Actions column:

```html
<td>
    <a href="/Author/Details?id=@author.AuthorId" class="btn btn-sm btn-info">Details</a>
    <a href="/Author/Edit?id=@author.AuthorId" class="btn btn-sm btn-warning">Edit</a>
    <a href="/Author/Delete?id=@author.AuthorId" class="btn btn-sm btn-danger">Delete</a>
</td>
```

---

## TASK 24: CREATE PARTIAL VIEWS

### BookShop/Pages/Shared/_AuthorFormPartial.cshtml

```html
@model BookShop.Models.Author

<div class="mb-3">
    <label for="Name" class="form-label">Name</label>
    <input type="text" id="Name" name="Name" value="@Model?.Name" class="form-control" />
    <span id="Name-error" class="text-danger"></span>
</div>

<div class="mb-3">
    <label for="Biography" class="form-label">Biography</label>
    <textarea id="Biography" name="Biography" class="form-control">@Model?.Biography</textarea>
</div>
```

### BookShop/Pages/Shared/_CategoryFormPartial.cshtml

```html
@model BookShop.Models.Category

<div class="mb-3">
    <label for="Name" class="form-label">Name</label>
    <input type="text" id="Name" name="Name" value="@Model?.Name" class="form-control" />
    <span id="Name-error" class="text-danger"></span>
</div>

<div class="mb-3">
    <label for="Description" class="form-label">Description</label>
    <textarea id="Description" name="Description" class="form-control">@Model?.Description</textarea>
</div>
```

### BookShop/Pages/Shared/_PublisherFormPartial.cshtml

```html
@model BookShop.Models.Publisher

<div class="mb-3">
    <label for="Name" class="form-label">Name</label>
    <input type="text" id="Name" name="Name" value="@Model?.Name" class="form-control" />
    <span id="Name-error" class="text-danger"></span>
</div>

<div class="mb-3">
    <label for="Address" class="form-label">Address</label>
    <input type="text" id="Address" name="Address" value="@Model?.Address" class="form-control" />
</div>

<div class="mb-3">
    <label for="Phone" class="form-label">Phone</label>
    <input type="text" id="Phone" name="Phone" value="@Model?.Phone" class="form-control" />
</div>
```

### BookShop/Pages/Shared/_CustomerFormPartial.cshtml

```html
@model BookShop.Models.Customer

<div class="mb-3">
    <label for="Name" class="form-label">Name</label>
    <input type="text" id="Name" name="Name" value="@Model?.Name" class="form-control" />
    <span id="Name-error" class="text-danger"></span>
</div>

<div class="mb-3">
    <label for="Email" class="form-label">Email</label>
    <input type="email" id="Email" name="Email" value="@Model?.Email" class="form-control" />
</div>

<div class="mb-3">
    <label for="Phone" class="form-label">Phone</label>
    <input type="text" id="Phone" name="Phone" value="@Model?.Phone" class="form-control" />
</div>

<div class="mb-3">
    <label for="Address" class="form-label">Address</label>
    <input type="text" id="Address" name="Address" value="@Model?.Address" class="form-control" />
</div>
```

---

## TASK 25: USE PARTIAL VIEWS IN CREATE PAGES

### Update Author Create.cshtml

```html
@page
@model BookShop.Pages.Author.CreateModel

<h2>Create Author</h2>

<div id="message" class="alert" style="display:none;"></div>

<form id="authorForm">
    <partial name="_AuthorFormPartial" model="null" />
    
    <button type="submit" class="btn btn-primary">Create</button>
    <a href="/Author/Index" class="btn btn-secondary">Cancel</a>
</form>

@section Scripts {
    <script>
        document.getElementById('authorForm').addEventListener('submit', function (e) {
            e.preventDefault();

            const author = {
                name: document.getElementById('Name').value,
                biography: document.getElementById('Biography').value
            };

            fetch('/api/AuthorApi', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(author)
            })
            .then(response => response.json())
            .then(data => {
                const messageDiv = document.getElementById('message');
                if (data.success) {
                    messageDiv.className = 'alert alert-success';
                    messageDiv.textContent = data.message;
                    messageDiv.style.display = 'block';
                    document.getElementById('authorForm').reset();
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

### Use Partials in Other Create Pages

Update Create pages for Category, Publisher, and Customer to use their respective partial views.

**Note**: Edit pages should NOT use partials due to model binding issues. Keep inline fields with asp-for="Entity.Property" syntax.

---

## TASK 26: CREATE VIEW COMPONENTS

### BookShop/ViewComponents/RecentBooksViewComponent.cs

```csharp
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ViewComponents
{
    public class RecentBooksViewComponent : ViewComponent
    {
        private readonly IBookRepository _bookRepository;

        public RecentBooksViewComponent(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IViewComponentResult Invoke()
        {
            var recentBooks = _bookRepository.GetAllBook()
                .OrderByDescending(b => b.BookId)
                .Take(5)
                .ToList();

            return View(recentBooks);
        }
    }
}
```

### BookShop/Pages/Shared/Components/RecentBooks/Default.cshtml

```html
@model IEnumerable<BookShop.Models.Book>

<div class="card">
    <div class="card-header">
        <h5>Recent Books</h5>
    </div>
    <div class="card-body">
        @if (Model.Any())
        {
            <ul class="list-group">
                @foreach (var book in Model)
                {
                    <li class="list-group-item">
                        <strong>@book.BookTitle</strong>
                        <span class="badge bg-primary float-end">@book.Price tk</span>
                    </li>
                }
            </ul>
        }
        else
        {
            <p>No recent books found.</p>
        }
    </div>
</div>
```

### BookShop/ViewComponents/CategoryStatsViewComponent.cs

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
            var categories = _categoryRepository.GetAllCategory()
                .Select(c => new
                {
                    c.Name,
                    BookCount = c.Books?.Count ?? 0
                })
                .OrderByDescending(c => c.BookCount)
                .ToList();

            return View(categories);
        }
    }
}
```

### BookShop/Pages/Shared/Components/CategoryStats/Default.cshtml

```html
@model IEnumerable<dynamic>

<div class="card">
    <div class="card-header">
        <h5>Category Statistics</h5>
    </div>
    <div class="card-body">
        @if (Model.Any())
        {
            <ul class="list-group">
                @foreach (var category in Model)
                {
                    <li class="list-group-item">
                        @category.Name
                        <span class="badge bg-info float-end">@category.BookCount books</span>
                    </li>
                }
            </ul>
        }
        else
        {
            <p>No categories found.</p>
        }
    </div>
</div>
```

---

## TASK 27: USE VIEW COMPONENTS IN PAGES

### Update Book Index.cshtml

```html
@page
@model BookShop.Pages.Book.IndexModel

<div class="row">
    <div class="col-md-9">
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
            <thead>
                <tr>
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
                    <tr>
                        <td>@book.BookTitle</td>
                        <td>@book.Author?.Name</td>
                        <td>@book.Category?.Name</td>
                        <td>@book.Publisher?.Name</td>
                        <td>@book.Price tk</td>
                        <td>@book.Stock</td>
                        <td>
                            <a href="/Book/Edit?id=@book.BookId" class="btn btn-sm btn-warning">Edit</a>
                            <a href="/Book/Delete?id=@book.BookId" class="btn btn-sm btn-danger">Delete</a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>

    <div class="col-md-3">
        @await Component.InvokeAsync("RecentBooks")
    </div>
</div>
```

### Update Category Index.cshtml

```html
@page
@model BookShop.Pages.Category.IndexModel

<div class="row">
    <div class="col-md-9">
        <h2>Categories</h2>

        <form method="get" class="mb-3">
            <div class="row">
                <div class="col-md-6">
                    <input type="text" name="searchTerm" value="@Model.SearchTerm" class="form-control" placeholder="Search categories..." />
                </div>
                <div class="col-md-3">
                    <button type="submit" class="btn btn-primary">Search</button>
                    <a href="/Category/Index" class="btn btn-secondary">Clear</a>
                </div>
            </div>
        </form>

        <a href="/Category/Create" class="btn btn-primary mb-3">Add New Category</a>

        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var category in Model.Categories)
                {
                    <tr>
                        <td>@category.Name</td>
                        <td>@category.Description</td>
                        <td>
                            <a href="/Category/Edit?id=@category.CategoryId" class="btn btn-sm btn-warning">Edit</a>
                            <a href="/Category/Delete?id=@category.CategoryId" class="btn btn-sm btn-danger">Delete</a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>

    <div class="col-md-3">
        @await Component.InvokeAsync("CategoryStats")
    </div>
</div>
```

---

## TASK 28: TESTING GUIDE

### Test Project Setup

1. Run the application:
```bash
cd BookShop
dotnet run
```

2. Navigate to `https://localhost:5001`

### Test Authentication

1. **Register New User**
   - Go to `/Account/Register`
   - Fill in: Full Name, Email, Password, Confirm Password
   - Click Register
   - Should redirect to home page and show username in navbar

2. **Logout**
   - Click Logout in navbar
   - Should redirect to Login page

3. **Login**
   - Go to `/Account/Login`
   - Enter registered email and password
   - Click Login
   - Should redirect to home page

4. **Test Authorization**
   - Logout
   - Try to access `/Book/Index` directly
   - Should redirect to Login page

### Test CRUD Operations

1. **Create Author**
   - Login first
   - Go to `/Author/Index`
   - Click "Add New Author"
   - Fill in Name and Biography
   - Click Create
   - Should show success message without page reload
   - Form should reset

2. **Create Category**
   - Go to `/Category/Index`
   - Click "Add New Category"
   - Fill in Name and Description
   - Click Create
   - Should show success message

3. **Create Publisher**
   - Go to `/Publisher/Index`
   - Click "Add New Publisher"
   - Fill in Name, Address, Phone
   - Click Create
   - Should show success message

4. **Create Book**
   - Go to `/Book/Index`
   - Click "Add New Book"
   - Fill in all fields and select Author, Category, Publisher
   - Click Create
   - Should show success message

5. **Edit Book**
   - Go to `/Book/Index`
   - Click Edit on any book
   - Modify fields
   - Click Update
   - Should redirect to Index with TempData success message

6. **Delete Book**
   - Go to `/Book/Index`
   - Click Delete on any book
   - Confirm deletion
   - Should redirect to Index with TempData success message

### Test Search Functionality

1. **Search Books**
   - Go to `/Book/Index`
   - Enter search term in search box
   - Click Search
   - Should show filtered results
   - Search term should persist in input field
   - Click Clear to reset

2. **Search Authors**
   - Go to `/Author/Index`
   - Test search functionality
   - Verify results

3. **Test Empty Search**
   - Leave search box empty
   - Click Search
   - Should show all records

### Test Stored Procedure

1. **View Author Details**
   - Go to `/Author/Index`
   - Click Details on any author
   - Should show author info and list of books
   - Verify data comes from stored procedure

### Test View Components

1. **Recent Books Component**
   - Go to `/Book/Index`
   - Check right sidebar
   - Should show 5 most recent books
   - Verify prices display with "tk"

2. **Category Stats Component**
   - Go to `/Category/Index`
   - Check right sidebar
   - Should show categories with book counts
   - Verify counts are accurate

### Test Partial Views

1. **Create with Partial**
   - Go to `/Author/Create`
   - Verify form fields render correctly
   - Submit form
   - Verify AJAX works with partial view fields

2. **Edit without Partial**
   - Go to `/Author/Edit?id=1`
   - Verify inline fields (not partial)
   - Submit form
   - Verify model binding works correctly

### Test TempData Messages

1. **Success Message**
   - Edit any entity
   - Submit form
   - Should see green success alert at top of page
   - Refresh page
   - Message should disappear

2. **Error Message**
   - Edit any entity
   - Leave required field empty
   - Submit form
   - Should see red error alert

### Test Validation

1. **Client-Side Validation**
   - Go to any Create/Edit page
   - Leave required fields empty
   - Try to submit
   - Should show validation errors

2. **Server-Side Validation**
   - Disable JavaScript in browser
   - Try to submit invalid form
   - Should show validation errors from server

---

## TASK 29: COMMON ISSUES AND SOLUTIONS

### Issue 1: Migration Fails

**Problem**: `dotnet ef migrations add` fails

**Solution**:
```bash
# Ensure you're in the correct project
cd BookShop

# Ensure packages are installed
dotnet restore

# Try migration again
dotnet ef migrations add MigrationName
```

### Issue 2: Database Connection Error

**Problem**: Cannot connect to database

**Solution**:
- Check connection string in `appsettings.json`
- Verify SQL Server is running
- Update `Server` name to match your SQL Server instance

### Issue 3: Identity Tables Not Created

**Problem**: Identity tables missing after migration

**Solution**:
- Ensure `base.OnModelCreating(modelBuilder)` is called in AppDbContext
- Delete existing migrations
- Create new migration: `dotnet ef migrations add AddIdentity`
- Update database: `dotnet ef database update`

### Issue 4: AJAX Not Working

**Problem**: Form submits but no success message

**Solution**:
- Check browser console for JavaScript errors
- Verify API endpoint URL is correct
- Ensure field IDs match JavaScript selectors
- Check API controller returns proper JSON response

### Issue 5: TempData Not Showing

**Problem**: TempData messages don't appear

**Solution**:
- Verify TempData code is in _Layout.cshtml
- Ensure you're using `RedirectToPage()` not `Page()`
- Check Bootstrap CSS is loaded

### Issue 6: Partial View Model Binding

**Problem**: Edit page doesn't save data when using partial

**Solution**:
- Don't use partials in Edit pages
- Use inline fields with `asp-for="Entity.Property"` syntax
- Partials work for Create pages with AJAX only

### Issue 7: View Component Not Found

**Problem**: View component doesn't render

**Solution**:
- Verify folder structure: `Pages/Shared/Components/ComponentName/Default.cshtml`
- Check component class name matches folder name
- Use `@await Component.InvokeAsync("ComponentName")` syntax

### Issue 8: Stored Procedure Error

**Problem**: Stored procedure execution fails

**Solution**:
- Verify stored procedure exists in database
- Check DTO is configured as keyless: `HasNoKey()`
- Ensure parameter syntax is correct: `@p0`, `@p1`, etc.
- Verify DTO properties match stored procedure columns

---

## TASK 30: PROJECT COMPLETION CHECKLIST

### Models Layer
- [x] All entity models created (Book, Author, Category, Publisher, Customer, Order, OrderItem)
- [x] DTOs created (BookDetailsDto, AuthorWithBooksDto)
- [x] ApplicationUser created for Identity

### Services Layer
- [x] AppDbContext configured with DbSets
- [x] Relationships configured with Fluent API
- [x] Repository interfaces created for all entities
- [x] Repository implementations with LINQ queries
- [x] Search methods implemented
- [x] Stored procedure method implemented

### Presentation Layer
- [x] Program.cs configured with services and middleware
- [x] Connection string in appsettings.json
- [x] Index pages for all entities
- [x] Create pages for all entities
- [x] Edit pages for all entities
- [x] Delete pages for all entities
- [x] Search functionality on all Index pages
- [x] AJAX on Create pages
- [x] TempData messages on Edit/Delete pages

### Authentication
- [x] Identity configured in Program.cs
- [x] Identity migration applied
- [x] Register page created
- [x] Login page created
- [x] Logout page created
- [x] Navigation menu updated with auth status
- [x] Global authorization configured

### Advanced Features
- [x] Stored procedure created in database
- [x] Author Details page using stored procedure
- [x] Partial views created for forms
- [x] Partial views used in Create pages
- [x] View components created (RecentBooks, CategoryStats)
- [x] View components used in Index pages

### Testing
- [x] All CRUD operations tested
- [x] Search functionality tested
- [x] Authentication flow tested
- [x] AJAX functionality tested
- [x] TempData messages tested
- [x] Stored procedure tested
- [x] View components tested
- [x] Partial views tested

### Documentation
- [x] Implementation guide created
- [x] Code comments added
- [x] Testing guide created
- [x] Common issues documented

---

## PROJECT COMPLETE!

Your BookShop project now includes:
- Complete 3-layer architecture
- Full CRUD operations for all entities
- Search functionality
- AJAX for Create pages
- TempData messages for Edit/Delete
- ASP.NET Core Identity authentication
- Stored procedures
- Partial views
- View components
- Comprehensive testing

**Next Steps**:
- Add more features (e.g., shopping cart, order processing)
- Implement role-based authorization
- Add pagination for large datasets
- Implement file upload for book covers
- Add reporting features
- Deploy to production
