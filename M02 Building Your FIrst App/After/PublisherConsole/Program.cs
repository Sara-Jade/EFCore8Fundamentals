using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

// Assumes db has already been created
using PubContext _context = new();
QueryFilters();
void QueryFilters()
{
    // using LINQ.
    // Note: EF will always parameterize the query, even if the variable comes from user input :-)
    // string firstName = "Josie";
    //var authors = _context.Authors.Where(a => a.FirstName == firstName).ToList();

    // Can use EF functions for SQL-like syntax
    //var nameFilter = "L%";
    //var authors = _context.Authors.Where(a => EF.Functions.Like(a.LastName, nameFilter)).ToList();

    // Find retrieves an entity by its key in the DbSet, so not LINQ, uses a Top 1 query.
    // EF core won't go to the db if it's already in memory.

    // Skip() and Take() are LINQ methods that support paging.
    // If group has 0 or fewer members than the Take size, no errors.
    //int groupSize = 2;
    //for (int i = 0; i < 5; ++i)
    //{
    //    var authors = _context.Authors.Skip(groupSize * i).Take(groupSize).ToList();
    //    Console.WriteLine($"Group {i}:");
    //    foreach (Author author in authors)
    //    {
    //        Console.WriteLine($"{author.FirstName} {author.LastName}");
    //    }
    //}

    // OrderBy(), ThenBy(), OrderByDescending(), ThenByDescending()
    SortAuthors();
    void SortAuthors()
    {
        // LINQ uses only the last OrderBy()!
        var authors = _context.Authors.OrderBy(a => a.FirstName).OrderBy(a => a.LastName).ToList();
        foreach (var author in authors) { Console.WriteLine($"{author.FirstName} {author.LastName}"); }

        // Use OrderBy(), ThenBy() to get expected results
        authors = _context.Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();
        foreach (var author in authors) { Console.WriteLine($"{author.FirstName} {author.LastName}"); }
    }
}

// Don't need this now that DB has been created.
//using (PubContext context=new PubContext())
//{
//    context.Database.EnsureCreated();
//}

//GetAuthors();
// AddAuthor();
// GetAuthors();
//AddAuthorWithBook();
//GetAuthorsWithBooks();

void AddAuthorWithBook()
{
    var author = new Author { FirstName = "Julie", LastName = "Lerman" };
    author.Books.Add(new Book { Title = "Programming Entity Framework",
                                PublishDate = new DateOnly(2009, 1, 1)
    });
    author.Books.Add(new Book { Title = "Programming Entity Framework 2nd Ed",
                                PublishDate = new DateOnly(2010,8,1) });
    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
}
void GetAuthorsWithBooks()
{
    using var context = new PubContext();
    var authors = context.Authors.Include(a => a.Books).ToList();
    foreach (var author in authors)
    {
        Console.WriteLine(author.FirstName + " " + author.LastName);
        foreach (var book in author.Books)
        {
            Console.WriteLine(book.Title);
        }
    }
}

void AddAuthor()
{
    var author = new Author { FirstName = "Josie", LastName = "Newf" };
    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthors() 
{
    using var context = new PubContext();
    // Trigger EF Core to execute a query.
    var authors = context.Authors.ToList();

    // Didn't ask for author's book list specifically, so no books will show for any author.
    foreach (var author in authors)
    {
        Console.WriteLine(author.FirstName + " " + author.LastName);
    }
}