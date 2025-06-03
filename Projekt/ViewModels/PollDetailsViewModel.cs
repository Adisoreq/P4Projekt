using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Models;

namespace Projekt.ViewModels
{
    public class PollDetailsViewModel : BaseViewModel
    {
        public PollModel Poll { get; }
        public ObservableCollection<OptionModel> Options { get; }
        private OptionModel? _selectedOption;
        private bool? _dialogResult;

        public string Title {
            get => Poll.Name;
            set 
            {
                Poll.Name = value;
                OnPropertyChanged(nameof(Poll));
            }
        }

        public OptionModel? SelectedOption
        {
            get => _selectedOption;
            set { _selectedOption = value; OnPropertyChanged(); }
        }

        public bool? DialogResult
        {
            get => _dialogResult;
            set { _dialogResult = value; OnPropertyChanged(); }
        }

        public ICommand VoteCommand { get; }
        public ICommand CloseCommand { get; }

        public PollDetailsViewModel(PollModel poll)
        {
            Poll = poll;
            Options = new ObservableCollection<OptionModel>(poll.Options);
            VoteCommand = new RelayCommand(Vote);
            CloseCommand = new RelayCommand(_ => DialogResult = true);
        }

        private void Vote(object? parameter)
        {
            if (SelectedOption != null)
            {
                DialogResult = true;
            }
            else
            {
            }
        }
    }
}