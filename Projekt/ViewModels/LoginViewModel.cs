using Lab11.ViewModels;
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

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public event EventHandler LoginSucceeded;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
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
    }
}