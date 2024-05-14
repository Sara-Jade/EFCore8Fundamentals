using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

//// If db not yet created or unknown
//using (PubContext context = new PubContext())
//{
//    context.Database.EnsureCreated();
//}

//this assumes you are working with the populated
//database created in previous module
PubContext _context = new();

//InsertAuthor();
//void InsertAuthor()
//{
//    var author = new Author { FirstName = "Julie", LastName = "Lerman" };
//    //using var context = new PubContext();
//    //context.Authors.Add(author);
//    //context.SaveChanges();
//    _context.Authors.Add(author);
//    _context.SaveChanges();

//    author = new Author { FirstName = "Hulu", LastName = "Lehrman" };
//    _context.Authors.Add(author);
//    _context.SaveChanges();
//}

//// This scenario gets and updates an author with change tracking.
//RetrieveAndUpdateAuthor();
//void RetrieveAndUpdateAuthor()
//{
//    var author = _context.Authors.FirstOrDefault(a => a.FirstName == "Julie" && a.LastName == "Lerman");
//    if (author is not null)
//    {
//        author.FirstName = "Julia";
//        _context.SaveChanges();
//    }

//    // Update multiple records. Need to SaveChanges() only once :-)
//    var lermanAuthors = _context.Authors.Where(a => a.LastName == "Lerman").ToList();
//    foreach (Author la in lermanAuthors)
//    {
//        la.LastName = "Jeepers";
//    }

//    // EF doesn't detect changes immediately. Usually SaveChanges() starts the detection process, though we can force detection.
//    Console.WriteLine($"Before changes detected:\r\n{_context.ChangeTracker.DebugView.ShortView}");
//    _context.ChangeTracker.DetectChanges();
//    Console.WriteLine($"After changes detected:\r\n{_context.ChangeTracker.DebugView.ShortView}");

//    _context.SaveChanges();
//}

//// Here's how a web app might get and update an author.
//// This set of methods does not use change tracking!
//UpdateAuthorWebAppSimulation(3);
//void UpdateAuthorWebAppSimulation(int authorId)
//{
//    Author author = GetAuthor(authorId);
//    if (author is not null )
//    {
//        author.FirstName = "Me";
//        UpdateAuthor(author);
//    }
//}

//void UpdateAuthor(Author author)
//{
//    using var shortLivedContext = new PubContext();
//    shortLivedContext.Authors.Update(author);
//    shortLivedContext.SaveChanges();
//}

//Author GetAuthor(int authorId)
//{
//    using var shortLivedContext = new PubContext();
//    return shortLivedContext.Authors.Find(authorId);
//}

// Let's delete an author.
DeleteAuthorById(1);
void DeleteAuthorById(int authorId)
{
    using var context = new PubContext();
    Author author = context.Authors.Find(authorId);
    if (author is not null )
    {
        context.Authors.Remove(author);
        Console.WriteLine($"change tracker longview: ${context.ChangeTracker.DebugView.LongView}");
        context.SaveChanges();
    }
}

//// Batch Add authors
//var authors = new Author[]
//{
//    new Author { FirstName = "Amy", LastName = "Alpha" },
//    new Author { FirstName = "Bambi", LastName = "Best" },
//    new Author { FirstName = "Carey", LastName = "Combs" },
//};
//var moreAuthors = new List<Author>
//{
//    new Author { FirstName = "Dora", LastName = "Dean" },
//    new Author { FirstName = "Eva", LastName = "Engineer" },
//    new Author { FirstName = "Florence", LastName = "Funkytown" },
//};
//BatchAddAuthors(moreAuthors);
//void BatchAddAuthors(IEnumerable<Author> authors)
//{
//    using var context = new PubContext();
//    context.Authors.AddRange(authors);
//    context.SaveChanges();
//}

// Don't mix change-tracked updates with non-changed-tracked update and delete shown below!

// Delete w/o using change tracking. Can bulk delete this way.
DeleteAuthorNoChangeTracking(33);
void DeleteAuthorNoChangeTracking(int authorId)
{
    using var context = new PubContext();

    // Executes immediately!
    // return value is how many rows were found and deleted, 0 if none found and deleted.
    int rowsDeleted = context.Authors.Where(a => a.Id == authorId).ExecuteDelete();
    rowsDeleted = context.Authors.Where(a => a.LastName.StartsWith("L")).ExecuteDelete();
}

// Update w/o using change tracking. Can bulk update this way.
UpdateAuthorsNoChangeTracking(8);
void UpdateAuthorsNoChangeTracking(int authorId)
{
    using var context = new PubContext();
    string newFirstName = "Sonya";

    // Executes immediately, but *** VS will not update the result in the debugger immediately ***
    // Return value is how many rows were updated. 0 if none found and updated.
    int rowsUpdated = context.Authors.Where(a => a.Id == authorId)
        .ExecuteUpdate(setters => setters.SetProperty(a => a.FirstName, newFirstName));

    //// Sometimes don't need a where in your query.
    //// Sometimes need a particular field in all records set.
    //// Once again, executes immediately, but VS doesn't update in debugger immediately.
    //rowsUpdated = context.Authors.ExecuteUpdate(s => s.SetProperty(a => a.LastName, a => a.LastName.ToLower()));

    // Change last names back to "title" case
    rowsUpdated = context.Authors
        .ExecuteUpdate(s => s.SetProperty(a => a.LastName, a => a.LastName.Substring(0, 1).ToUpper() + a.LastName.Substring(1)));
}