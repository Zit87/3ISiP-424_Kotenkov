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
            if (a < 2 || a > 40)
            {
                Console.WriteLine("Количество операций должно быть от 2 до 40!");
                return;
            }

            for (int i = 0; i < a; i++)
                {
                    Console.WriteLine("Введите в формате (Название услуги или товара; Количество денег)");
                    string d = Console.ReadLine();
                    string[] chisla = d.Split(new char[] { ';' });
                    double   b = Convert.ToDouble(chisla[1]);
                    string c = Convert.ToString(chisla[0]);
                    oper.Add(c, b);

                }
            while (true)
            {
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
                        double summa = oper.Values.Sum();
                        double srednee = oper.Values.Average();
                        double maksimum = oper.Values.Max();
                        double minimum = oper.Values.Min();

                        Console.WriteLine($"Сумма: {summa}");
                        Console.WriteLine($"Среднее: {srednee}");
                        Console.WriteLine($"Максимум: {maksimum}");
                        Console.WriteLine($"Минимум: {minimum}");

                        break;
                    case 3:
                        var sort = oper.ToList();

                        for (int i = 0; i < sort.Count - 1; i++)
                        {
                            for (int j = 0; j < sort.Count - i - 1; j++)
                            {
                                if (sort[j].Value > sort[j + 1].Value)
                                {
                                    var temp = sort[j];
                                    sort[j] = sort[j + 1];
                                    sort[j + 1] = temp;
                                }
                            }
                        }
                        Console.WriteLine("Сортировкапо цене:");
                        foreach (var d in sort)
                        {
                            Console.WriteLine($"(Название услуги или товара : {d.Key} Количество денег: {d.Value})");
                        }


                        break;
                    case 4:
                        Console.WriteLine("Выберите валлюту:");
                        Console.WriteLine("1. Доллары");
                        Console.WriteLine("2. Евро");
                        Console.WriteLine("3. Ввести свой курс");
                        int valuta = Convert.ToInt32(Console.ReadLine());
                        double kurs = 0;
                        switch (valuta)
                        {
                            case 1:
                                kurs = 80;
                                break;
                            case 2:
                                kurs = 95;
                                break;
                            case 3:
                                Console.WriteLine("Введите курс рубля к валюте:");
                                kurs = Convert.ToDouble(Console.ReadLine());
                                break;

                            default:
                                Console.WriteLine("Неверный выбор валюты");
                                break;


                        }
                        if (kurs > 0)
                        {
                            Console.WriteLine("Конвертированные значения:");
                            foreach (var d in oper)
                            {
                                double konvert = d.Value / kurs;
                                Console.WriteLine($"(Название услуги или товара : {d.Key} Количество денег: {konvert})");
                            }
                        }
                        break;
                    case 5:
                        Console.WriteLine("Введите название услуги или товара для поиска:");
                        string poisk = Console.ReadLine();
                        bool naydeno = false;

                        foreach (var d in oper)
                        {
                            if (d.Key.ToLower().Contains(poisk.ToLower()))
                            {
                                Console.WriteLine($"(Название услуги или товара : {d.Key} Количество денег: {d.Value})");
                                naydeno = true;
                            }
                        }
                        if (!naydeno)
                        {
                            Console.WriteLine("Услуга или товар не найден");
                        }
                        break;
                    case 0:
                        Console.WriteLine("Выход из программы");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор операции");
                        break;
                }



            }


        }


        static void Main(string[] args)
        {
            operac();



        }
    }
}
