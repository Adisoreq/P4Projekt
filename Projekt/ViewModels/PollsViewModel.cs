using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Models;
using Projekt.Services;
using Projekt.Data;

namespace Projekt.ViewModels
{
    public class PollsViewModel : BaseViewModel
    {
        private readonly PollService _pollService;
        private ObservableCollection<PollModel> _polls = new ObservableCollection<PollModel>();
        private PollModel? _selectedPoll;

        public ObservableCollection<PollModel> Polls
        {
            get => _polls;
            set
            {
                _polls = value;
                OnPropertyChanged();
            }
        }

        public PollModel? SelectedPoll
        {
            get => _selectedPoll;
            set
            {
                _selectedPoll = value;
                OnPropertyChanged();
                if (value != null)
                {
                    SelectPollCommand.Execute(value);
                }
            }
        }

        public ICommand SelectPollCommand { get; }
        public ICommand RefreshPollsCommand { get; }

        public PollsViewModel()
        {
            var dbContext = new P4ProjektDbContext();
            _pollService = new PollService(dbContext);
            
            SelectPollCommand = new RelayCommand(OnPollSelected);
            RefreshPollsCommand = new RelayCommand(_ => LoadPolls());
            
            LoadPolls();
        }

        public PollsViewModel(PollService pollService)
        {
            _pollService = pollService;
            
            SelectPollCommand = new RelayCommand(OnPollSelected);
            RefreshPollsCommand = new RelayCommand(_ => LoadPolls());
            
            LoadPolls();
        }

        private void LoadPolls()
        {
            var pollsList = _pollService.GetPolls();
            Polls = new ObservableCollection<PollModel>(pollsList);
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