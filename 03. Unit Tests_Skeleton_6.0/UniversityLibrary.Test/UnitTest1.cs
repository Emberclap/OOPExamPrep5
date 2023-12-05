namespace UniversityLibrary.Test
{
    using NUnit.Framework;
    using System.Collections.Immutable;
    using System.Linq;

    [TestFixture]
    public class Tests
    {
        private UniversityLibrary library;
        private TextBook textBook;
        [SetUp]
        public void Setup()
        {
            this.library = new UniversityLibrary();
        }

        [Test]
        public void ConstructorShould_InitializeProperly()
        {
            UniversityLibrary library = new UniversityLibrary();
            Assert.IsNotNull(library);
            Assert.That(library.Catalogue.Count == 0);
            
        }
        [Test]
        public void AddTextBookToLibrary_ShouldWorkProperly()
        {
            string title = "Title";
            string author = "Author";
            string category = "Category";
            int expectedCount = 2;
            TextBook textBook = new TextBook(title, author, category);
            TextBook textBook2 = new TextBook("Title2", "Author2", "Category2");
            Assert.That(library.AddTextBookToLibrary(textBook), Is.EqualTo(textBook.ToString()));
            //library.AddTextBookToLibrary(textBook);
            library.AddTextBookToLibrary(textBook2);
            Assert.AreEqual(expectedCount, library.Catalogue.Count);
            Assert.That(1 == textBook.InventoryNumber);
            Assert.That(2 == textBook2.InventoryNumber);

        }
        [Test]
        public void AddTextBookToLibrary_ShouldIncreseBookCount()
        {
            string title = "Title";
            string author = "Author";
            string category = "Category";
            TextBook textBook = new TextBook(title, author, category);
            library.AddTextBookToLibrary(textBook);
            library.AddTextBookToLibrary(textBook);

            int expectedCountBefore = 2;
            int expectedCountAfter = 3;
            Assert.AreEqual(expectedCountBefore, library.Catalogue.Count);
            library.AddTextBookToLibrary(textBook);
            Assert.That(expectedCountAfter == library.Catalogue.Count);

        }
        [Test]
        public void LoanNewTextBook_ShouldWorkCorrectly()
        {
            int bookInventoryNumber = 1;
            string studentName = "Student1";
            string title = "Title";
            string author = "Author";
            string category = "Category";
            TextBook textBook = new TextBook(title, author, category);
            TextBook textBook2 = new TextBook("Title2", "Author2", "Category2");
            library.AddTextBookToLibrary(textBook);
            library.AddTextBookToLibrary(textBook2);
            library.LoanTextBook(bookInventoryNumber+1, studentName);
            Assert.That($"{textBook.Title} loaned to {studentName}." 
                == library.LoanTextBook(bookInventoryNumber, studentName));
            Assert.That($"{studentName} still hasn't returned {textBook.Title}!"
                == library.LoanTextBook(bookInventoryNumber, studentName));
            Assert.That(textBook.Holder == "Student1");
            Assert.That(textBook2.Holder == "Student1");

        }
        [Test]
        public void ReturningTextBookToLibrary()
        {
            int bookInventoryNumber = 1;
            string title = "Title";
            string author = "Author";
            string category = "Category";
            TextBook textBook = new TextBook(title, author, category);
            TextBook textBook2 = new TextBook("Title2", "Author2", "Category2");
            TextBook textBook3 = new TextBook("Title3", "Author3", "Category3");
            TextBook textBook4 = new TextBook("Title4", "Author4", "Category4");

            library.AddTextBookToLibrary(textBook);
            library.AddTextBookToLibrary(textBook2);
            library.AddTextBookToLibrary(textBook3);
            library.AddTextBookToLibrary(textBook4);
            library.LoanTextBook(bookInventoryNumber, "Student1");
            library.LoanTextBook(bookInventoryNumber+1, "Student2");
           

            Assert.That($"{textBook.Title} is returned to the library." == library.ReturnTextBook(bookInventoryNumber));
            Assert.That($"{textBook2.Title} is returned to the library." == library.ReturnTextBook(bookInventoryNumber + 1));
            TextBook expectedTextBook = library.Catalogue.FirstOrDefault(x=>x.InventoryNumber == bookInventoryNumber);
            Assert.That(textBook == library.Catalogue.FirstOrDefault(x => x.InventoryNumber == bookInventoryNumber));
            Assert.That(textBook2 == library.Catalogue.FirstOrDefault(x=> x.InventoryNumber == bookInventoryNumber+1));
        }
        [Test]
        public void ReturningTextBookToLibrary_ShouldSetHolder ()
        {
            int bookInventoryNumber = 1;
            string title = "Title";
            string author = "Author";
            string category = "Category";
            TextBook textBook = new TextBook(title, author, category);
            TextBook textBook2 = new TextBook("Title2", "Author2", "Category2");
            TextBook textBook3 = new TextBook("Title3", "Author3", "Category3");
            TextBook textBook4 = new TextBook("Title4", "Author4", "Category4");

            library.AddTextBookToLibrary(textBook);
            library.AddTextBookToLibrary(textBook2);
            library.AddTextBookToLibrary(textBook3);
            library.AddTextBookToLibrary(textBook4);
            library.LoanTextBook(bookInventoryNumber, "Student1");
            library.LoanTextBook(bookInventoryNumber + 1, "Student2");

            Assert.That($"{textBook.Title} is returned to the library." == library.ReturnTextBook(bookInventoryNumber));
            Assert.That($"{textBook2.Title} is returned to the library." == library.ReturnTextBook(bookInventoryNumber + 1));
            Assert.That(textBook == library.Catalogue.FirstOrDefault(x => x.InventoryNumber == bookInventoryNumber));
            Assert.That(textBook2 == library.Catalogue.FirstOrDefault(x => x.InventoryNumber == bookInventoryNumber + 1));
            Assert.That(textBook.Holder == string.Empty);
            Assert.That(textBook2.Holder == string.Empty);

        }
    }
    
}