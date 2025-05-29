using Projekt.Models;
using System.Windows.Input;

namespace Projekt.ViewModels
{
    public class PollsViewModel : BaseViewModel
    {
        public ICommand PollSelectedCommand { get; }

        public PollsViewModel()
        {
            PollSelectedCommand = new RelayCommand(OnPollSelected);
        }

        private void OnPollSelected(object parameter)
        {
            if (parameter is PollModel selectedPoll)
            {
                // Logic for handling poll selection
            }
        }
    }
}