# BookShop Project - Complete Implementation Guide

## Table of Contents
1. Project Architecture Overview
2. Step-by-Step Implementation
3. Key Concepts and Logic
4. Testing Guide

---

## 1. PROJECT ARCHITECTURE OVERVIEW

### Architecture Pattern: 3-Layer Architecture

```
BookShop.Models (Data Layer)
    ↓
BookShop.Services (Business/Data Access Layer)
    ↓
BookShop (Presentation Layer)
```

**Why 3-Layer?**
- **Separation of Concerns**: Each layer has a specific responsibility
- **Maintainability**: Changes in one layer don't affect others
- **Testability**: Each layer can be tested independently
- **Scalability**: Easy to add new features

---

## 2. STEP-BY-STEP IMPLEMENTATION

### PHASE 1: PROJECT SETUP

#### Step 1.1: Create Solution Structure

```bash
# Create solution
dotnet new sln -n BookShop

# Create projects
dotnet new classlib -n BookShop.Models
dotnet new classlib -n BookShop.Services
dotnet new webapp -n BookShop

# Add projects to solution
dotnet sln add BookShop.Models/BookShop.Models.csproj
dotnet sln add BookShop.Services/BookShop.Services.csproj
dotnet sln add BookShop/BookShop.csproj

# Add project references
cd BookShop.Services
dotnet add reference ../BookShop.Models/BookShop.Models.csproj

cd ../BookShop
dotnet add reference ../BookShop.Models/BookShop.Models.csproj
dotnet add reference ../BookShop.Services/BookShop.Services.csproj
```

**Logic**: Separate projects for better organization and dependency management.

---

### PHASE 2: DATA LAYER (BookShop.Models)

#### Step 2.1: Create Entity Models

**File: BookShop.Models/Book.cs**

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

        // Foreign Keys
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }

        // Navigation Properties
        public Author? Author { get; set; }
        public Category? Category { get; set; }
        public Publisher? Publisher { get; set; }
    }
}
```

**Logic**:
- `[Key]`: Marks primary key
- `[Required]`: Enforces NOT NULL constraint
- Foreign Keys: `int` properties link to related entities
- Navigation Properties: `?` nullable for lazy loading

**Repeat for**: Author, Category, Publisher, Customer, Order, OrderItem

---

#### Step 2.2: Create DTO (Data Transfer Object)

**File: BookShop.Models/BookDetailsDto.cs**

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
        
        // Flattened related data
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

**Why DTO?**
- Avoids circular reference issues in JSON serialization
- Flattens complex object graphs for display
- Controls exactly what data is exposed to UI

---

### PHASE 3: DATA ACCESS LAYER (BookShop.Services)

#### Step 3.1: Install NuGet Packages

```bash
cd BookShop.Services
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**Why these packages?**
- `SqlServer`: Database provider for SQL Server
- `Tools`: Enables migrations and database updates

---

#### Step 3.2: Create DbContext

**File: BookShop.Services/AppDbContext.cs**

```csharp
using BookShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
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
            base.OnModelCreating(modelBuilder); // CRITICAL for Identity

            // Configure relationships
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
        }
    }
}
```

**Logic**:
- Inherits `IdentityDbContext<ApplicationUser>` for authentication
- `DbSet<T>`: Represents table in database
- `OnModelCreating`: Configures relationships
- `DeleteBehavior.Restrict`: Prevents cascade deletes (data integrity)
- `base.OnModelCreating()`: MUST call for Identity tables

---

#### Step 3.3: Create Repository Pattern

**File: BookShop.Services/IBookRepository.cs**

```csharp
using BookShop.Models;

namespace BookShop.Services
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBook();
        IEnumerable<Book> SearchBooks(string searchTerm);
        Book? GetBookById(int id);
        BookDetailsDto? GetBookDetails(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
```

**Why Interface?**
- Abstraction: Hide implementation details
- Dependency Injection: Easy to swap implementations
- Testing: Can mock for unit tests

---

**File: BookShop.Services/SqlBookRepository.cs**

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

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Where(b => b.BookTitle.Contains(searchTerm))
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

**Key LINQ Patterns**:
- `.Include()`: Eager loading (loads related entities)
- `.Where()`: Filtering
- `.Select()`: Projection (mapping to DTO)
- `.FirstOrDefault()`: Returns first match or null
- `.ToList()`: Executes query and returns list

**Repeat for**: Author, Category, Publisher, Customer, Order repositories

---

### PHASE 4: PRESENTATION LAYER (BookShop)

#### Step 4.1: Configure Services in Program.cs

**File: BookShop/Program.cs**

```csharp
using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages and Controllers
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToFolder("/Account");
});
builder.Services.AddControllers();

// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Configure Identity
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

// Register Repositories (Dependency Injection)
builder.Services.AddScoped<IBookRepository, SqlBookRepository>();
builder.Services.AddScoped<IAuthorRepository, SqlAuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, SqlCategoryRepository>();
builder.Services.AddScoped<IPublisherRepository, SqlPublisherRepository>();
builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();

var app = builder.Build();

// Configure Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  // WHO are you?
app.UseAuthorization();   // WHAT can you do?
app.MapRazorPages();
app.MapControllers();

app.Run();
```

**Logic**:
- `AddRazorPages`: Enables Razor Pages
- `AuthorizeFolder("/")`: All pages require login
- `AllowAnonymousToFolder("/Account")`: Except login/register
- `AddDbContext`: Registers database context
- `AddIdentity`: Configures authentication
- `AddScoped`: Registers repositories (one instance per request)
- Middleware order matters: Routing → Authentication → Authorization

---

