using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Models;

namespace Projekt.ViewModels
{
    public class AddPollViewModel : BaseViewModel
    {
        private string _pollName = string.Empty;
        private string _pollDescription = string.Empty;
        private ObservableCollection<OptionModel> _options = new();

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

        public ObservableCollection<OptionModel> Options => _options;

        public ICommand AddPollCommand { get; }
        public ICommand AddOptionCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action? RequestClose;

        public AddPollViewModel()
        {
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
                Options = Options.ToList()
            };

            // TODO: Dodaj ankietę do repozytorium

            RequestClose?.Invoke();
        }

        private void AddOption(object? parameter)
        {
            Options.Add(new OptionModel { Text = "Nowa opcja" });
        }
    }
}