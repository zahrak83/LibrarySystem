using LibrarySystem.Entities;
using LibrarySystem.Enum;
using LibrarySystem.Extention;
using LibrarySystem.Infrastructure;
using LibrarySystem.Interface.Service;
using LibrarySystem.Services;

IAuthService auth = new AuthService();
IBookService bookService = new BookService();
IBorrowService borrowService = new BorrowService();
ICategoryService categoryService = new CategoryService();
IReviewService reviewService = new ReviewService();

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
        Console.WriteLine("5. Add Review to a Book");
        Console.WriteLine("6. View/Edit/Delete My Reviews");
        Console.WriteLine("7. Manage Wishlist");   
        Console.WriteLine("8. View My Penalty");  
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
            case "5":
                AddReview(user);
                break;
            case "6":
                ManageMyReviews(user);
                break;
            case "7":
                ManageWishlist(user);  
                break;
            case "8":
                ShowPenalty(user);      
                break;
            case "0":
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option. Please try again."); 
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
        Console.WriteLine("6. Manage Reviews");
        Console.WriteLine("7. View Users' Penalties");
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
            case "6":
                ManageReviews();
                break;
            case "7":
                ShowAllPenalties(admin);
                break;
            case "0":
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid choice! Try again.");
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
            ConsolePainter.WriteLine("   (No books in this category)", foreground: ConsoleColor.DarkGray);
            continue;
        }


        var wishlistService = new WishlistService();

        ConsolePainter.WriteTable(cat.Books.Select(b => new
        {
            b.Id,
            b.Title,
            b.Author,
            AverageRating = b.Reviews != null && b.Reviews.Any(r => r.IsApproved)
                ? Math.Round(b.Reviews.Where(r => r.IsApproved).Average(r => r.Rating), 2): 0,
            ReviewCount = b.Reviews?.Count(r => r.IsApproved) ?? 0,
            WishlistCount = wishlistService.GetWishlistByBook(b.Id).Count   
        }), headerColor: ConsoleColor.Yellow, rowColor: ConsoleColor.White);



        foreach (var book in cat.Books)
        {
            var approvedReviews = book.Reviews?.Where(r => r.IsApproved).ToList();
            if (approvedReviews != null && approvedReviews.Any())
            {
                ConsolePainter.WriteLine($"  Reviews for '{book.Title}':", foreground: ConsoleColor.Green);
                foreach (var review in approvedReviews)
                {
                    ConsolePainter.WriteLine($"    [{review.User.UserName}] Rating: {review.Rating}, Comment: {review.Comment}", foreground: ConsoleColor.Gray);
                }
            }
        }

        Console.WriteLine();
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

        
        var penalty = borrowService.ReturnBook(borrowedBookId); 

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Book returned successfully!");
        if (penalty > 0)
            Console.WriteLine($"Late return! Penalty applied: {penalty} Toman.");
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
void AddReview(User user)
{
    try
    {
        ShowCategoriesAndBooks();
        Console.Write("Enter Book ID to review: ");
        int bookId = int.Parse(Console.ReadLine());

        Console.Write("Enter comment (optional): ");
        string? comment = Console.ReadLine();

        int rating;
        do
        {
            Console.Write("Enter rating (1-5): ");
        } while (!int.TryParse(Console.ReadLine(), out rating) || rating < 1 || rating > 5);

        reviewService.AddReview(user.Id, bookId, comment, rating);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Review submitted successfully! Waiting for admin approval.");
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
void ManageMyReviews(User user)
{
    try
    {
        var reviews = reviewService.GetReviewsByUser(user.Id);
        if (!reviews.Any())
        {
            Console.WriteLine("You have no reviews.");
            Console.ReadKey();
            return;
        }

        ConsolePainter.WriteTable(reviews.Select(r => new
        {
            r.Id,
            BookTitle = r.Book.Title,
            r.Comment,
            r.Rating,
            Status = r.IsApproved ? "Approved" : "Pending"
        }), headerColor: ConsoleColor.Green, rowColor: ConsoleColor.White);

        Console.WriteLine("Enter Review ID to edit/delete (or 0 to cancel): ");
        int reviewId;
        while (!int.TryParse(Console.ReadLine(), out reviewId) || (reviewId != 0 && !reviews.Any(r => r.Id == reviewId)))
        {
            Console.Write("Invalid ID, enter again: ");
        }
        if (reviewId == 0) return;

        Console.WriteLine("1. Edit Review");
        Console.WriteLine("2. Delete Review");
        Console.Write("Choose an option: ");
        var action = Console.ReadLine();

        if (action == "1")
        {
            Console.Write("Enter new comment (optional): ");
            string? comment = Console.ReadLine();
            int rating;
            do
            {
                Console.Write("Enter new rating (1-5): ");
            } while (!int.TryParse(Console.ReadLine(), out rating) || rating < 1 || rating > 5);

            reviewService.EditReview(reviewId, comment, rating);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Review updated successfully!");
            Console.ResetColor();
        }
        else if (action == "2")
        {
            reviewService.DeleteReview(reviewId);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Review deleted successfully!");
            Console.ResetColor();
        }

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
void ManageReviews()
{
    var reviews = reviewService.GetAllReviews();
    if (!reviews.Any())
    {
        Console.WriteLine("(No reviews found)");
        Console.ReadKey();
        return;
    }

    ConsolePainter.WriteTable(reviews.Select(r => new
    {
        r.Id,
        User = r.User.UserName,
        Book = r.Book.Title,
        r.Comment,
        r.Rating,
        Status = r.IsApproved ? "Approved" : "Pending"
    }), headerColor: ConsoleColor.Cyan, rowColor: ConsoleColor.White);

    Console.Write("Enter Review ID to Approve/Reject (0 to cancel): ");
    int reviewId;
    while (!int.TryParse(Console.ReadLine(), out reviewId) || (reviewId != 0 && !reviews.Any(r => r.Id == reviewId)))
    {
        Console.Write("Invalid ID, enter again: ");
    }
    if (reviewId == 0) return;

    var review = reviews.First(r => r.Id == reviewId);

    Console.WriteLine("1. Approve Review");
    Console.WriteLine("2. Reject/Delete Review");
    Console.Write("Choose an option: ");
    var action = Console.ReadLine();

    if (action == "1")
    {
        reviewService.ApproveReview(reviewId);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Review approved!");
        Console.ResetColor();
    }
    else if (action == "2")
    {
        reviewService.DeleteReview(reviewId);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Review deleted!");
        Console.ResetColor();
    }

    Console.ReadKey();
}
void ManageWishlist(User user)
{
    var wishlistService = new WishlistService();
    Console.Clear();
    Console.WriteLine("1. Add to Wishlist");
    Console.WriteLine("2. View My Wishlist");
    Console.WriteLine("3. Remove from Wishlist");
    Console.Write("Choose: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ShowCategoriesAndBooks();
            Console.Write("Enter Book ID: ");
            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                wishlistService.AddToWishlist(user.Id, bookId);
                Console.WriteLine("Book added to Wishlist!");
            }
            else
            {
                Console.WriteLine("Invalid Book ID!");
            }
            break;

        case "2":
            var list = wishlistService.GetWishlistByUser(user.Id);
            if (list.Any())
                ConsolePainter.WriteTable(list.Select(w => new { w.Id, w.Book.Title, w.CreatedAt }));
            else
                Console.WriteLine("Your wishlist is empty.");
            break;

        case "3":
            var removeList = wishlistService.GetWishlistByUser(user.Id);
            if (!removeList.Any())
            {
                Console.WriteLine("Your wishlist is empty.");
                break;
            }

            ConsolePainter.WriteTable(removeList.Select(w => new { w.Id, w.Book.Title }));
            Console.Write("Enter Wishlist ID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int wid) && removeList.Any(w => w.Id == wid))
            {
                wishlistService.RemoveFromWishlist(wid);
                Console.WriteLine("Removed successfully!");
            }
            else
            {
                Console.WriteLine("Invalid Wishlist ID!");
            }
            break;

        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }

    Console.ReadKey();
}
void ShowPenalty(User user)
{
    Console.Clear();
    Console.WriteLine($"Your total penalty: {user.PenaltyAmount} Toman");
    Console.ReadKey();
}
void ShowAllPenalties(User admin)
{
    using var context = new AppDBContext();
    var users = context.Users
                       .Where(u => u.Id != admin.Id)
                       .Select(u => new { u.UserName, u.PenaltyAmount })
                       .ToList();

    Console.Clear();
    ConsolePainter.WriteLine("--- Users' Penalties ---", ConsoleColor.Yellow);
    ConsolePainter.WriteTable(users, headerColor: ConsoleColor.Cyan, rowColor: ConsoleColor.White);
    Console.WriteLine("\nPress any key to return...");
    Console.ReadKey();
}




