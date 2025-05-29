using System;
using System.Windows;
using System.Windows.Controls;
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
            var vm = new LoginViewModel();
            vm.RequestClose += () => { DialogResult = true; Close(); };
            DataContext = vm;
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
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}