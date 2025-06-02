using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Models;
using Projekt.Data;
using System.Linq;
using Projekt.Services;
using System.Runtime.CompilerServices;

namespace Projekt.ViewModels
{
    public class AddPollViewModel : BaseViewModel
    {
        private readonly P4ProjektDbContext _dbContext;
        private string _NewOptionText = "Nowa opcja";

        public PollModel Poll = new PollModel 
        { 
            Name = "New Poll",
            Description = "Poll description",

            Author = UserSession.Instance.User,
            AuthorId = UserSession.Instance.UserId,

            Public = true,
            Closed = false,
            MultipleChoice = false
        };
        
        private ObservableCollection<OptionModel> _options = 
        [
            new OptionModel { Text = "Option 1" },
            new OptionModel { Text = "Option 2" },
            new OptionModel { Text = "Option 3" }
        ];
        
        private ObservableCollection<CategoryViewModel> _categories = new();
   
        public string Name
        {
            get => Poll.Name;
            set { Poll.Name = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => Poll.Description;
            set { Poll.Description = value; OnPropertyChanged(); }
        }

        public bool Public
        {
            get => Poll.Public;
            set { Poll.Public = value; OnPropertyChanged(); }
        }

        public bool Closed
        {
            get => Poll.Closed;
            set { Poll.Closed = value; OnPropertyChanged(); }
        }

        public bool MultipleChoice
        {
            get => Poll.MultipleChoice;
            set { Poll.MultipleChoice = value; OnPropertyChanged(); }
        }

        public string NewOptionText
        {
            get => _NewOptionText;
            set
            {
                _NewOptionText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OptionModel> Options 
        {
            get => _options;
            set
            {
                _options = value;
                OnPropertyChanged();
            }
        }
        
        public ObservableCollection<CategoryViewModel> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPollCommand { get; }
        public ICommand AddOptionCommand { get; }
        public ICommand RemoveOptionCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action? RequestClose;
        public event Action? PollAdded;

        public AddPollViewModel()
        {   
            AddPollCommand = new RelayCommand(AddPoll);
            AddOptionCommand = new RelayCommand(AddOption);
            RemoveOptionCommand = new RelayCommand<OptionModel>(RemoveOption);
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
            
            LoadCategories();
        }
        
        private void LoadCategories()
        {
            var allCategories = PollService.Instance.GetCategories();
            if (allCategories != null)
            {
                Categories.Clear();
                foreach (var category in allCategories)
                {
                    Categories.Add(new CategoryViewModel 
                    { 
                        Category = category, 
                        IsSelected = false 
                    });
                }
            }
        }

        private async void AddPoll(object? parameter)
        {
            try
            {
                // Create a new poll with all required properties
                PollModel newPoll = new PollModel
                {
                    Name = Name,
                    Description = Description,
                    Author = UserSession.Instance.User,
                    AuthorId = UserSession.Instance.UserId,
                    Public = Public,
                    Closed = Closed,
                    MultipleChoice = MultipleChoice,
                    Created = DateTimeOffset.Now,
                    End = DateTimeOffset.Now.AddDays(7) // Default 7 day duration
                };

                // Add options to the poll
                foreach (var option in Options)
                {
                    // Create new option objects to avoid tracking issues
                    newPoll.Options.Add(new OptionModel 
                    { 
                        Text = option.Text 
                    });
                }
                
                // Add selected categories
                var selectedCategories = Categories.Where(c => c.IsSelected).Select(c => c.Category).ToList();
                foreach (var category in selectedCategories)
                {
                    newPoll.Categories.Add(category);
                }
                
                // Add the poll to the database asynchronously
                await PollService.Instance.AddPoll(newPoll);
                
                // Notify subscribers that a poll was added
                PollAdded?.Invoke();
                
                // Close the dialog
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error adding poll: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void AddOption(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewOptionText))
            {
                Options.Add(new OptionModel { Text = NewOptionText });
                NewOptionText = "Nowa opcja";
            }
        }

        private void RemoveOption(OptionModel option)
        {
            if (option != null && Options.Contains(option))
            {
                Options.Remove(option);
            }
        }
    }
    
    // ViewModel class for category selection
    public class CategoryViewModel : BaseViewModel
    {
        private bool _isSelected;
        
        public CategoryModel Category { get; set; }
        public string Name => Category?.Name ?? string.Empty;
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}