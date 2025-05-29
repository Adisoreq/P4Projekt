using System;
using System.Windows;
using Projekt.Models;
using Projekt.ViewModels;

namespace Projekt.Views
{
    public partial class LoginView : Window
    {
        public event EventHandler LoginSucceeded;

        public LoginView()
        {
            InitializeComponent();
            
            // Make sure we're using the LoginViewModel
            DataContext = new LoginViewModel();
            ((LoginViewModel)DataContext).LoginSucceeded += LoginView_LoginSucceeded;
        }

        private void LoginView_LoginSucceeded(object sender, EventArgs e)
        {
            this.DialogResult = true;
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).LoginCommand.Execute(null);
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).RegisterCommand.Execute(null);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoginViewModel)DataContext;
            viewModel.PasswordChangedCommand.Execute(PasswordBox);
        }
    }
}