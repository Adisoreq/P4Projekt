using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Projekt.Data;
using Projekt.Models;
using Projekt.Services;
using Projekt.ViewModels;

namespace Projekt.Views
{
    public partial class AddPollView : Window
    {
        public event EventHandler? PollAdded;
        private bool _initializationCompleted = false;

        public AddPollView()
        {
            InitializeComponent();
            
            if (!UserSession.Instance.IsLoggedIn)
            {
                if (!ShowLoginAndContinue())
                {
                    return;
                }
            }
            
            InitializeViewModel();
        }

        private bool ShowLoginAndContinue()
        {
            // Create and show login view
            var loginView = new LoginView();
            var loginViewModel = new LoginViewModel();
            loginView.DataContext = loginViewModel;
            
            bool? result = loginView.ShowDialog();
            
            return result == true && UserSession.Instance.IsLoggedIn;
        }
        
        private void InitializeViewModel()
        {
            DataContext = new AddPollViewModel();
            
            if (DataContext is AddPollViewModel viewModel)
            {
                viewModel.PollAdded += ViewModel_PollAdded;
                viewModel.RequestClose += () => this.Close();
            }
            
            _initializationCompleted = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddPollViewModel viewModel)
            {
                viewModel.AddPollCommand.Execute(null);
            }
        }

        private void ViewModel_PollAdded()
        {
            PollAdded?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Option_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.DataContext is OptionModel option)
            {
                if (DataContext is AddPollViewModel viewModel)
                {
                    viewModel.RemoveOptionCommand.Execute(option);
                    e.Handled = true; // Mark the event as handled
                }
            }
        }
        
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            
            // If initialization wasn't completed (login failed/canceled), close the window
            if (!_initializationCompleted)
            {
                this.Close();
            }
        }
    }
}