using Projekt.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Projekt.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new RelayCommand(Login));
            }
        }

        private ICommand _registerCommand;
        public ICommand RegisterCommand
        {
            get
            {
                return _registerCommand ?? (_registerCommand = new RelayCommand(Register));
            }
        }

        private ICommand _passwordChangedCommand;
        public ICommand PasswordChangedCommand
        {
            get
            {
                return _passwordChangedCommand ?? (_passwordChangedCommand = new RelayCommand<object>(PasswordChanged));
            }
        }

        public event EventHandler LoginSucceeded;

        public LoginViewModel()
        {
        }

        private void Login(object parameter)
        {
            // In a real application, this would validate against a database
            if (Username == "admin" && Password == "admin")
            {
                // Store user details in the singleton
                UserSession.Instance.Login(Username, "Admin", 1);

                ErrorMessage = "";
                LoginSucceeded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ErrorMessage = "Nieprawidłowy login lub hasło.";
            }
        }

        private void Register(object parameter)
        {
            // Registration logic would go here
            // For now, just show a placeholder message
            ErrorMessage = "Funkcja rejestracji będzie dostępna wkrótce.";
        }

        private void PasswordChanged(object passwordBox)
        {
            var passwordBoxInstance = passwordBox as System.Windows.Controls.PasswordBox;
            if (passwordBoxInstance != null)
            {
                Password = passwordBoxInstance.Password;
            }
        }

        private ICommand _loginSucceededCommand;
        public ICommand LoginSucceededCommand
        {
            get
            {
                return _loginSucceededCommand ?? (_loginSucceededCommand = new RelayCommand(OnLoginSucceeded));
            }
        }

        private void OnLoginSucceeded(object? parameter)
        {
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }
    }
}