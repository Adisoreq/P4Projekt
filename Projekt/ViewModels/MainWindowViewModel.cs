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
        private readonly P4ProjektDbContext _dbContext;
        private PollModel _selectedPoll;
        private int _selectedOptionIndex;
        private int _selectedTabIndex;
        private ObservableCollection<TabItem> _tabs;

        // Events for view interactions
        public event EventHandler<EventArgs> ShowLoginRequested;
        public event EventHandler<EventArgs> ShowPollsRequested;
        public event EventHandler<PollModel> ShowPollDetailsRequested;
        public event EventHandler<EventArgs> ShowAddPollRequested;
        public event EventHandler<TabItem> TabClosed;

        public MainWindowViewModel(P4ProjektDbContext dbContext)
        {
            _dbContext = dbContext;
            Polls = new ObservableCollection<PollModel>();
            _tabs = new ObservableCollection<TabItem>();
            
            // Initialize commands
            LoginCommand = new RelayCommand(OnShowLoginRequested);
            ShowPollsCommand = new RelayCommand(OnShowPollsRequested);
            ShowPollDetailsCommand = new RelayCommand(OnShowPollDetailsRequested);
            ShowAddPollCommand = new RelayCommand(OnShowAddPollRequested);
            CloseTabCommand = new RelayCommand(CloseTab);
            SelectTabCommand = new RelayCommand(SelectTab);

            // Initialize tabs
            InitializeTabs();

            _selectedOptionIndex = 0;
            _selectedTabIndex = 0;
        }

        public ObservableCollection<PollModel> Polls { get; }

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
            set
            {
                if (_selectedOptionIndex != value)
                {
                    _selectedOptionIndex = value;
                    OnPropertyChanged();
                    if (_selectedTabIndex != value)
                        SelectedTabIndex = value; // Synchronizuj indeksy tylko jeśli są różne
                }
            }
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged();
                    if (_selectedOptionIndex != value)
                        SelectedOptionIndex = value; // Synchronizuj w drugą stronę tylko jeśli są różne
                }
            }
        }

        public ObservableCollection<TabItem> Tabs
        {
            get => _tabs;
            set
            {
                _tabs = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand ShowPollsCommand { get; }
        public ICommand ShowPollDetailsCommand { get; }
        public ICommand ShowAddPollCommand { get; }
        public ICommand CloseTabCommand { get; }
        public ICommand SelectTabCommand { get; }

        private void InitializeTabs()
        {
            // Create tabs
            var browseTab = new TabItem { Header = "Przeglądaj" };
            browseTab.Content = new Views.PollsView();

            var favoritesTab = new TabItem { Header = CreateClosableTabHeader("Ulubione") };
            favoritesTab.Content = new TextBlock { Text = "Twoje ulubione ankiety", FontSize = 16, Margin = new System.Windows.Thickness(10) };

            var myPollsTab = new TabItem { Header = CreateClosableTabHeader("Moje ankiety") };
            myPollsTab.Content = new TextBlock { Text = "Twoje własne ankiety", FontSize = 16, Margin = new System.Windows.Thickness(10) };

            Tabs = new ObservableCollection<TabItem> { browseTab, favoritesTab, myPollsTab };
        }

        private object CreateClosableTabHeader(string headerText)
        {
            var header = new StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal };
            header.Children.Add(new TextBlock { Text = headerText, VerticalAlignment = System.Windows.VerticalAlignment.Center });

            var closeButton = new Button
            {
                Content = "✖",
                Width = 18,
                Height = 18,
                Margin = new System.Windows.Thickness(5, 0, 0, 0),
                Padding = new System.Windows.Thickness(0),
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Style = (Style)Application.Current.FindResource(ToolBar.ButtonStyleKey)
            };
            closeButton.Command = CloseTabCommand;
            closeButton.CommandParameter = header.Parent; // Pass the TabItem as the command parameter

            header.Children.Add(closeButton);
            return header;
        }

        public void Initialize()
        {
            OnShowLoginRequested(null);
            // Set default tab
            SelectedTabIndex = 0;
        }

        public void UpdateUIForLoggedInUser()
        {
            // Update UI elements based on logged-in user
        }

        public int DetermineTabIndex(TabItem tabItem)
        {
            return Tabs.IndexOf(tabItem);
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

        private void CloseTab(object? parameter)
        {
            if (parameter is TabItem tabItem)
            {
                if (Tabs.Contains(tabItem))
                {
                    Tabs.Remove(tabItem);
                }
            }
        }

        private void SelectTab(object? parameter)
        {
            if (parameter is int index)
            {
                SelectedTabIndex = index;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}