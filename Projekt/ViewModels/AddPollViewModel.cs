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
        private string _NewOptionText = "Nowa opcja";

        // Example data

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

        public ICommand AddPollCommand { get; }
        public ICommand AddOptionCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action? RequestClose;
        public event Action? PollAdded;

        public AddPollViewModel(P4ProjektDbContext dbContext)
        {
            AddPollCommand = new RelayCommand(AddPoll);
            AddOptionCommand = new RelayCommand(AddOption);
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        private void AddPoll(object? parameter)
        {
            Poll.Options = [.. Options];

            PollService.Instance.AddPoll(Poll).ContinueWith(_ =>
            {
                PollAdded?.Invoke();
                RequestClose?.Invoke();
            });
        }

        private void AddOption(object? parameter)
        {
            Options.Add(new OptionModel { Text = NewOptionText });
        }
    }
}