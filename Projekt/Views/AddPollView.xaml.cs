using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Projekt.Data;
using Projekt.Models;
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
            DataContext = new AddPollViewModel();
            
            // Subscribe to the view model's PollAdded event
            if (DataContext is AddPollViewModel viewModel)
            {
                viewModel.PollAdded += ViewModel_PollAdded;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddPollViewModel viewModel)
            {
                viewModel.AddPollCommand.Execute(null);
            }
        }

        private void ViewModel_PollAdded()
        {
            PollAdded?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Option_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.DataContext is OptionModel option)
            {
                if (DataContext is AddPollViewModel viewModel)
                {
                    viewModel.RemoveOptionCommand.Execute(option);
                    e.Handled = true; // Mark the event as handled
                }
            }
        }
    }
}