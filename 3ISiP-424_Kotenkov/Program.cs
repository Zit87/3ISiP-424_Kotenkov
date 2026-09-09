using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _3ISiP_424_Kotenkov
{
    internal class Program
    {

        static void operac()
        {
            Dictionary<string, double> oper = new Dictionary<string, double>();
            
                Console.WriteLine("Введите кол-во операций:");
                int a = Convert.ToInt32(Console.ReadLine());
                if (a < 2)
                {
                    Console.WriteLine("Только больше 2 операций!");
                }
                else if (a > 40)
                {
                    Console.WriteLine("Только не больше 40 операций!");
                }
               
                for (int i = 0; i < a; i++)
                {
                    Console.WriteLine("Введите в формате (Название услуги или товара; Количество денег)");
                    string d = Console.ReadLine();
                    string[] chisla = d.Split(new char[] { ';' });
                    int b = Convert.ToInt32(chisla[1]);
                    string c = Convert.ToString(chisla[0]);
                    oper.Add(c, b);

                }
                Console.WriteLine("1. Вывод данных");
                Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
                Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
                Console.WriteLine("4. Конвертация валюты (пользователь вводит курс или выбирает из списка)");
                Console.WriteLine("5. Поиск по названию ");
                Console.WriteLine("0. Выход");
            int g = Convert.ToInt32(Console.ReadLine());
            switch (g)
            {
                case 1:
                    foreach (var d in oper)
                    {
                        Console.WriteLine($"(Название услуги или товара : {d.Key} Количество денег: {d.Value})");
                    }
                    break;
                case 2:
                    
                    Console.WriteLine("Среднее:");
                    var valya = new List<double>();
                    
                        foreach (var d in oper)
                        {
                            

                            valya.Add( d.Value);
                            
                        }
                    
                    foreach(var r in valya)
                    {
                        Console.WriteLine(r);
                    }

                    break;




                   

            }



               
                
            
        }


        static void Main(string[] args)
        {
            operac();



        }
    }
}
