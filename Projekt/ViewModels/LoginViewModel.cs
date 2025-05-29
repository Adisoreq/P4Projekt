using Projekt.Models;
using System;
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
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand PasswordChangedCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action? RequestClose;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
            PasswordChangedCommand = new RelayCommand<object>(PasswordChanged);
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        private void Login(object? parameter)
        {
            if (Username == "admin" && Password == "admin")
            {
                UserSession.Instance.Login(Username, "Admin", 1);
                ErrorMessage = "";
                RequestClose?.Invoke();
            }
            else
            {
                ErrorMessage = "Nieprawidłowy login lub hasło.";
            }
        }

        private void Register(object? parameter)
        {
            ErrorMessage = "Funkcja rejestracji będzie dostępna wkrótce.";
        }

        private void PasswordChanged(object? passwordBox)
        {
            if (passwordBox is System.Windows.Controls.PasswordBox pb)
                Password = pb.Password;
        }
    }
}