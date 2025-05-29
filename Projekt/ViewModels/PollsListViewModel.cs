using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Projekt.Data;
using Projekt.Models;
using System.Diagnostics;

namespace Projekt.ViewModels
{
    public class PollsListViewModel : BaseViewModel
    {
        private ObservableCollection<PollModel> _polls = new ObservableCollection<PollModel>();
        private PollModel? _selectedPoll;

        public PollsListViewModel()
        {
            LoadPolls();
            PollSelectedCommand = new RelayCommand(OnPollSelected);
        }

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
            }
        }

        public ICommand PollSelectedCommand { get; }

        private void LoadPolls()
        {
            using var db = new P4ProjektDbContext();
            var polls = db.Polls.ToList();
            Polls = new ObservableCollection<PollModel>(polls);
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
