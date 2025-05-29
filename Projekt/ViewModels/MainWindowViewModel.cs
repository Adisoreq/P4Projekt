// ViewModels/MainViewModel.cs  
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private readonly P4ProjektDbContext _context;
        private int _selectedOptionIndex;
        private PollModel? _selectedPoll;

        public MainWindowViewModel(P4ProjektDbContext context)
        {
            _context = context;
            AddPollCommand = new RelayCommand(_ => OnAddClick());
            BrowseCommand = new RelayCommand(_ => OnBrowseClick());
            LoginCommand = new RelayCommand(_ => OnLoginClick());
            SelectOptionCommand = new RelayCommand<int>(OnOptionSelected);
        }

        public ObservableCollection<PollModel> Polls
        {
            get => new ObservableCollection<PollModel>(_context.Polls.ToList());
        }

        // Properties for tab management
        public TabItem? BrowseTab { get; set; }
        public TabItem? MyPollsTab { get; set; }
        public TabItem? AddPollTab { get; set; }
        public TabItem? ProfileTab { get; set; }
        
        // Commands
        public ICommand AddPollCommand { get; }
        public ICommand BrowseCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand SelectOptionCommand { get; }

        // Events for view communication
        public event EventHandler<TabItem> TabSelectionRequested;
        public event EventHandler ShowLoginRequested;
        public event EventHandler ShowPollsRequested;
        public event EventHandler<PollModel> ShowPollDetailsRequested;
        public event EventHandler ShowAddPollRequested;

        public int SelectedOptionIndex
        {
            get => _selectedOptionIndex;
            set
            {
                if (_selectedOptionIndex != value)
                {
                    _selectedOptionIndex = value;
                    OnOptionSelected(value);
                    OnPropertyChanged(nameof(SelectedOptionIndex));
                }
            }
        }

        public PollModel? SelectedPoll
        {
            get => _selectedPoll;
            set
            {
                if (_selectedPoll != value)
                {
                    _selectedPoll = value;
                    OnPollSelected(value);
                    OnPropertyChanged(nameof(SelectedPoll));
                }
            }
        }

        public void Initialize()
        {
            if (!UserSession.Instance.IsLoggedIn)
            {
                OnShowLoginRequested();
            }
            else
            {
                OnShowPollsRequested();
                UpdateUIForLoggedInUser();
            }
        }

        public void UpdateUIForLoggedInUser()
        {
            // Logic for updating UI when user logs in
            OnPropertyChanged(nameof(UserSession.Instance.IsLoggedIn));
        }

        // Command handlers
        private void OnAddClick()
        {
            SelectTab(2);
        }

        private void OnBrowseClick()
        {
            SelectTab(0);
        }

        private void OnLoginClick()
        {
            SelectTab(3);
        }

        private void OnOptionSelected(int optionIndex)
        {
            switch (optionIndex)
            {
                case 0: SelectTab(0); break;
                case 1: SelectTab(1); break;
                case 2: SelectTab(2); break;
                case 3: SelectTab(3); break;
            }
        }

        // Event raisers for View communication
        public void SelectTab(int index)
        {
            TabItem? tabToShow = null;
            switch (index)
            {
                case 0: tabToShow = BrowseTab; break;
                case 1: tabToShow = MyPollsTab; break;
                case 2: tabToShow = AddPollTab; break;
                case 3: tabToShow = ProfileTab; break;
            }

            if (tabToShow != null)
            {
                TabSelectionRequested?.Invoke(this, tabToShow);
            }
        }

        public void OnShowLoginRequested()
        {
            ShowLoginRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnShowPollsRequested() 
        {
            ShowPollsRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnShowPollDetailsRequested(PollModel poll)
        {
            ShowPollDetailsRequested?.Invoke(this, poll);
        }

        public void OnShowAddPollRequested()
        {
            ShowAddPollRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnPollSelected(PollModel? poll)
        {
            if (poll != null)
            {
                OnShowPollDetailsRequested(poll);
            }
            else
            {
                MessageBox.Show("Nie wybrano ankiety.");
            }
        }

        public void Vote(UserModel user, PollModel poll, OptionModel option)
        {
            OnPropertyChanged();
        }

        public int DetermineTabIndex(TabItem tabItem)
        {
            if (tabItem == BrowseTab) return 0;
            if (tabItem == MyPollsTab) return 1;
            if (tabItem == AddPollTab) return 2;
            if (tabItem == ProfileTab) return 3;
            return 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}