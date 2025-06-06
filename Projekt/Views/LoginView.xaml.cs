using Projekt.Models;
using Projekt.Services;
using Projekt.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Projekt.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && vm.PasswordChangedCommand != null)
            {
                vm.PasswordChangedCommand.Execute(sender);
            }
        }
    }
}