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
            // Utwórz instancję DbContext
            using (var dbContext = new P4ProjektDbContext())
            {
                // Utwórz instancję PollService
                var pollService = new PollService(dbContext);

                // Przykładowe ID ankiety
                int pollId = 1;

                // Pobierz kategorie ankiety
                var categories = pollService.getPollCategories(pollId);

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
