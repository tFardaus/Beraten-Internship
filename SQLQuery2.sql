CREATE PROCEDURE GetAllBooks
AS
BEGIN
    SELECT 
        b.BookId,
        b.BookTitle,
        b.BookDescription,
        b.Price,
        b.Stock,
        a.Name AS AuthorName,
        c.Name AS CategoryName,
        p.Name AS PublisherName
    FROM Books b
    LEFT JOIN Authors a ON b.AuthorId = a.AuthorId
    LEFT JOIN Categories c ON b.CategoryId = c.CategoryId
    LEFT JOIN Publishers p ON b.PublisherId = p.PublisherId
END