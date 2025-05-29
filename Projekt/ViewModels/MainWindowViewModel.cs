// ViewModels/MainViewModel.cs  
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Projekt.Models;
using Projekt.Data;

namespace Projekt.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly P4ProjektDbContext _dbContext;
        private TabItem _browseTab;
        private TabItem _myPollsTab;
        private TabItem _addPollTab;
        private TabItem _profileTab;
        private PollModel _selectedPoll;
        private int _selectedOptionIndex;

        public MainWindowViewModel(P4ProjektDbContext dbContext)
        {
            _dbContext = dbContext;
            LoginCommand = new RelayCommand(OnShowLoginRequested);
            ShowPollsCommand = new RelayCommand(OnShowPollsRequested);
            ShowPollDetailsCommand = new RelayCommand<PollModel>(OnShowPollDetailsRequested);
            ShowAddPollCommand = new RelayCommand(OnShowAddPollRequested);
        }

        public ObservableCollection<PollModel> Polls
        {
            get => new ObservableCollection<PollModel>(_dbContext.Polls.ToList());
        }

        public TabItem BrowseTab
        {
            get => _browseTab;
            set => _browseTab = value;
        }

        public TabItem MyPollsTab
        {
            get => _myPollsTab;
            set => _myPollsTab = value;
        }

        public TabItem AddPollTab
        {
            get => _addPollTab;
            set => _addPollTab = value;
        }

        public TabItem ProfileTab
        {
            get => _profileTab;
            set => _profileTab = value;
        }

        public PollModel SelectedPoll
        {
            get => _selectedPoll;
            set => _selectedPoll = value;
        }

        public int SelectedOptionIndex
        {
            get => _selectedOptionIndex;
            set => _selectedOptionIndex = value;
        }

        public ICommand LoginCommand { get; }
        public ICommand ShowPollsCommand { get; }
        public ICommand ShowPollDetailsCommand { get; }
        public ICommand ShowAddPollCommand { get; }

        public event EventHandler<TabItem> TabSelectionRequested;
        public event EventHandler ShowLoginRequested;
        public event EventHandler ShowPollsRequested;
        public event EventHandler<PollModel> ShowPollDetailsRequested;
        public event EventHandler ShowAddPollRequested;

        public void Initialize()
        {
            if (!UserSession.Instance.IsLoggedIn)
            {
                OnShowLoginRequested(null);
            }
            else
            {
                OnShowPollsRequested(null);
                UpdateUIForLoggedInUser();
            }
        }

        public void UpdateUIForLoggedInUser()
        {
            // Logic for updating UI when user logs in
            OnPropertyChanged(nameof(UserSession.Instance.IsLoggedIn));
        }

        public int DetermineTabIndex(TabItem tabItem)
        {
            if (tabItem == BrowseTab) return 0;
            if (tabItem == MyPollsTab) return 1;
            if (tabItem == AddPollTab) return 2;
            if (tabItem == ProfileTab) return 3;
            return 0;
        }

        public void OnShowLoginRequested(object parameter)
        {
            ShowLoginRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnShowPollsRequested(object parameter)
        {
            ShowPollsRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnShowPollDetailsRequested(object parameter)
        {
            if (parameter is PollModel poll)
            {
                ShowPollDetailsRequested?.Invoke(this, poll);
            }
        }

        public void OnShowAddPollRequested(object parameter)
        {
            ShowAddPollRequested?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}