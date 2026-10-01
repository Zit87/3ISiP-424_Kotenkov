using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
enum Genr
{
    Fiction,
    Science,
    History,
    Fantasy,
    Detective
}

class Book
{
    private static int nextID = 0;
    public int ID;
    public string Title;
    public string Author;
    public Genr Genr;
    public int Year;
    public decimal Price;

    public string genr;
    public Book(string Title, string Author, decimal Price, int Year, Genr Genr)
    {
        ID = ++nextID;
        this.Title = Title;
        this.Author = Author;
        this.Price = Price;
        this.Year = Year;
        this.Genr = Genr;


        if (Genr.Fiction == Genr)
        {
            genr = "Художественная";
        }
        else if (Genr.Science == Genr)
        {
            genr = "Наука";
        }
        else if (Genr.History == Genr)
        {
            genr = "История";
        }
        else if (Genr.Fantasy == Genr)
        {
            genr = "Фантастика";
        }
        else if (Genr.Detective == Genr)
        {
            genr = "Детектив";
        } 
    }
        public void PrintInfo()
    {
        Console.WriteLine($"ID книги {ID} название {Title} автор {Author} цена {Price} год изданиия {Year} жанр {genr} ");
    }
}

class Spisok
{
    public Genr Genr;
    List<Book> book = new List<Book>();

    public Spisok()
    {

        book.Add(new Book("Киев","Зеля",67 ,2026 , Genr.Fantasy));
        book.Add(new Book("Конь", "Емеля", 52 , 1999, Genr.Fiction));
        book.Add(new Book("Преступление и Наказание", "Достоевский", 300.2323m, 1865, Genr.Fantasy));
        book.Add(new Book("Приключения Шерлока Холмса", "Конан Дойл", 3443.5656m, 1888, Genr.Detective));
        book.Add(new Book("Это Спарта! Законы легендарного государства", "Плутарх", 899, 1000, Genr.History));
    }

    public void dobav()
    {

        Console.WriteLine("Введите название книги");
        string title = Console.ReadLine();
        if (string.IsNullOrEmpty(title))
        {
            Console.WriteLine("Без названия нельзя!!!");
            return;
        }
        Console.WriteLine("Введите автора книги");
        string author = Console.ReadLine();
        if (string.IsNullOrEmpty(author))
        {
            Console.WriteLine("Без автора нельзя!!!");
            return;
        }
        Console.WriteLine("Введите цену");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Ошибка! Введите число.");
            return;
        }
        if (price < 0)
        {
            Console.WriteLine("Введите положительноое значение!!!");
            return;
        }
        Console.WriteLine("Введите год издания");
        if (!int.TryParse(Console.ReadLine(), out int god))
        {
            Console.WriteLine("Ошибка! Введите число.");
            return;
        }
        if (god < 0)
        {
            Console.WriteLine("Введите положительноое значение!!!");
            return;
        }
        Console.WriteLine("Выберите жанр");
        Console.WriteLine("1. Художественная");
        Console.WriteLine("2. Наука");
        Console.WriteLine("3. История");
        Console.WriteLine("4. Фантастика");
        Console.WriteLine("5. Детектив");
        if (!int.TryParse(Console.ReadLine(), out int b))
        {
            Console.WriteLine("Ошибка! Введите число.");
            return;
        }


        switch (b)
        {
            case 1:
                Genr = Genr.Fiction;
                break;
            case 2:
                Genr = Genr.Science;
                break;
            case 3:
                Genr = Genr.History;
                break;
            case 4:
                Genr = Genr.Fantasy;
                break;
            case 5:
                Genr = Genr.Detective;
                break;
            default:
                Console.WriteLine("Неверная категория!");
                return;
        }
        book.Add(new Book(title,author, price, god, Genr));

        foreach (Book book1 in book)
        {
            book1.PrintInfo();
        }

    }


    public void ydal()
    {
        Console.WriteLine("Введите какой товар хотите удалить:");
        foreach (Book book1 in book)
        {
            book1.PrintInfo();
        }
        if (!int.TryParse(Console.ReadLine(), out int n))
        {
            Console.WriteLine("Ошибка! Введите число.");
            return;
        }
        if (n < 1 || n > book.Count)
        {
            Console.WriteLine("Ошибка! Введите корректное число.");
            return;
        }
        book.RemoveAt(n - 1);
        Console.WriteLine("Удалено!");
        foreach (Book book1 in book)
        {
            book1.PrintInfo();
        }
    }

    

   

    public void vivod()
    {
        foreach (Book book1 in book)
        {
            book1.PrintInfo();
        }
    }

    public void poisk()
    {
        Console.WriteLine("Выберите по чему искать:");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По жанру");
        Console.WriteLine("3. По автору");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Ошибка! Введите число.");
            return;
        }
        switch (choice)
        {
            case 1:
                Console.WriteLine("Введите название книги для поиска:");
                string title = Console.ReadLine();
                bool found1 = false;

                foreach (Book book1 in book)
                {
                    if (book1.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Ваша книга:");
                        book1.PrintInfo();
                        found1 = true;
                    }
                }

                if (!found1)
                {
                    Console.WriteLine("Товар не найден!");
                }

                break;
            case 2:
                Console.WriteLine("Выберите жанр");
                Console.WriteLine("1. Художественная");
                Console.WriteLine("2. Наука");
                Console.WriteLine("3. История");
                Console.WriteLine("4. Фантастика");
                Console.WriteLine("5. Детектив");
                if (!int.TryParse(Console.ReadLine(), out int b))
                {
                    Console.WriteLine("Ошибка! Введите число.");
                    return;
                }
                Genr genr;
                switch (b)
                {
                    case 1:
                        genr = Genr.Fiction;
                        break;
                    case 2:
                        genr = Genr.Science;
                        break;
                    case 3:
                        genr = Genr.History;
                        break;
                    case 4:
                        genr = Genr.Fantasy;
                        break;
                    case 5:
                        genr = Genr.Detective;
                        break;
                    default:
                        Console.WriteLine("Неверная категория.");
                        return;
                }

                bool found = false;

                foreach (Book book1 in book)
                {
                    if (book1.Genr == genr)
                    {
                        Console.WriteLine("Ваш товар:");
                        book1.PrintInfo();

                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Товар не найден!");
                }

                break;

            case 3:
                Console.WriteLine("Введите автора книги для поиска:");
                string title1 = Console.ReadLine();
                bool found2 = false;

                foreach (Book book1 in book)
                {
                    if (book1.Title.Equals(title1, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Ваша книга:");
                        book1.PrintInfo();
                        found2 = true;
                    }
                }

                if (!found2)
                {
                    Console.WriteLine("Товар не найден!");
                }

                

                break;
            default:
                Console.WriteLine("Неверный выбор.");
                break;
        }
    }

}

class Program
{

    static void Main()
    {
        Spisok spisok = new Spisok();

        while (true)
        {

            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Удалить книгу");
            Console.WriteLine("3. Отсортировать книги");
            Console.WriteLine("4. Сгруппировать книги");
            Console.WriteLine("5. Поиск книги");
            Console.WriteLine("6. Вывод");
            Console.WriteLine("0. Выход");

            if (!int.TryParse(Console.ReadLine(), out int a))
            {
                Console.WriteLine("Ошибка! Введите число.");
                continue;
            }
            switch (a)
            {
                case 1:
                    spisok.dobav();
                    break;
                case 2:
                    spisok.ydal();
                    break;
                case 3:
                    
                    break;
                case 4:
                    
                    break;
                case 5:
                    spisok.poisk();
                    break;
                case 6:
                    spisok.vivod();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Такого выбора нет!!!");
                    break;

            }
        }
    }


    

    
}