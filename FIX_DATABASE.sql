-- Run this script in SQL Server Management Studio or Azure Data Studio
-- This will clean up existing data and prepare for the new schema

USE Books;
GO

-- Delete all existing books (they don't have proper relationships yet)
DELETE FROM Books;
GO

-- Now run the migration from command line:
-- cd "d:\ASP.net\BookShop Project\BookShop.Services"
-- dotnet ef migrations add AddRelationalTables --startup-project ../BookShop
-- dotnet ef database update --startup-project ../BookShop
