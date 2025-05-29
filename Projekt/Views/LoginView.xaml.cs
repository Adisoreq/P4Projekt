using System;
using System.Windows;
using System.Windows.Controls;
using Projekt.Models;
using Projekt.ViewModels;

namespace Projekt.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
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