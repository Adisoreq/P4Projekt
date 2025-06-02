using Projekt.Services;
using Projekt.Data;
using Projekt.Models;
using System;

namespace ProjektTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var dbContext = AppService.DbContext)
            {
                int pollId = 1;
                var categories = PollService.Instance.getPollCategories(pollId);

                // Wyświetl kategorie
                Console.WriteLine($"Kategorie dla ankiety o ID {pollId}:");
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"- {category.Name}");
                    }
                    Console.WriteLine("Koniec");
                }
                else
                {
                    Console.WriteLine("Brak kategorii dla tej ankiety.");
                }
            }

            Console.ReadKey();
        }
    }
}
