using Projekt.Models;
using System;
using System.Windows.Input;
using System.Linq;
using Projekt.Data;
using Projekt.Services;
using System.Diagnostics;

namespace Projekt.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly P4ProjektDbContext _dbContext;
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool? _dialogResult;

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

        public bool? DialogResult
        {
            get => _dialogResult;
            set { _dialogResult = value; OnPropertyChanged(); }
        }

        public bool IsLoggedIn => UserSession.Instance.IsLoggedIn;
        public bool IsNotLoggedIn => !UserSession.Instance.IsLoggedIn;
        public string LoggedInUsername => UserSession.Instance.Username;
        public string LoggedInEmail => UserSession.Instance.Email;

        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand PasswordChangedCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand LogoutCommand { get; set; }


        public LoginViewModel()
        {
            ErrorMessage = string.Empty;
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
            PasswordChangedCommand = new RelayCommand<object>(PasswordChanged);
            CloseCommand = new RelayCommand(_ => DialogResult = true);
            LogoutCommand = new RelayCommand(Logout);
        }

        public LoginViewModel(P4ProjektDbContext dbContext)
        {
            _dbContext = dbContext;
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
            PasswordChangedCommand = new RelayCommand<object>(PasswordChanged);
            CloseCommand = new RelayCommand(_ => DialogResult = true);
            LogoutCommand = new RelayCommand(Logout);
        }

        private void Login(object? parameter)
        {
            var user = UserService.LogInAsUser(Username, Password);
            
            if (user != null)
            {
                Debug.WriteLine($"Zalogowano jako: {user.Name}");

                UserSession.Instance.Login(Username, Password);
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(IsNotLoggedIn));
                OnPropertyChanged(nameof(LoggedInUsername));
                OnPropertyChanged(nameof(LoggedInEmail));
                DialogResult = true;
                ErrorMessage = string.Empty;
            }
            else
            {
                Debug.WriteLine($"Błąd: Nieznany użytkownik");

                ErrorMessage = "Nieprawidłowy login lub hasło.";
                OnPropertyChanged(nameof(ErrorMessage));
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

        private void Logout(object? parameter)
        {
            UserSession.Instance.Logout();
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsNotLoggedIn));
            OnPropertyChanged(nameof(LoggedInUsername));
            OnPropertyChanged(nameof(LoggedInEmail));
            DialogResult = false;
        }
    }
}