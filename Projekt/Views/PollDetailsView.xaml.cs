using System;
using System.Windows;
using Projekt.Models;
using Projekt.ViewModels;

namespace Projekt.Views
{
    public partial class PollDetailsView : Window
    {
        public PollDetailsView(PollModel poll)
        {
            InitializeComponent();
            var viewModel = new PollDetailsViewModel(poll);
            viewModel.VoteCompleted += ViewModel_VoteCompleted;
            DataContext = viewModel;
        }

        private void Vote_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PollDetailsViewModel viewModel)
            {
                viewModel.SelectedOption = OptionsList.SelectedItem as OptionModel;
                viewModel.VoteCommand.Execute(null);
            }
        }

        private void ViewModel_VoteCompleted(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}