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
        public static int ID = 0;
        public string Name { get; set; }

        public double Price { get; set; }

        public int Kolvo {  get; set; }

        public string Nasklade { get; set; }

        public string Kategor {  get; set; }

        public Kategorii Kat;


        public Tovar(string Name, double Price, int Kolvo, Kategorii Kat )
        {
            ID++;
            this.Name = Name;
            this.Price = Price;
            this.Kolvo = Kolvo;
            if(Kolvo >= 1 )
            {
                Nasklade = "Есть";
            }
            else
            {
                Nasklade = "Нету";
            }

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
        public void dobav()
        {
            
            Console.WriteLine("Введите название товара");
            string name = Console.ReadLine();
            Console.WriteLine("Введите цену(за шт)");
            double price = Convert.ToDouble(Console.ReadLine());
            if (price < 0)
            {
                Console.WriteLine("Введите положительноое значение!!!");
                return;
            }
            Console.WriteLine("Введите Количество товаров");
            int kolvo = Convert.ToInt32(Console.ReadLine());
            if (kolvo < 0)
            {
                Console.WriteLine("Введите положительноое значение!!!");
                return;
            }
            Console.WriteLine("Выберите категорию");
            Console.WriteLine("1. Электроника");
            Console.WriteLine("2. Еда");
            Console.WriteLine("3. Концелярия");
            int b = Convert.ToInt32(Console.ReadLine());

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

            }
            tovar.Add(new Tovar(name, price, kolvo, Kat));

            foreach (Tovar tovar1 in tovar)
            {
                tovar1.PrintInfo();
            }

        }

        

    }




    internal class Program
    {
        
      
        static void Main(string[] args)
        {
            Spisok spisok = new Spisok();
            while (true) {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку товара");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товара");
            int a = Convert.ToInt32(Console.ReadLine());
                switch (a)
                {
                    case 1:
                        spisok.dobav();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
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
