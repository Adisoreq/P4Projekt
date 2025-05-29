using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Models;

namespace Projekt.ViewModels
{
    public class AddPollViewModel
    {
        private string _pollName = string.Empty;
        private string _pollDescription = string.Empty;
        private ObservableCollection<OptionModel> _options = new ObservableCollection<OptionModel>();
        
        public string PollName
        {
            get => _pollName;
            set => _pollName = value;
        }
        
        public string PollDescription
        {
            get => _pollDescription;
            set => _pollDescription = value;
        }
        
        public ObservableCollection<OptionModel> Options => _options;
        
        public ICommand AddPollCommand { get; }
        public ICommand AddOptionCommand { get; }
        
        public event EventHandler? PollAdded;
        
        public AddPollViewModel()
        {
            AddPollCommand = new RelayCommand(AddPoll);
            AddOptionCommand = new RelayCommand(AddOption);
        }
        
        private void AddPoll(object? parameter)
        {
            // Walidacja i dodawanie ankiety do bazy/kolekcji
            if (string.IsNullOrWhiteSpace(PollName))
                return;
                
            PollModel newPoll = new PollModel
            {
                Name = PollName,
                Description = PollDescription,
                Options = Options.ToList()
            };
            
            // TODO: Dodaj ankietę do repozytorium
            
            PollAdded?.Invoke(this, EventArgs.Empty);
        }
        
        private void AddOption(object? parameter)
        {
            Options.Add(new OptionModel { Text = "Nowa opcja" });
        }
    }
}