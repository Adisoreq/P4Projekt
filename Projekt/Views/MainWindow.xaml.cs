using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Projekt.Data;
using Projekt.Models;
using Projekt.ViewModels;

namespace Projekt.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            
            var dbContext = new P4ProjektDbContext();

            viewModel = new MainWindowViewModel(dbContext)
            {
                BrowseTab = MainTabControl.Items.Count > 0 ? MainTabControl.Items[0] as TabItem : null,
                MyPollsTab = MainTabControl.Items.Count > 1 ? MainTabControl.Items[1] as TabItem : null,
                AddPollTab = MainTabControl.Items.Count > 2 ? MainTabControl.Items[2] as TabItem : null,
                ProfileTab = MainTabControl.Items.Count > 3 ? MainTabControl.Items[3] as TabItem : null
            };
            
            // Set up event handlers
            viewModel.TabSelectionRequested += ViewModel_TabSelectionRequested;
            viewModel.ShowLoginRequested += ViewModel_ShowLoginRequested;
            viewModel.ShowPollsRequested += ViewModel_ShowPollsRequested;
            viewModel.ShowPollDetailsRequested += ViewModel_ShowPollDetailsRequested;
            viewModel.ShowAddPollRequested += ViewModel_ShowAddPollRequested;
            
            DataContext = viewModel;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            viewModel.Initialize();
        }

        // ViewModel event handlers
        private void ViewModel_TabSelectionRequested(object sender, TabItem tabItem)
        {
            if (!MainTabControl.Items.Contains(tabItem))
            {
                int index = viewModel.DetermineTabIndex(tabItem);
                MainTabControl.Items.Insert(index, tabItem);
            }
            MainTabControl.SelectedItem = tabItem;
        }

        private void ViewModel_ShowLoginRequested(object sender, EventArgs e)
        {
            ShowLoginView();
        }

        private void ViewModel_ShowPollsRequested(object sender, EventArgs e)
        {
            ShowPollsView();
        }

        private void ViewModel_ShowPollDetailsRequested(object sender, PollModel poll)
        {
            ShowPollDetailsView(poll);
        }

        private void ViewModel_ShowAddPollRequested(object sender, EventArgs e)
        {
            ShowAddPollView();
        }

        // View-specific methods that must remain in the View
        public void ShowLoginView()
        {
            var loginView = new LoginView();
            loginView.Owner = this;
            loginView.LoginSucceeded += (s, e) =>
            {
                viewModel.OnShowPollsRequested();
                viewModel.UpdateUIForLoggedInUser();
            };

            loginView.ShowDialog();
        }

        public void ShowPollsView()
        {
            var pollsView = new PollsView();
            // UI-specific setup if needed
        }

        public void ShowAddPollView()
        {
            var addPollView = new AddPollView();
            addPollView.Owner = this;
            addPollView.PollAdded += (s, e) => viewModel.OnShowPollsRequested();
            addPollView.ShowDialog();
        }

        public void ShowPollDetailsView(PollModel poll)
        {
            var pollDetailsView = new PollDetailsView(poll);
            pollDetailsView.Owner = this;
            pollDetailsView.ShowDialog();
        }

        // UI event handlers that need to be connected to the ViewModel
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LoginCommand.Execute(null);
        }

        private void OptionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OptionsList.SelectedIndex >= 0)
            {
                viewModel.SelectedOptionIndex = OptionsList.SelectedIndex;
            }
        }

        private void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var tabItem = FindParent<TabItem>(button);

            // Nie zamykać pierwszej karty
            if (tabItem != null && MainTabControl.Items.Contains(tabItem) && MainTabControl.Items.IndexOf(tabItem) != 0)
            {
                MainTabControl.Items.Remove(tabItem);
            }
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent) return parent;
            return FindParent<T>(parentObject);
        }

        private void PollsListView_PollSelected(object sender, RoutedEventArgs e)
        {
            if (sender is PollsListView pollsListView && pollsListView.PollsListBox.SelectedItem is PollModel selectedPoll)
            {
                viewModel.SelectedPoll = selectedPoll;
            }
            else
            {
                MessageBox.Show("Nie wybrano ankiety.");
            }
        }

        private void PollsView_Loaded(object sender, RoutedEventArgs e)
        {
            // UI-specific initialization if needed
        }
    }
}
