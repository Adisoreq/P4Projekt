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

        // Events for view interactions
        public event EventHandler<EventArgs> ShowLoginRequested;
        public event EventHandler<EventArgs> ShowPollsRequested;
        public event EventHandler<PollModel> ShowPollDetailsRequested;
        public event EventHandler<EventArgs> ShowAddPollRequested;

        public MainWindowViewModel(P4ProjektDbContext dbContext)
        {
            _dbContext = dbContext;
            Polls = new ObservableCollection<PollModel>();
            
            // Initialize commands
            LoginCommand = new RelayCommand(OnShowLoginRequested);
            ShowPollsCommand = new RelayCommand(OnShowPollsRequested);
            ShowPollDetailsCommand = new RelayCommand(OnShowPollDetailsRequested);
            ShowAddPollCommand = new RelayCommand(OnShowAddPollRequested);
        }

        public ObservableCollection<PollModel> Polls { get; }
        
        public TabItem BrowseTab 
        { 
            get => _browseTab; 
            set { _browseTab = value; OnPropertyChanged(); }
        }
        
        public TabItem MyPollsTab 
        { 
            get => _myPollsTab; 
            set { _myPollsTab = value; OnPropertyChanged(); }
        }
        
        public TabItem AddPollTab 
        { 
            get => _addPollTab; 
            set { _addPollTab = value; OnPropertyChanged(); }
        }
        
        public TabItem ProfileTab 
        { 
            get => _profileTab; 
            set { _profileTab = value; OnPropertyChanged(); }
        }

        public PollModel SelectedPoll
        {
            get => _selectedPoll;
            set 
            { 
                _selectedPoll = value; 
                OnPropertyChanged();
                if (value != null)
                {
                    OnShowPollDetailsRequested(value);
                }
            }
        }

        public int SelectedOptionIndex
        {
            get => _selectedOptionIndex;
            set { _selectedOptionIndex = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand ShowPollsCommand { get; }
        public ICommand ShowPollDetailsCommand { get; }
        public ICommand ShowAddPollCommand { get; }

        public void Initialize()
        {
            // Load data or perform initialization
            // For now just trigger login view
            OnShowLoginRequested(null);
        }

        public void UpdateUIForLoggedInUser()
        {
            // Update UI elements based on logged-in user
        }

        public int DetermineTabIndex(TabItem tabItem)
        {
            if (tabItem == BrowseTab) return 0;
            if (tabItem == MyPollsTab) return 1;
            if (tabItem == AddPollTab) return 2;
            if (tabItem == ProfileTab) return 3;
            return -1;
        }

        public void OnShowLoginRequested(object? parameter)
        {
            ShowLoginRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnShowPollsRequested(object? parameter)
        {
            ShowPollsRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnShowPollDetailsRequested(object? parameter)
        {
            if (parameter is PollModel poll)
            {
                ShowPollDetailsRequested?.Invoke(this, poll);
            }
            else if (SelectedPoll != null)
            {
                ShowPollDetailsRequested?.Invoke(this, SelectedPoll);
            }
        }

        public void OnShowAddPollRequested(object? parameter)
        {
            ShowAddPollRequested?.Invoke(this, EventArgs.Empty);
        }

        public void HandlePollSelected(PollModel selectedPoll)
        {
            if (selectedPoll != null)
            {
                SelectedPoll = selectedPoll;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}