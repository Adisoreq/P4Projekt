using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Projekt.Models;

namespace Projekt.Views
{
    public partial class MainWindow : Window
    {
        private readonly TabItem? browseTab;
        private readonly TabItem? myPollsTab;
        private readonly TabItem? addPollTab;
        private readonly TabItem? profileTab;

        public MainWindow()
        {
            InitializeComponent();

            browseTab = MainTabControl.Items.Count > 0 ? MainTabControl.Items[0] as TabItem : null;
            myPollsTab = MainTabControl.Items.Count > 1 ? MainTabControl.Items[1] as TabItem : null;
            addPollTab = MainTabControl.Items.Count > 2 ? MainTabControl.Items[2] as TabItem : null;
            profileTab = MainTabControl.Items.Count > 3 ? MainTabControl.Items[3] as TabItem : null;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (!UserSession.Instance.IsLoggedIn)
            {
                ShowLoginView();
            }
            else
            {
                ShowPollsView();
                UpdateUIForLoggedInUser();
            }
        }

        public void ShowLoginView()
        {
            var loginView = new LoginView();
            loginView.Owner = this;
            loginView.LoginSucceeded += (s, e) =>
            {
                ShowPollsView();
                UpdateUIForLoggedInUser();
            };

            loginView.ShowDialog();
        }

        private void UpdateUIForLoggedInUser()
        {
            Title = "Ankiety";

            // Enable user-specific tabs like myPollsTab that should only be visible when logged in
            if (myPollsTab != null && !MainTabControl.Items.Contains(myPollsTab))
            {
                MainTabControl.Items.Insert(1, myPollsTab);
            }
        }

        public void ShowPollsView()
        {
            var pollsView = new PollsView();
        }

        public void ShowAddPollView()
        {
            var addPollView = new AddPollView();
            addPollView.Owner = this;
            addPollView.PollAdded += (s, e) => ShowPollsView();
        }

        public void ShowPollDetailsView(Projekt.Models.PollModel poll)
        {
            var pollDetailsView = new PollDetailsView(poll);
            pollDetailsView.Owner = this;
            pollDetailsView.VoteCompleted += (s, e) => ShowPollsView();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 2;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 0;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 3;
        }

        private void OptionsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OptionsList.SelectedIndex < 0)
                return;

            TabItem? tabToShow = null;
            switch (OptionsList.SelectedIndex)
            {
                case 0: tabToShow = browseTab; break;
                case 1: tabToShow = myPollsTab; break;
                case 2: tabToShow = addPollTab; break;
                case 3: tabToShow = profileTab; break;
            }

            if (tabToShow != null)
            {
                if (!MainTabControl.Items.Contains(tabToShow))
                {
                    MainTabControl.Items.Insert(OptionsList.SelectedIndex, tabToShow);
                }
                MainTabControl.SelectedItem = tabToShow;
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
            if (sender is PollsListView pollsListView && pollsListView.PollsListBox.SelectedItem is Projekt.Models.PollModel selectedPoll)
            {
                MessageBox.Show($"Wybrano ankietę: {selectedPoll.Name}");
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
