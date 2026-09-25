using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
class TextStats {
    public string Text; 
    public int Kolvoslov; 
    public string SamoeKor;
    public string SamoeDln; 
    public int KolvoPred; 
    public int GlBukvi; 
    public int SoglBukvi; 
    public Dictionary<char, int> Chastota;
}
class Program
{
    static List<TextStats> history = new List<TextStats>();
    static string GlNis = "аеёиоуыэюя";
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Введите текст (не менее 100 символов):");
            string text = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Текст не может быть пустым.");
                continue;
            }

            if (text.Length < 100)
            {
                Console.WriteLine("Текст слишком короткий. Пожалуйста, введите текст длиной не менее 100 символов.");
                continue;
            }

            TextStats stats = AnalyzeText(text);
            history.Add(stats);

            PrintStats(stats);

           
            Console.WriteLine("Продолжить работу с новым текстом? (1 - да / 2 - нет)");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Некорректный ввод. Завершение программы.");
                break;
            }

            switch (choice)
            {
                case 1:
                    continue;

                case 2:
                    Console.WriteLine("Показать статистику по прошлым текстам? (1 - да / 2 - нет)");

                    if (int.TryParse(Console.ReadLine(), out int historyChoice))
                    {
                        if (historyChoice == 1)
                        {
                            ShowHistory();
                        }
                        else if (historyChoice != 2)
                        {
                            Console.WriteLine("Некорректный выбор.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ввод.");
                    }

                    return;

                default:
                    Console.WriteLine("Некорректный ввод. Завершение программы.");
                    return;
            }
        }

    }



    static TextStats AnalyzeText(string text)
    {
        TextStats stats = new TextStats();
        stats.Text = text;

       
        MatchCollection matches = Regex.Matches(text, @"[а-яА-ЯёЁa-zA-Z]+");
        List<string> words = new List<string>();
        foreach (Match m in matches)
            words.Add(m.Value);

        stats.Kolvoslov = words.Count;

        if (words.Count > 0)
        {
            string shortest = words[0];
            string longest = words[0];

            for (int i = 1; i < words.Count; i++)
            {
                if (words[i].Length < shortest.Length)
                    shortest = words[i];
                if (words[i].Length > longest.Length)
                    longest = words[i];
            }

            stats.SamoeKor = shortest;
            stats.SamoeDln = longest;
        }
        else
        {
            stats.SamoeKor = "";
            stats.SamoeDln = "";
        }

      
        stats.KolvoPred = Regex.Matches(text, @"[.!?]+").Count;

   
        int GL = 0;
        int Sogl = 0;
        Dictionary<char, int> skebob = new Dictionary<char, int>();

        string lowerText = text.ToLower();
        for (int i = 0; i < lowerText.Length; i++)
        {
            char c = lowerText[i];
            if (char.IsLetter(c))
            {
                if (GlNis.IndexOf(c) >= 0)
                    GL++;
                else
                    Sogl++;

                if (skebob.ContainsKey(c))
                    skebob[c]++;
                else
                    skebob[c] = 1;
            }
        }

        stats.GlBukvi = GL;
        stats.SoglBukvi = Sogl;
        stats.Chastota = skebob;

        return stats;
    }

    static void PrintStats(TextStats s)
    {
        Console.WriteLine("--- Статистика текста ---");
        Console.WriteLine("Количество слов: " + s.Kolvoslov);
        Console.WriteLine("Самое короткое слово: " + s.SamoeKor);
        Console.WriteLine("Самое длинное слово: " + s.SamoeDln);
        Console.WriteLine("Количество предложений: " + s.KolvoPred);
        Console.WriteLine("Гласных букв: " + s.GlBukvi);
        Console.WriteLine("Согласных букв: " + s.SoglBukvi);
        Console.WriteLine("Частота букв:");

        List<char> letters = new List<char>();

        foreach (char letter in s.Chastota.Keys)
        {
            letters.Add(letter);
        }

        for (int i = 0; i < letters.Count - 1; i++)
        {
            for (int j = 0; j < letters.Count - i - 1; j++)
            {
                if (s.Chastota[letters[j]] < s.Chastota[letters[j + 1]])
                {
                    char temp = letters[j];
                    letters[j] = letters[j + 1];
                    letters[j + 1] = temp;
                }
            }
        }

        foreach (char letter in letters)
        {
            Console.WriteLine("  '" + letter + "': " + s.Chastota[letter]);
        }
    }

    static void ShowHistory()
    {
        Console.WriteLine("=== История всех текстов ===");
       
        for (int i = 0; i < history.Count; i++)
        {
            Console.WriteLine("Текст №" + (i + 1) + ":");
        
            Console.WriteLine(history[i].Text);
            PrintStats(history[i]);
        }
    }
}