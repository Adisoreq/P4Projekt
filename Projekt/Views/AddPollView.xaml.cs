using System;
using System.Windows;
using Projekt.Models;

namespace Projekt.Views
{
    public partial class AddPollView : Window
    {
        public event EventHandler? PollAdded;

        public AddPollView()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Dodaj ankietę do kolekcji (lub bazy danych)
            PollAdded?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}