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

            viewModel = new MainWindowViewModel(dbContext);
            
            DataContext = viewModel;

            viewModel.ShowLoginRequested += (s, e) => {
                var loginView = new LoginView();
                loginView.Owner = this;
                

                var loginViewModel = new LoginViewModel(dbContext);
                loginView.DataContext = loginViewModel;

                bool? result = loginView.ShowDialog();

                if (result == true && UserSession.Instance.IsLoggedIn)
                {
                    viewModel.UpdateUIForLoggedInUser();
                }
            };
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

        private void PollsView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
