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
using Projekt.Views;

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
            VoteCommand = new RelayCommand(Vote, CanVote);

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
        public ICommand VoteCommand { get; }

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
            SelectedTabIndex = 0;
        }

        public void UpdateUIForLoggedInUser()
        {
            if (UserSession.Instance.IsLoggedIn)
            {
                var loggedInUsername = UserSession.Instance.Username;
            }
            
            OnPropertyChanged(nameof(Tabs));
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
                if (SelectedPoll != poll)
                    SelectedPoll = poll;
                SelectedTabIndex = 1;
                if (SelectedPoll != poll)
                    SelectedPoll = poll;
                SelectedTabIndex = 1;
            }
            else if (SelectedPoll != null)
            {
                ShowPollDetailsRequested?.Invoke(this, SelectedPoll);
            }
        }

        public void OnShowAddPollRequested(object? parameter)
        {
            var addPollViewModel = new AddPollViewModel(_dbContext);
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

        public void LoadPolls()
        {
            Polls.Clear();
            foreach (var poll in _dbContext.Polls.ToList())
                Polls.Add(poll);
        }

        public void HandlePollSelected(PollModel selectedPoll)
        {

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

        private void Vote(object? parameter)
        {
            if (SelectedPoll == null || SelectedOptionIndex < 0)
                return;

            // Pobierz wybraną opcję
            var poll = _dbContext.Polls
                .Where(p => p.Id == SelectedPoll.Id)
                .Select(p => new { p.Id, Options = p.Options.ToList() })
                .FirstOrDefault();

            if (poll == null)
                return;

            var option = poll.Options.ElementAtOrDefault(SelectedOptionIndex);
            if (option == null)
                return;

            // Sprawdź, czy użytkownik już głosował w tej ankiecie
            int userId = UserSession.Instance.UserId;
            bool alreadyVoted = _dbContext.Votes.Any(v => v.PollId == poll.Id && v.UserId == userId);

            if (alreadyVoted)
            {
                MessageBox.Show("Już oddałeś głos w tej ankiecie.", "Głosowanie", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Dodaj głos
            var vote = new VoteModel
            {
                PollId = poll.Id,
                OptionId = option.Id,
                UserId = userId
            };

            _dbContext.Votes.Add(vote);
            _dbContext.SaveChanges();

            // Odśwież dane w UI
            LoadPolls();
            SelectedPoll = Polls.FirstOrDefault(p => p.Id == poll.Id);

            MessageBox.Show("Twój głos został zapisany.", "Głosowanie", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CanVote(object? parameter)
        {
            return SelectedPoll != null && SelectedOptionIndex >= 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}