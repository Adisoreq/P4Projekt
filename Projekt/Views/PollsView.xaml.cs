using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Projekt.Models;
using Projekt.Services;
using Projekt.Data;
using Projekt.ViewModels;

namespace Projekt.Views
{
    public partial class PollsView : UserControl
    {
        private readonly PollService? _pollService;

        // Add the event to propagate from the internal PollsListView
        public event RoutedEventHandler? PollSelected;

        public PollsView()
        {
            InitializeComponent();
            DataContext = new PollsViewModel();
        }

        public PollsView(PollService pollService)
        {
            InitializeComponent();
            DataContext = new PollsViewModel(pollService);
        }

        private void PollsListViewControl_PollSelected(object sender, RoutedEventArgs e)
        {
            PollSelected?.Invoke(sender, e);
        }
    }
}