using System;
using System.Windows;
using Projekt.Models;

namespace Projekt.Views
{
    public partial class PollDetailsView : Window
    {
        private PollModel poll;
        public event EventHandler? VoteCompleted;

        public PollDetailsView(PollModel poll)
        {
            InitializeComponent();
            this.poll = poll;
            PollNameText.Text = poll.Name;
            PollDescText.Text = poll.Description;
            OptionsList.ItemsSource = poll.Options;
        }

        private void Vote_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsList.SelectedItem is OptionModel option)
            {
                MessageBox.Show($"Oddano głos na: {option.Text}");
                VoteCompleted?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            else
            {
                MessageBox.Show("Wybierz opcję!");
            }
        }
    }
}