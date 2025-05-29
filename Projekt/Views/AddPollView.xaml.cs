using System;
using System.Windows;
using Projekt.ViewModels;

namespace Projekt.Views
{
    public partial class AddPollView : Window
    {
        public event EventHandler? PollAdded;

        public AddPollView()
        {
            InitializeComponent();
            var vm = new AddPollViewModel();
            vm.RequestClose += () => Close();
            DataContext = vm;
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