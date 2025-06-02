using Projekt.Services;
using System;

namespace Projekt.Models
{
    public class UserSession
    {
        private static readonly UserSession _instance = new UserSession();

        public static UserSession Instance
        {
            get
            {
                return _instance;
            }
        }

        public UserModel? User { get; set; }
        public bool IsLoggedIn { get; private set; }
        public string Username 
        {
            get { 
                if (User != null) return User.Name;
                else return string.Empty;
            } 
        }
        public int UserId 
        {
            get
            {
                if (User != null) return User.Id;
                else return -1;
            }
        }
        public string Email 
        {
            get
            {
                if (User != null) return User.Email;
                else return string.Empty;
            }
        }

        private UserSession()
        {
            IsLoggedIn = false;
            User = null;
        }

        public void Login(string username, string password)
        {
            UserModel? LoginUser = UserService.LogInAsUser(username, password);
            
            if (LoginUser == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            } 
            else
            {
                User = LoginUser;
                IsLoggedIn = true;
            }         
        }

        public void Logout()
        {
            IsLoggedIn = false;
            User = null;
        }
    }
}