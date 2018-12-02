using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NoSQL
{
    public class BookDAO
    {
        private MongoClient mongoClient;
        private IMongoCollection<Book> bookCollection;

        public BookDAO()
        {
            mongoClient = new MongoClient("mongodb://localhost");
            var database = mongoClient.GetDatabase("book");
            bookCollection = database.GetCollection<Book>("Book");
        }

        public void FindAll()
        {
            var books = bookCollection.AsQueryable<Book>().ToList();
            if (books.Count == 0)
            {
                Console.WriteLine("Book collection is empty");
            }
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        //Добавьте следующие книги(название, автор, количество экземпляров, жанр, год издания):
        //Hobbit, Tolkien, 5, fantasy, 2014
        //Lord of the rings, Tolkien, 3, fantasy, 2015
        //Kolobok, 10, kids, 2000
        //Repka, 11, kids, 2000
        //Dyadya Stiopa, Mihalkov, 1, kids, 2001
        public void AddBooks()
        {
            List<Book> books = new List<Book> {
                { new Book{
                    Name = "Hobbit",
                    Author = "Tolkien",
                    Count = 5,
                    Genre = new List<string> { "fantasy" },
                    Year = 2014
                } },
                { new Book {
                    Name = "Lord of the rings",
                    Author = "Tolkien",
                    Count = 3,
                    Genre = new List<string> { "fantasy" },
                    Year = 2015
                } },
                { new Book {
                    Name = "Kolobok",
                    Count = 10,
                    Genre = new List<string> { "kids" },
                    Year = 2000
                } },
                { new Book {
                    Name = "Repka",
                    Count = 11,
                    Genre = new List<string> { "kids" },
                    Year = 2000
                } },
                { new Book {
                    Name = "Dyadya Stiopa",
                    Author = "Mihalkov",
                    Count = 1,
                    Genre = new List<string> { "kids" },
                    Year = 2001
                } }
            };

            bookCollection.InsertMany(books);
        }

        //Найдите книги с количеством экземпляров больше единицы.
        //a.Покажите в результате только название книги.
        //b.Отсортируйте книги по названию.
        //c.Ограничьте количество возвращаемых книг тремя.
        //d.Подсчитайте количество таких книг.
        public void FindBooksWithCountMoreThanOne()
        {
            var books = bookCollection.AsQueryable<Book>().Where(b => b.Count > 1).OrderBy(b => b.Name).Take(3).Select(b => b.Name).ToList();
            foreach (var book in books)
            {
                Console.WriteLine("Name:" + book + "\n");
            }
            Console.WriteLine("Count:" + books.Count);
        }

        //Найдите книгу с макимальным/минимальным количеством (count).
        public void FindBooksWithMaxMinCount()
        {
            var max = bookCollection.AsQueryable<Book>().Max(b => b.Count);
            var min = bookCollection.AsQueryable<Book>().Min(b => b.Count);

            var bookWithMaxCount = bookCollection.AsQueryable<Book>().Where(b => b.Count == max);
            var bookWithMinCount = bookCollection.AsQueryable<Book>().Where(b => b.Count == min);

            Console.WriteLine("Max Count:" + max);
            Console.WriteLine("Book with Max Count:");
            foreach (var book in bookWithMaxCount)
            {
                Console.WriteLine(book);
            }

            Console.WriteLine("Min Count:" + min);
            Console.WriteLine("Book with Min Count:");
            foreach (var book in bookWithMinCount)
            {
                Console.WriteLine(book);
            }
        }

        //Найдите список авторов (каждый автор должен быть в списке один раз).
        public void FindAllAuthors()
        {
            var authors = bookCollection.AsQueryable<Book>().Where(b => !string.IsNullOrEmpty(b.Author)).Select(b => b.Author).Distinct();

            Console.WriteLine("List of authors:");
            foreach (var author in authors)
            {
                Console.WriteLine("Name:" + author + "\n");
            }
        }

        //Выберите книги без авторов.
        public void GetBooksWithoutAuthors()
        {
            var books = bookCollection.AsQueryable<Book>().Where(b => string.IsNullOrEmpty(b.Author));

            Console.WriteLine("List of books without author:");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        //Увеличьте количество экземпляров каждой книги на единицу.
        public void IncreaseCountOfEachBookByOne()
        {
            bookCollection.UpdateMany(Builders<Book>.Filter.Empty, Builders<Book>.Update.Inc(b => b.Count, 1));
            FindAll();
        }

        //Добавьте дополнительный жанр “favority” всем книгам с жанром “fantasy” 
        //(последующие запуски запроса не должны дублировать жанр “favority”).
        public void AddGenreFavorityToFantasyBook()
        {
            bookCollection.UpdateMany(b => b.Genre.Contains("fantasy") && !b.Genre.Contains("favority"),
                new JsonUpdateDefinition<Book>("{$addToSet: {Genre: \"favority\"} }"));
            FindAll();
        }

        //Удалите книги с количеством экземпляров меньше трех.
        public void DeleteBooksWithCountLessThanThree()
        {
            bookCollection.DeleteMany<Book>(b => b.Count < 3);
            FindAll();
        }

        //Удалите все книги.
        public void DeleteAllBooks()
        {
            bookCollection.DeleteMany("{}");
            Console.WriteLine("All books have been deleted");
            FindAll();
        }
    }
}
