using Projekt.Data;
using Projekt.Models;
using Projekt.Services;
using Projekt.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Projekt.ViewModels
{
    public class PollsListViewModel : BaseViewModel
    {
        private ObservableCollection<PollModel> _polls = new ObservableCollection<PollModel>();
        public ObservableCollection<PollListItemViewModel> PollItems { get; set; }
        
        private PollModel? _selectedPoll;

        public PollsListViewModel(PollService pollService)
        {
            LoadPolls();
            PollSelectedCommand = new RelayCommand(OnPollSelected);

            var polls = pollService.GetPolls();
            PollItems = new ObservableCollection<PollListItemViewModel>(
                polls.Select(p => new PollListItemViewModel(p))
            );
        }

        public ObservableCollection<PollModel> Polls = new(PollService.Instance.GetPolls());

        public PollModel? SelectedPoll
        {
            get => _selectedPoll;
            set
            {
                _selectedPoll = value;
                OnPropertyChanged();
            }
        }

        public ICommand PollSelectedCommand { get; }

        private void LoadPolls()
        {
            Polls = new ObservableCollection<PollModel>(PollService.Instance.GetPolls());
        }

        private void OnPollSelected(object? parameter)
        {
            if (parameter is PollModel poll)
            {
                SelectedPoll = poll;
            }
        }
    }
}
