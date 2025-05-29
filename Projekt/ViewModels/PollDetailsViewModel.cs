using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Projekt.Models;

namespace Projekt.ViewModels
{
    public class PollDetailsViewModel
    {
        public PollModel Poll { get; }
        public ObservableCollection<OptionModel> Options { get; }
        public OptionModel? SelectedOption { get; set; }
        public ICommand VoteCommand { get; }

        public event EventHandler? VoteCompleted;

        public PollDetailsViewModel(PollModel poll)
        {
            Poll = poll;
            Options = new ObservableCollection<OptionModel>(poll.Options);
            VoteCommand = new RelayCommand(Vote);
        }

        private void Vote(object? parameter)
        {
            if (SelectedOption != null)
            {
                // Actual voting logic
                MessageBox.Show($"Oddano głos na: {SelectedOption.Text}");
                CompleteVote();
            }
            else
            {
                MessageBox.Show("Wybierz opcję!");
            }
        }

        public void CompleteVote()
        {
            VoteCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}