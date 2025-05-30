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
            
            DataContext = viewModel;

            // Subscribe to events in ViewModel and create appropriate views
            viewModel.ShowLoginRequested += (s, e) => new LoginView().ShowDialog();
            viewModel.ShowPollsRequested += (s, e) => new PollsView();
            viewModel.ShowPollDetailsRequested += (s, poll) => {
                var detailsView = new PollDetailsView(poll);
                detailsView.Owner = this;
                detailsView.ShowDialog();
            };
            viewModel.ShowAddPollRequested += (s, e) => {
                var addView = new AddPollView();
                addView.Owner = this;
                addView.PollAdded += (sender, args) => viewModel.OnShowPollsRequested(null);
                addView.ShowDialog();
            };
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            viewModel.Initialize();
        }

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
        }
    }
}
