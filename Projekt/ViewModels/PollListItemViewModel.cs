using Projekt.Data;
using Projekt.Models;
using Projekt.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Projekt.ViewModels
{
    public class PollListItemViewModel : BaseViewModel
    {
        public PollModel Poll { get; }

        private ObservableCollection<string> _categories = new();
        public ObservableCollection<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public string Name => Poll.Name;
        public string Description => Poll.Description;

        public PollListItemViewModel(PollModel poll)
        {
            Poll = poll;

            using (var dbContext = new P4ProjektDbContext())
            {

                var categories = PollService.Instance.getPollCategories(Poll.Id);

                if (categories != null)
                {
                    Categories = new ObservableCollection<string>(categories.Select(c => c.Name));
                    System.Diagnostics.Debug.WriteLine($"Total categories added: {Categories.Count}");
                    foreach (var cat in Categories) {
                        System.Diagnostics.Debug.WriteLine("Kategoria:" + cat);
                    }
                    
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No categories found for poll");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}