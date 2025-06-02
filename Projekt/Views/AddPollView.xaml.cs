using System;
using System.Windows;
using Projekt.Data;
using Projekt.Services;
using Projekt.ViewModels;

namespace Projekt.Views
{
    public partial class AddPollView : Window
    {
        public event EventHandler? PollAdded;

        public AddPollView()
        {
            InitializeComponent();
            DataContext = new AddPollViewModel(AppService.DbContext);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddPollViewModel viewModel)
            {
                viewModel.AddPollCommand.Execute(null);
            }
        }

        private void ViewModel_PollAdded(object sender, EventArgs e)
        {
            PollAdded?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}