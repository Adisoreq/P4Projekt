using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Models;
using Projekt.Services;

namespace Projekt.ViewModels
{
    public class PollDetailsViewModel : BaseViewModel
    {
        private PollModel _poll;
        public PollModel Poll { get => _poll; }
        public OptionModel[] Options { get => Poll.Options.ToArray(); }
        private OptionModel? _selectedOption;
        private bool? _dialogResult;

        public string Title {
            get => Poll.Name;
        }

        public string Description {
            get => Poll.Description;
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
            _poll = poll;  // Uncomment this line to use the actual poll
            VoteCommand = new RelayCommand(Vote);
            CloseCommand = new RelayCommand(_ => DialogResult = true);

            // Remove the temporary code that creates a hardcoded poll
            // The line below is causing your actual poll to be ignored
            /*
            _poll = new PollModel
            {
                Name = "Poll",
                Description = "Desc",
                Options = [
                    // ...options...
                ]
            };
            */
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