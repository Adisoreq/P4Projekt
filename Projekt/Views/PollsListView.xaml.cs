using System.Collections.Generic;
using System.Windows.Controls;
using Projekt.Models;
using Projekt.ViewModels;
using System.Windows;
using Projekt.Services;
using Projekt.Data;
using System.Windows.Input;

namespace Projekt.Views
{
    public partial class PollsListView : UserControl
    {
        public event EventHandler<RoutedEventArgs> PollSelected;

        public PollsListView()
        {
            InitializeComponent();
            var dbContext = new P4ProjektDbContext();
            var pollService = new PollService(dbContext);
            DataContext = new PollsListViewModel(pollService);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem != null)
            {
                MessageBox.Show($"You double-clicked on: {listView.SelectedItem}");
            }
        }
    }
}