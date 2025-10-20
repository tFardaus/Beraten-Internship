# Guide: Creating Razor Pages for Remaining Entities

## ✅ Completed:
- **Author** - Full CRUD with AJAX (Index, Create, Edit, Delete)
- **Category** - Index and Create pages started

## 📋 To Create:

### 1. Category (Complete Edit & Delete)
### 2. Publisher (All pages)
### 3. Customer (All pages)
### 4. Order (All pages)

## Template Pattern (Copy & Replace)

For each entity, create 4 pages in `Pages/{EntityName}/`:

### Index.cshtml
```cshtml
@page
@model BookShop.Pages.{Entity}.IndexModel
@{ ViewData["Title"] = "{Entities}"; }

<h1>{Entities}</h1>
<p><a asp-page="Create" class="btn btn-primary">Create New</a></p>
<table class="table">
    <thead>
        <tr>
            <th>{Property1}</th>
            <th>{Property2}</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model.{Entities})
        {
            <tr>
                <td>@item.{Property1}</td>
                <td>@item.{Property2}</td>
                <td>
                    <a asp-page="Edit" asp-route-id="@item.{Entity}Id" class="btn btn-sm btn-warning">Edit</a>
                    <a asp-page="Delete" asp-route-id="@item.{Entity}Id" class="btn btn-sm btn-danger">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

### Index.cshtml.cs
```csharp
using BookShop.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.{Entity}
{
    public class IndexModel : PageModel
    {
        private readonly I{Entity}Repository _repository;

        public IndexModel(I{Entity}Repository repository)
        {
            _repository = repository;
        }

        public List<Models.{Entity}> {Entities} { get; set; } = new();

        public void OnGet()
        {
            {Entities} = _repository.GetAll{Entities}().ToList();
        }
    }
}
```

### Create.cshtml (with AJAX)
```cshtml
@page
@model BookShop.Pages.{Entity}.CreateModel
@{ ViewData["Title"] = "Create {Entity}"; }

<h1>Create {Entity}</h1>
<hr />
<div class="row">
    <div class="col-md-4">
        <div id="message" class="alert" style="display:none;"></div>
        <form id="create{Entity}Form" method="post">
            <!-- Add form fields for each property -->
            <div class="form-group">
                <label asp-for="{Entity}.{Property}" class="control-label"></label>
                <input asp-for="{Entity}.{Property}" class="form-control" />
            </div>
            <div class="form-group mt-3">
                <input type="submit" value="Create" class="btn btn-primary" />
                <a asp-page="Index" class="btn btn-secondary">Back</a>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    <script>
        $(document).ready(function() {
            $('#create{Entity}Form').on('submit', function(e) {
                e.preventDefault();
                $.ajax({
                    url: '/api/{entities}',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        {property1}: $('#{Entity}_{Property1}').val(),
                        {property2}: $('#{Entity}_{Property2}').val()
                    }),
                    success: function(response) {
                        $('#message').removeClass('alert-danger').addClass('alert-success')
                            .text(response.message).show();
                        $('#create{Entity}Form')[0].reset();
                    },
                    error: function() {
                        $('#message').removeClass('alert-success').addClass('alert-danger')
                            .text('Error creating {entity}.').show();
                    }
                });
            });
        });
    </script>
}
```

### Create.cshtml.cs
```csharp
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookShop.Pages.{Entity}
{
    public class CreateModel : PageModel
    {
        private readonly I{Entity}Repository _repository;

        public CreateModel(I{Entity}Repository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Models.{Entity} {Entity} { get; set; } = new();

        public IActionResult OnGet() => Page();

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            _repository.Add{Entity}({Entity});
            return RedirectToPage("./Index");
        }
    }
}
```

## Quick Reference: Entity Properties

### Publisher
- PublisherId (PK)
- Name (required)
- Address
- Phone

### Customer
- CustomerId (PK)
- Name (required)
- Email (required)
- Phone
- Address

### Order
- OrderId (PK)
- CustomerId (FK) - **Use dropdown**
- OrderDate
- TotalAmount
- Status

## Steps to Complete:

1. **Copy Author pages** as template
2. **Find & Replace:**
   - `Author` → `{NewEntity}`
   - `author` → `{newEntity}`
   - `Authors` → `{NewEntities}`
   - Update properties in forms
3. **Test each page** after creation

## Migration Command:

Before testing, run:
```bash
cd BookShop.Services
dotnet ef migrations add AddRelationalTables --startup-project ../BookShop
dotnet ef database update --startup-project ../BookShop
```

## Navigation Links

Add to `_Layout.cshtml`:
```html
<li class="nav-item">
    <a class="nav-link" asp-page="/Author/Index">Authors</a>
</li>
<li class="nav-item">
    <a class="nav-link" asp-page="/Category/Index">Categories</a>
</li>
<li class="nav-item">
    <a class="nav-link" asp-page="/Publisher/Index">Publishers</a>
</li>
<li class="nav-item">
    <a class="nav-link" asp-page="/Customer/Index">Customers</a>
</li>
<li class="nav-item">
    <a class="nav-link" asp-page="/Order/Index">Orders</a>
</li>
```

All pages follow the same pattern - just copy, replace entity names, and update properties!
