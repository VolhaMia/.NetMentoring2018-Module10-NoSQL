using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            BookDAO bookDAO = new BookDAO();

            Console.WriteLine("Add Books:");
            bookDAO.AddBooks();
            bookDAO.FindAll();

            Console.WriteLine("\nFind 3 Books With Count More Than One:");
            bookDAO.FindBooksWithCountMoreThanOne();

            Console.WriteLine("\nFind Books With Max and Min Count:");
            bookDAO.FindBooksWithMaxMinCount();

            Console.WriteLine("\nFind All Authors:");
            bookDAO.FindAllAuthors();

            Console.WriteLine("\nGet Books without Authors:");
            bookDAO.GetBooksWithoutAuthors();

            Console.WriteLine("\nIncrease count of each book by one:");
            bookDAO.IncreaseCountOfEachBookByOne();

            Console.WriteLine("\nAdd Genre Favority To Fantasy Book:");
            bookDAO.AddGenreFavorityToFantasyBook();

            Console.WriteLine("\nDelete books with Count less than 3:");
            bookDAO.DeleteBooksWithCountLessThanThree();

            Console.WriteLine("\nDelete all books:");
            bookDAO.DeleteAllBooks();
        }
    }
}
