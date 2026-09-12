using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _3ISiP_424_Kotenkov
{
    public enum Kategorii
    {
        Электроника,
        Еда,
        Концелярия
    }



    class Tovar
    {
        private static int nextID = 0;
        public  int ID ;
        public string Name { get; set; }

        public double Price { get; set; }

        public int Kolvo {  get; set; }

        public string Nasklade
        {
            get
            {
                if (Kolvo >= 1)
                {
                    return "Есть";
                }
                else
                {
                    return "Нету";
                }
            }
        }

        public string Kategor {  get; set; }

        public Kategorii Kat;



        public Tovar(string Name, double Price, int Kolvo, Kategorii Kat )
        {
            ID = ++nextID;
            this.Name = Name;
            this.Price = Price;
            this.Kolvo = Kolvo;
            this.Kat = Kat;
            

            if (Kategorii.Концелярия ==Kat)
            {
                Kategor = "Концелярия";
            }else if (Kategorii.Электроника == Kat){
                Kategor = "Электроника";
            }
            else if (Kategorii.Еда ==Kat){
                Kategor = "Еда";
            }
            
        }



        public void PrintInfo()
        {
            Console.WriteLine($"ID товара {ID} название {Name} цена {Price} кол-во {Kolvo} наличие на складе {Nasklade} категория {Kategor}");
        }


    }

    class Spisok
    {
        public Kategorii Kat;
        List<Tovar> tovar = new List<Tovar>(); 
       
       public Spisok()
        {
           
            tovar.Add(new Tovar("пельмени", 100, 10, Kategorii.Еда));
            tovar.Add(new Tovar("ручка", 50, 30, Kategorii.Концелярия));
            tovar.Add(new Tovar("карандаш", 30, 123, Kategorii.Концелярия));
            tovar.Add(new Tovar("ноут", 100000, 5, Kategorii.Электроника));
            tovar.Add(new Tovar("мышка", 1500, 67, Kategorii.Электроника));
        }

        public void dobav()
        {
            
            Console.WriteLine("Введите название товара");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Без названия нельзя!!!");
                return;
            }
            Console.WriteLine("Введите цену(за шт)");
            if (!double.TryParse(Console.ReadLine(), out double price))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }
            if (price < 0 )
            {
                Console.WriteLine("Введите положительноое значение!!!");
                return;
            }
            Console.WriteLine("Введите Количество товаров");
            if (!int.TryParse(Console.ReadLine(), out int kolvo))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }
            if (kolvo < 0)
            {
                Console.WriteLine("Введите положительноое значение!!!");
                return;
            }
            Console.WriteLine("Выберите категорию");
            Console.WriteLine("1. Электроника");
            Console.WriteLine("2. Еда");
            Console.WriteLine("3. Концелярия");
            if(!int.TryParse(Console.ReadLine(), out int b))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }

            
            switch (b)
            {
                case 1:
                    Kat = Kategorii.Электроника;
                    break;
                case 2:
                    Kat = Kategorii.Еда;
                    break;
                case 3:
                    Kat = Kategorii.Концелярия;
                    break;
                default:
                    Console.WriteLine("Неверная категория!");
                    return;
            }
            tovar.Add(new Tovar(name, price, kolvo, Kat));

            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }

        }


        public void ydal()
        {
            Console.WriteLine("Введите какой товар хотите удалить:");
            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }
            if (!int.TryParse(Console.ReadLine(), out int n))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }
            if(n < 1 || n > tovar.Count)
            {
                Console.WriteLine("Ошибка! Введите корректное число.");
                return;
            }
            tovar.RemoveAt(n - 1);
            Console.WriteLine("Удалено!");
            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }
        }

        public void post()
        {
            Console.WriteLine("Введите куда хотите поставить товар:");
            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }
            if (!int.TryParse(Console.ReadLine(), out int n))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }
            if(n < 1 || n > tovar.Count)
            {
                Console.WriteLine("Ошибка! Введите корректное число.");
                return;
            }
            Console.WriteLine("Введите сколько поставить:");
            if (!int.TryParse(Console.ReadLine(), out int m))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }

            if(m<= 0)
            {
                Console.WriteLine("Ошибка! Введите положительное число.");
                return;
            }
            tovar[n - 1].Kolvo += m;

            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }
        }

        public void prod()
        {
            Console.WriteLine("Введите что хотите продать:");
            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }
            if (!int.TryParse(Console.ReadLine(), out int n))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }
            if(n < 1 || n > tovar.Count)
            {
                Console.WriteLine("Ошибка! Введите корректное число.");
                return;
            }
            Console.WriteLine("Введите сколько продать:");
            if (!int.TryParse(Console.ReadLine(), out int m))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }
            if(m <= 0)
            {
                Console.WriteLine("Ошибка! Введите положительное число.");
                return;
            }
            if (tovar[n - 1].Kolvo >= m)
            {
                Console.WriteLine("Товар продан");
                tovar[n - 1].Kolvo -= m;

            }
            else
            {
                Console.WriteLine("Недостаточно товара на складе!");
            }

            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }
        }

        public void vivod()
        {
            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }
        }

        public void poisk()
        {
            Console.WriteLine("Выберите по чему искать:");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По категории");
            Console.WriteLine("3. По Id");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Ошибка! Введите число.");
                return;
            }
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Введите название товара для поиска:");
                    string name = Console.ReadLine();
                    bool found1 = false;

                    foreach (Tovar tovar1 in tovar)
                    {
                        if (tovar1.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Ваш товар:");
                            tovar1.PrintInfo();
                            found1 = true;
                        }
                    }

                    if (!found1)
                    {
                        Console.WriteLine("Товар не найден!");
                    }
            
                    break;
                case 2:
                    Console.WriteLine("Выберите категорию");
                    Console.WriteLine("1. Электроника");
                    Console.WriteLine("2. Еда");
                    Console.WriteLine("3. Концелярия");
                    if (!int.TryParse(Console.ReadLine(), out int b))
                    {
                        Console.WriteLine("Ошибка! Введите число.");
                        return;
                    }
                    Kategorii category;
                    switch (b)
                    {
                        case 1:
                            category = Kategorii.Электроника;
                            break;
                        case 2:
                            category = Kategorii.Еда;
                            break;
                        case 3:
                            category = Kategorii.Концелярия;
                            break;
                        default:
                            Console.WriteLine("Неверная категория.");
                            return;
                    }

                    bool found = false;

                    foreach (Tovar tovar1 in tovar)
                    {
                        if (tovar1.Kat == category)
                        {
                            Console.WriteLine("Ваш товар:");
                            tovar1.PrintInfo();

                            found = true;
                        }
                    }

                    if (!found)
                    {
                        Console.WriteLine("Товар не найден!");
                    }

                    break;

                case 3:
                    Console.WriteLine("Введите Id товара для поиска:");

                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("Ошибка! Введите число.");
                        return;
                    }

                    bool foundId = false;

                    foreach (Tovar tovar1 in tovar)
                    {
                        if (tovar1.ID == id)
                        {
                            Console.WriteLine("Ваш товар:");
                            tovar1.PrintInfo();
                            foundId = true;
                            break;
                        }
                    }

                    if (!foundId)
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




    internal class Program
    {
        
      
        static void Main(string[] args)
        {
            
            Spisok spisok = new Spisok();

            while(true) {
            
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку товара");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товара");
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
                        spisok.post();
                        break;
                    case 4:
                        spisok.prod();
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
}
