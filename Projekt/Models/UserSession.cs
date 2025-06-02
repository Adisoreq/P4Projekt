using System;

namespace Projekt.Models
{
    public class UserSession
    {
        private static UserSession _instance = new();
        private static readonly object _lock = new object();

        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserSession();
                        }
                    }
                }
                return _instance;
            }
        }

        public bool IsLoggedIn { get; private set; }
        public string Username { get; private set; }
        public string UserRole { get; private set; }
        public int UserId { get; private set; }
        public string Email { get; private set; }

        private UserSession()
        {
            IsLoggedIn = false;
            Username = string.Empty;
            UserRole = string.Empty;
            Email = string.Empty;
        }

        public void Login(string username, string role, int id, string email)
        {
            Username = username;
            UserRole = role;
            UserId = id;
            Email = email;
            IsLoggedIn = true;
        }

        public void Logout()
        {
            IsLoggedIn = false;
            Username = string.Empty;
            UserRole = string.Empty;
            UserId = 0;
            Email = string.Empty;
        }
    }
}