# BookShop Relational Database Schema

## Database Tables (7 Tables)

### 1. Authors
- AuthorId (PK)
- Name
- Biography
- Books (Navigation: One-to-Many)

### 2. Categories
- CategoryId (PK)
- Name
- Description
- Books (Navigation: One-to-Many)

### 3. Publishers
- PublisherId (PK)
- Name
- Address
- Phone
- Books (Navigation: One-to-Many)

### 4. Books
- BookId (PK)
- BookTitle
- BookDescription
- Price
- Stock
- AuthorId (FK)
- CategoryId (FK)
- PublisherId (FK)
- Author (Navigation)
- Category (Navigation)
- Publisher (Navigation)
- OrderItems (Navigation: One-to-Many)

### 5. Customers
- CustomerId (PK)
- Name
- Email
- Phone
- Address
- Orders (Navigation: One-to-Many)

### 6. Orders
- OrderId (PK)
- CustomerId (FK)
- OrderDate
- TotalAmount
- Status
- Customer (Navigation)
- OrderItems (Navigation: One-to-Many)

### 7. OrderItems (Junction Table)
- OrderItemId (PK)
- OrderId (FK)
- BookId (FK)
- Quantity
- Price
- Order (Navigation)
- Book (Navigation)

## Relationships

```
Author (1) ----< Books (Many)
Category (1) ----< Books (Many)
Publisher (1) ----< Books (Many)
Customer (1) ----< Orders (Many)
Order (1) ----< OrderItems (Many) >---- Books (Many)
```

## LINQ Examples Used

### Method Syntax (Lambda):
```csharp
// Simple query
_context.Authors.ToList()

// With navigation properties
_context.Authors.Include(a => a.Books).ToList()

// Filtering
_context.Books.Where(b => b.Price > 20).ToList()

// Single item
_context.Authors.FirstOrDefault(a => a.AuthorId == id)

// Nested includes
_context.Orders
    .Include(o => o.Customer)
    .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Book)
    .ToList()
```

## API Endpoints

### Authors
- GET /api/authors - Get all authors
- POST /api/authors - Create author
- PUT /api/authors/{id} - Update author
- DELETE /api/authors/{id} - Delete author

### Categories
- GET /api/categories
- POST /api/categories
- PUT /api/categories/{id}
- DELETE /api/categories/{id}

### Publishers
- GET /api/publishers
- POST /api/publishers
- PUT /api/publishers/{id}
- DELETE /api/publishers/{id}

### Books
- GET /api/books
- POST /api/books
- PUT /api/books/{id}
- DELETE /api/books/{id}

### Customers
- GET /api/customers
- POST /api/customers
- PUT /api/customers/{id}
- DELETE /api/customers/{id}

### Orders
- GET /api/orders
- POST /api/orders
- PUT /api/orders/{id}
- DELETE /api/orders/{id}

## Next Steps

1. **Create Migration:**
   ```bash
   Add-Migration AddRelationalTables
   Update-Database
   ```

2. **Create Razor Pages for each entity:**
   - Pages/Author/ (Index, Create, Edit, Delete, Detail)
   - Pages/Category/ (Index, Create, Edit, Delete, Detail)
   - Pages/Publisher/ (Index, Create, Edit, Delete, Detail)
   - Pages/Customer/ (Index, Create, Edit, Delete, Detail)
   - Pages/Order/ (Index, Create, Edit, Delete, Detail)

3. **Add AJAX to all pages** (same pattern as Book)

4. **Update existing Book pages** to use dropdowns for Author, Category, Publisher selection

## Architecture

```
Models Layer (BookShop.Models)
    ↓
Services Layer (BookShop.Services)
    - Repositories (LINQ queries)
    - DbContext
    ↓
Presentation Layer (BookShop)
    - API Controllers (AJAX endpoints)
    - Razor Pages (UI)
```

All CRUD operations use LINQ with Entity Framework Core!
