using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Models;
using Projekt.Data;
using System.Linq;

namespace Projekt.ViewModels
{
    public class AddPollViewModel : BaseViewModel
    {
        private readonly P4ProjektDbContext _dbContext;
        private string _pollName = string.Empty;
        private string _pollDescription = string.Empty;
        private ObservableCollection<OptionModel> _options = new();
        private bool _public = true;
        private bool _closed = false;
        private bool _multipleChoice = false;

        public string PollName
        {
            get => _pollName;
            set { _pollName = value; OnPropertyChanged(); }
        }

        public string PollDescription
        {
            get => _pollDescription;
            set { _pollDescription = value; OnPropertyChanged(); }
        }

        public bool Public
        {
            get => _public;
            set { _public = value; OnPropertyChanged(); }
        }

        public bool Closed
        {
            get => _closed;
            set { _closed = value; OnPropertyChanged(); }
        }

        public bool MultipleChoice
        {
            get => _multipleChoice;
            set { _multipleChoice = value; OnPropertyChanged(); }
        }

        public ObservableCollection<OptionModel> Options => _options;

        public ICommand AddPollCommand { get; }
        public ICommand AddOptionCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action? RequestClose;
        public event Action? PollAdded;

        public AddPollViewModel(P4ProjektDbContext dbContext)
        {
            _dbContext = dbContext;
            AddPollCommand = new RelayCommand(AddPoll);
            AddOptionCommand = new RelayCommand(AddOption);
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        private void AddPoll(object? parameter)
        {
            if (string.IsNullOrWhiteSpace(PollName))
                return;

            PollModel newPoll = new()
            {
                Name = PollName,
                Description = PollDescription,
                Public = Public,
                Closed = Closed,
                MultipleChoice = MultipleChoice,
                Created = DateTimeOffset.Now,
                End = DateTimeOffset.Now.AddDays(7),
                AuthorId = UserSession.Instance.UserId,
                Options = Options.ToList()
            };

            _dbContext.Polls.Add(newPoll);
            _dbContext.SaveChanges();

            PollAdded?.Invoke();
            RequestClose?.Invoke();
        }

        private void AddOption(object? parameter)
        {
            Options.Add(new OptionModel { Text = "Nowa opcja" });
        }
    }
}