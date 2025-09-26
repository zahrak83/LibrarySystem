using LibrarySystem.Entities;
using LibrarySystem.Enum;
using LibrarySystem.Extention;
using LibrarySystem.Interface.Service;
using LibrarySystem.Services;

IAuthService auth = new AuthService();
IBookService bookService = new BookService();
IBorrowService borrowService = new BorrowService();
ICategoryService categoryService = new CategoryService();

while (true)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("=== Library System ===");
    Console.ResetColor();
    Console.WriteLine("1. Register");
    Console.WriteLine("2. Login");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Register();
            break;
        case "2":
            Login();
            break;
        case "0":
            return;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid option");
            Console.ResetColor();
            Console.ReadKey();
            break;
    }
}

void Register()
{
    try
    {
        Console.Clear();
        Console.WriteLine("--- Register ---");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        Console.WriteLine("Role(Admin = 0, User = 1): ");
        int roleChoice;
        while (!int.TryParse(Console.ReadLine(), out roleChoice) || (roleChoice != 0 && roleChoice != 1))
        {
            Console.Write("Invalid input. Enter 0 for Admin or 1 for User: ");
        }

        RoleEnum role = roleChoice == 0 ? RoleEnum.Admin : RoleEnum.User;

        var user = auth.Register(username, password, role);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"User '{user.UserName}' registered successfully!");
        Console.ResetColor();
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
        Console.ReadKey();
    }
}

void Login()
{
    try
    {
        Console.Clear();
        Console.WriteLine("--- Login ---");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        var user = auth.Login(username, password);
        if (user == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid username or password.");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        if (user.Role == RoleEnum.Admin)
            AdminMenu(user);
        else
            UserMenu(user);
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
        Console.ReadKey();
    }
}
void UserMenu(User user)
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"--- User Menu ({user.UserName}) ---");
        Console.ResetColor();
        Console.WriteLine("1. View Categories and Books");
        Console.WriteLine("2. Borrow a Book");
        Console.WriteLine("3. View My Borrowed Books");
        Console.WriteLine("4. Return a Book");
        Console.WriteLine("0. Logout");
        Console.Write("Choose an option: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ShowCategoriesAndBooks();
                break;
            case "2":
                BorrowBook(user);
                break;
            case "3":
                ShowMyBorrowedBooks(user);
                break;
            case "4":
                ReturnBook(user);
                break;
            case "0":
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.ResetColor();
                Console.ReadKey();
                break;
        }
    }
}
void AdminMenu(User admin)
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"--- Admin Menu ({admin.UserName}) ---");
        Console.ResetColor();
        Console.WriteLine("1. View All Users");
        Console.WriteLine("2. Add Category");
        Console.WriteLine("3. Add Book");
        Console.WriteLine("4. View Categories and Books");
        Console.WriteLine("5. View Borrowed Books by Users");
        Console.WriteLine("0. Logout");
        Console.Write("Choose an option: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ShowAllUsers();
                break;
            case "2":
                AddCategory();
                break;
            case "3":
                AddBook();
                break;
            case "4":
                ShowCategoriesAndBooks();
                break;
            case "5":
                ShowAllBorrowedBooks();
                break;
            case "0":
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.ResetColor();
                Console.ReadKey();
                break;
        }
    }
}
void ShowCategoriesAndBooks()
{
    Console.Clear();
    var categories = categoryService.GetAllCategories();
    if (!categories.Any())
    {
        Console.WriteLine("(No categories found)");
        Console.ReadKey();
        return;
    }
    foreach (var cat in categories)
    {
        ConsolePainter.WriteLine($"Category [{cat.Id}]: {cat.Name}", foreground: ConsoleColor.Cyan);
        if (cat.Books == null || !cat.Books.Any())
        {
            ConsolePainter.WriteLine(" (No books in this category)", foreground: ConsoleColor.DarkGray);
            continue;
        }
        ConsolePainter.WriteTable(cat.Books.Select(b => new { b.Id, b.Title, b.Author, }), headerColor: ConsoleColor.Yellow, rowColor: ConsoleColor.White);
    }
    Console.ReadKey();
}
void BorrowBook(User user)
{
    try
    {
        ShowCategoriesAndBooks();
        Console.Write("Enter Book ID to borrow: ");
        int bookId = int.Parse(Console.ReadLine());

        borrowService.BorrowBook(user.Id, bookId);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Book borrowed successfully!");
        Console.ResetColor();
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
        Console.ReadKey();
    }
}
void ShowMyBorrowedBooks(User user)
{
    Console.Clear();
    var borrowed = borrowService.GetBorrowedBooksByUser(user.Id);
    if (!borrowed.Any())
    {
        Console.WriteLine("You have no borrowed books.");
    }
    else
    {
        ConsolePainter.WriteTable(borrowed.Select(bb => new
        {
            bb.Id,
            bb.Book.Title,
            bb.Book.Author,
            Category = bb.Book.Category?.Name ?? "Unknown",
            bb.BorrowTime
        }), headerColor: ConsoleColor.Green, rowColor: ConsoleColor.White);
    }
    Console.ReadKey();
}
void ReturnBook(User user)
{
    try
    {
        var borrowed = borrowService.GetBorrowedBooksByUser(user.Id);
        if (!borrowed.Any())
        {
            Console.WriteLine("You have no borrowed books to return.");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        
        ConsolePainter.WriteTable(borrowed.Select(bb => new
        {
            bb.Id,
            bb.Book.Title,
            bb.Book.Author,
            Category = bb.Book.Category?.Name ?? "Unknown",
            bb.BorrowTime
        }), headerColor: ConsoleColor.Green, rowColor: ConsoleColor.White);

        Console.Write("Enter BorrowedBook ID to return: ");
        int borrowedBookId;
        while (!int.TryParse(Console.ReadLine(), out borrowedBookId) || !borrowed.Any(bb => bb.Id == borrowedBookId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid ID. Enter a valid BorrowedBook ID: ");
            Console.ResetColor();
        }

        borrowService.ReturnBook(borrowedBookId);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Book returned successfully!");
        Console.ResetColor();
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
        Console.ReadKey();
    }
}
void ShowAllUsers()
{
    Console.Clear();
    var users = ((AuthService)auth).GetAllUsers();
    if (!users.Any())
    {
        Console.WriteLine("(No users found)");
    }
    else
    {
        ConsolePainter.WriteTable(users.Select(u => new
        {
            u.Id,
            u.UserName,
            Role = u.Role.ToString()
        }), headerColor: ConsoleColor.Magenta, rowColor: ConsoleColor.White);
    }
    Console.ReadKey();
}
void AddCategory()
{
    try
    {
        Console.Clear();
        string name;
        do
        {
            Console.Write("Enter Category Name: ");
            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Category name cannot be empty.");
                Console.ResetColor();
            }
        } while (string.IsNullOrWhiteSpace(name));

        categoryService.AddCategory(name);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Category added successfully!");
        Console.ResetColor();
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
        Console.ReadKey();
    }
}
void AddBook()
{
    try
    {
        ShowCategoriesAndBooks();

        string title;
        do
        {
            Console.Write("Enter Book Title: ");
            title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Book title cannot be empty.");
                Console.ResetColor();
            }
        } while (string.IsNullOrWhiteSpace(title));

        string author;
        do
        {
            Console.Write("Enter Book Author: ");
            author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Author cannot be empty.");
                Console.ResetColor();
            }
        } while (string.IsNullOrWhiteSpace(author));

        int catId;
        var categories = categoryService.GetAllCategories();
        do
        {
            Console.Write("Enter Category ID: ");
        } while (!int.TryParse(Console.ReadLine(), out catId) || !categories.Any(c => c.Id == catId));

        bookService.AddBook(title, author, catId);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Book added successfully!");
        Console.ResetColor();
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ResetColor();
        Console.ReadKey();
    }
}
void ShowAllBorrowedBooks()
{
    Console.Clear();
    var borrowed = borrowService.GetAllBorrowedBooks();
    if (!borrowed.Any())
    {
        Console.WriteLine("(No borrowed books)");
    }
    else
    {
        ConsolePainter.WriteTable(borrowed.Select(bb => new
        {
            User = bb.User.UserName,
            BookId = bb.Book.Id,
            Title = bb.Book.Title,
            Category = bb.Book.Category?.Name ?? "Unknown",
            bb.BorrowTime
        }), headerColor: ConsoleColor.Cyan, rowColor: ConsoleColor.White);
    }
    Console.ReadKey();
}

