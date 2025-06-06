using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Models;
using Projekt.Services;
using Projekt.Data;
using Projekt.Views;
using System.Diagnostics;

namespace Projekt.ViewModels
{
    public class PollsViewModel : BaseViewModel
    {
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

        public ICommand AddNewPollCommand { get; }
        public ICommand SelectPollCommand { get; }
        public ICommand RefreshPollsCommand { get; }

        public PollsViewModel()
        {
            AddNewPollCommand = new RelayCommand(_ => ShowNewPollWindow());
            SelectPollCommand = new RelayCommand(OnPollSelected);
            RefreshPollsCommand = new RelayCommand(_ => LoadPolls());
            
            LoadPolls();
        }

        private void LoadPolls()
        {
            _polls = new ObservableCollection<PollModel>(PollService.Instance.GetPolls());
        }

        private void OnPollSelected(object? parameter)
        {
            if (parameter is PollModel poll)
            {
                SelectedPoll = poll;
            }
        }

        private void ShowNewPollWindow()
        {
            var addPollViewModel = new AddPollViewModel();
            var addPollView = new AddPollView
            {
                DataContext = addPollViewModel
            };

            addPollViewModel.RequestClose += () => addPollView.Close();
            addPollViewModel.PollAdded += () =>
            {
                LoadPolls();
            };

            addPollView.ShowDialog();
        }
    }
}