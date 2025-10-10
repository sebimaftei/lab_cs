using System.ComponentModel.DataAnnotations;

public class MainClass
{
    //Book record
    public record Book(string Title, string Author, int YearPublished);
    //Borrower record
    record Borrower(int Id, string Name, List<Book> BorrowedBooks);
    //obj is an object parameter
    public void typeMatch(object obj)
    {
        //checks if obj is a Book
        if (obj is Book book)
        {
            Console.WriteLine("{}, {}", book.Title, book.YearPublished.ToString());
        }
        // checks if obj is a Borrower
        else if (obj is Borrower borrower)
        {
            Console.WriteLine("{}, {}", borrower.Name, borrower.BorrowedBooks.Count.ToString());
        }
        else Console.WriteLine("Unknown Type");
    }
    
    //Librarian class that can be initialized only at the beginning because of init
    public class Librarian
    {
        string Name { get; init; }
        string Email { get; init; }
        string LibrarySection { get; init; }

        public Librarian(string name, string email, string librarySection)
        {
            this.Name = name;
            this.Email = email;
            this.LibrarySection = librarySection;
        }
    }
    
    public List<Book> books = new List<Book>();
    
    //adds book by title
    public void AddBook(string title)
    {
        books.Add(new Book(title, "John", 2011));
    }
    
    //prints all books
    public void PrintBooks()
    {
        foreach (var book in books)
        {
            Console.WriteLine(book.Title);
        }
    }
    
    
    public static void Main()
    {
        MainClass mainClass = new MainClass();
        //creates new Borrower and book
        var b1 = new Borrower (1, "John", new List<Book>());
        var book1 = new Book("a", "a", 2004);
        //clones a Borrower and adds the book
        var b2 = b1 with
        {
            BorrowedBooks = new List<Book> { book1 }
        };
        
        var l1 = new Librarian("Ion", "ion@gmail.com", "fiction");
        
        //adds books with the AddBook function
        mainClass.AddBook("a");
        mainClass.AddBook("b");
        mainClass.AddBook("c");
        mainClass.books.Add(new Book("d", "d", 2004));
        mainClass.books.Add(new Book("e", "e", 2005));
        
        var pr = (params IEnumerable<Book> b) =>
        {
            foreach (var var in b)
            {
                if (var.YearPublished >= 2010) 
                    Console.WriteLine(var.Title);
            }

            return 0;
        };
        var c = pr(mainClass.books);
    }
}
