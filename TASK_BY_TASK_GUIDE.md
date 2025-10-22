# BookShop Project - Task-by-Task Implementation Guide

## TASK 1: PROJECT SETUP

### Create Solution and Projects

```bash
dotnet new sln -n BookShop
dotnet new classlib -n BookShop.Models
dotnet new classlib -n BookShop.Services
dotnet new webapp -n BookShop

dotnet sln add BookShop.Models/BookShop.Models.csproj
dotnet sln add BookShop.Services/BookShop.Services.csproj
dotnet sln add BookShop/BookShop.csproj

cd BookShop.Services
dotnet add reference ../BookShop.Models/BookShop.Models.csproj

cd ../BookShop
dotnet add reference ../BookShop.Models/BookShop.Models.csproj
dotnet add reference ../BookShop.Services/BookShop.Services.csproj
```

---

## TASK 2: CREATE ENTITY MODELS

### BookShop.Models/Book.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string BookTitle { get; set; } = string.Empty;
        public string BookDescription { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public Author? Author { get; set; }
        public Category? Category { get; set; }
        public Publisher? Publisher { get; set; }
    }
}
```

### BookShop.Models/Author.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public ICollection<Book>? Books { get; set; }
    }
}
```

### BookShop.Models/Category.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Book>? Books { get; set; }
    }
}
```

### BookShop.Models/Publisher.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public ICollection<Book>? Books { get; set; }
    }
}
```

### BookShop.Models/Customer.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ICollection<Order>? Orders { get; set; }
    }
}
```

### BookShop.Models/Order.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
```

### BookShop.Models/OrderItem.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public Order? Order { get; set; }
        public Book? Book { get; set; }
    }
}
```

---

## TASK 3: CREATE DTOs

### BookShop.Models/BookDetailsDto.cs

```csharp
namespace BookShop.Models
{
    public class BookDetailsDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorBiography { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty;
        public string PublisherAddress { get; set; } = string.Empty;
        public string PublisherPhone { get; set; } = string.Empty;
    }
}
```

---

## TASK 4: INSTALL NUGET PACKAGES

```bash
cd BookShop.Services
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

cd ../BookShop
dotnet add package Microsoft.EntityFrameworkCore.Design
```

---

## TASK 5: CREATE DBCONTEXT

### BookShop.Services/AppDbContext.cs

```csharp
using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Book)
                .WithMany()
                .HasForeignKey(oi => oi.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
```

---

## TASK 6: CREATE REPOSITORIES

### BookShop.Services/IBookRepository.cs

```csharp
using BookShop.Models;

namespace BookShop.Services
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBook();
        Book? GetBookById(int id);
        BookDetailsDto? GetBookDetails(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
```

### BookShop.Services/SqlBookRepository.cs

```csharp
using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class SqlBookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public SqlBookRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBook()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .ToList();
        }

        public Book? GetBookById(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.BookId == id);
        }

        public BookDetailsDto? GetBookDetails(int id)
        {
            return _context.Books
                .Where(b => b.BookId == id)
                .Select(b => new BookDetailsDto
                {
                    BookId = b.BookId,
                    BookTitle = b.BookTitle,
                    BookDescription = b.BookDescription,
                    Price = b.Price,
                    Stock = b.Stock,
                    AuthorName = b.Author!.Name,
                    AuthorBiography = b.Author.Biography,
                    CategoryName = b.Category!.Name,
                    CategoryDescription = b.Category.Description,
                    PublisherName = b.Publisher!.Name,
                    PublisherAddress = b.Publisher.Address,
                    PublisherPhone = b.Publisher.Phone
                })
                .FirstOrDefault();
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}
```

### Create similar repositories for Author, Category, Publisher, Customer, Order

**IAuthorRepository.cs, SqlAuthorRepository.cs**
**ICategoryRepository.cs, SqlCategoryRepository.cs**
**IPublisherRepository.cs, SqlPublisherRepository.cs**
**ICustomerRepository.cs, SqlCustomerRepository.cs**
**IOrderRepository.cs, SqlOrderRepository.cs**

---

## TASK 7: CONFIGURE PROGRAM.CS

### BookShop/Program.cs

```csharp
using BookShop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookRepository, SqlBookRepository>();
builder.Services.AddScoped<IAuthorRepository, SqlAuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, SqlCategoryRepository>();
builder.Services.AddScoped<IPublisherRepository, SqlPublisherRepository>();
builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
```

### BookShop/appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=BookShopDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## TASK 8: CREATE DATABASE MIGRATION

```bash
cd BookShop
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## TASK 9: CREATE BOOK INDEX PAGE

### BookShop/Pages/Book/Index.cshtml.cs

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

        public IEnumerable<BookShop.Models.Book> Books { get; set; } = new List<BookShop.Models.Book>();

        public void OnGet()
        {
            Books = _bookRepository.GetAllBook();
        }
    }
}
```

### BookShop/Pages/Book/Index.cshtml

```html
@page
@model BookShop.Pages.Book.IndexModel

<h2>Books</h2>
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
```

---

## TASK 10: CREATE BOOK CREATE PAGE

### BookShop/Pages/Book/Create.cshtml.cs

```csharp
using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public BookShop.Models.Book Book { get; set; } = new BookShop.Models.Book();

        public IEnumerable<Author> Authors { get; set; } = new List<Author>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Publisher> Publishers { get; set; } = new List<Publisher>();

        public void OnGet()
        {
            Authors = _authorRepository.GetAllAuthor();
            Categories = _categoryRepository.GetAllCategory();
            Publishers = _publisherRepository.GetAllPublisher();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Authors = _authorRepository.GetAllAuthor();
                Categories = _categoryRepository.GetAllCategory();
                Publishers = _publisherRepository.GetAllPublisher();
                return Page();
            }

            _bookRepository.AddBook(Book);
            return RedirectToPage("./Index");
        }
    }
}
```

### BookShop/Pages/Book/Create.cshtml

```html
@page
@model BookShop.Pages.Book.CreateModel

<h2>Create Book</h2>

<form method="post">
    <div class="mb-3">
        <label asp-for="Book.BookTitle" class="form-label"></label>
        <input asp-for="Book.BookTitle" class="form-control" />
        <span asp-validation-for="Book.BookTitle" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Book.BookDescription" class="form-label"></label>
        <textarea asp-for="Book.BookDescription" class="form-control"></textarea>
    </div>

    <div class="mb-3">
        <label asp-for="Book.Price" class="form-label"></label>
        <input asp-for="Book.Price" class="form-control" />
        <span asp-validation-for="Book.Price" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Book.Stock" class="form-label"></label>
        <input asp-for="Book.Stock" class="form-control" />
    </div>

    <div class="mb-3">
        <label asp-for="Book.AuthorId" class="form-label">Author</label>
        <select asp-for="Book.AuthorId" class="form-control">
            <option value="">Select Author</option>
            @foreach (var author in Model.Authors)
            {
                <option value="@author.AuthorId">@author.Name</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label asp-for="Book.CategoryId" class="form-label">Category</label>
        <select asp-for="Book.CategoryId" class="form-control">
            <option value="">Select Category</option>
            @foreach (var category in Model.Categories)
            {
                <option value="@category.CategoryId">@category.Name</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label asp-for="Book.PublisherId" class="form-label">Publisher</label>
        <select asp-for="Book.PublisherId" class="form-control">
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
```

---

## TASK 11: CREATE BOOK EDIT PAGE

### BookShop/Pages/Book/Edit.cshtml.cs

```csharp
using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public BookShop.Models.Book Book { get; set; } = new BookShop.Models.Book();

        public IEnumerable<Author> Authors { get; set; } = new List<Author>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Publisher> Publishers { get; set; } = new List<Publisher>();

        public IActionResult OnGet(int id)
        {
            Book = _bookRepository.GetBookById(id);
            if (Book == null)
            {
                return NotFound();
            }

            Authors = _authorRepository.GetAllAuthor();
            Categories = _categoryRepository.GetAllCategory();
            Publishers = _publisherRepository.GetAllPublisher();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Authors = _authorRepository.GetAllAuthor();
                Categories = _categoryRepository.GetAllCategory();
                Publishers = _publisherRepository.GetAllPublisher();
                return Page();
            }

            _bookRepository.UpdateBook(Book);
            return RedirectToPage("./Index");
        }
    }
}
```

### BookShop/Pages/Book/Edit.cshtml

```html
@page
@model BookShop.Pages.Book.EditModel

<h2>Edit Book</h2>

<form method="post">
    <input type="hidden" asp-for="Book.BookId" />

    <div class="mb-3">
        <label asp-for="Book.BookTitle" class="form-label"></label>
        <input asp-for="Book.BookTitle" class="form-control" />
        <span asp-validation-for="Book.BookTitle" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Book.BookDescription" class="form-label"></label>
        <textarea asp-for="Book.BookDescription" class="form-control"></textarea>
    </div>

    <div class="mb-3">
        <label asp-for="Book.Price" class="form-label"></label>
        <input asp-for="Book.Price" class="form-control" />
        <span asp-validation-for="Book.Price" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Book.Stock" class="form-label"></label>
        <input asp-for="Book.Stock" class="form-control" />
    </div>

    <div class="mb-3">
        <label asp-for="Book.AuthorId" class="form-label">Author</label>
        <select asp-for="Book.AuthorId" class="form-control">
            @foreach (var author in Model.Authors)
            {
                <option value="@author.AuthorId">@author.Name</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label asp-for="Book.CategoryId" class="form-label">Category</label>
        <select asp-for="Book.CategoryId" class="form-control">
            @foreach (var category in Model.Categories)
            {
                <option value="@category.CategoryId">@category.Name</option>
            }
        </select>
    </div>

    <div class="mb-3">
        <label asp-for="Book.PublisherId" class="form-label">Publisher</label>
        <select asp-for="Book.PublisherId" class="form-control">
            @foreach (var publisher in Model.Publishers)
            {
                <option value="@publisher.PublisherId">@publisher.Name</option>
            }
        </select>
    </div>

    <button type="submit" class="btn btn-primary">Update</button>
    <a href="/Book/Index" class="btn btn-secondary">Cancel</a>
</form>
```

---

## TASK 12: CREATE BOOK DELETE PAGE

### BookShop/Pages/Book/Delete.cshtml.cs

```csharp
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.Book
{
    public class DeleteModel : PageModel
    {
        private readonly IBookRepository _bookRepository;

        public DeleteModel(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public BookShop.Models.Book Book { get; set; } = new BookShop.Models.Book();

        public IActionResult OnGet(int id)
        {
            Book = _bookRepository.GetBookById(id);
            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            _bookRepository.DeleteBook(id);
            return RedirectToPage("./Index");
        }
    }
}
```

### BookShop/Pages/Book/Delete.cshtml

```html
@page
@model BookShop.Pages.Book.DeleteModel

<h2>Delete Book</h2>

<div class="alert alert-danger">
    <h4>Are you sure you want to delete this book?</h4>
</div>

<dl class="row">
    <dt class="col-sm-3">Title</dt>
    <dd class="col-sm-9">@Model.Book.BookTitle</dd>

    <dt class="col-sm-3">Description</dt>
    <dd class="col-sm-9">@Model.Book.BookDescription</dd>

    <dt class="col-sm-3">Price</dt>
    <dd class="col-sm-9">@Model.Book.Price tk</dd>

    <dt class="col-sm-3">Stock</dt>
    <dd class="col-sm-9">@Model.Book.Stock</dd>

    <dt class="col-sm-3">Author</dt>
    <dd class="col-sm-9">@Model.Book.Author?.Name</dd>

    <dt class="col-sm-3">Category</dt>
    <dd class="col-sm-9">@Model.Book.Category?.Name</dd>

    <dt class="col-sm-3">Publisher</dt>
    <dd class="col-sm-9">@Model.Book.Publisher?.Name</dd>
</dl>

<form method="post">
    <input type="hidden" name="id" value="@Model.Book.BookId" />
    <button type="submit" class="btn btn-danger">Delete</button>
    <a href="/Book/Index" class="btn btn-secondary">Cancel</a>
</form>
```

---

## TASK 13: CREATE SIMILAR PAGES FOR OTHER ENTITIES

Create Index, Create, Edit, Delete pages for:
- Author
- Category
- Publisher
- Customer
- Order

Follow the same pattern as Book pages.

