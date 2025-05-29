using System.Collections.Generic;
using System.Windows.Controls;
using Projekt.Models;
using System.Windows;

namespace Projekt.Views
{
    public partial class PollsListView : UserControl
    {
        public event EventHandler<RoutedEventArgs> PollSelected;

        public PollsListView()
        {
            InitializeComponent();
            LoadPolls(); // Load polls when the control is created
        }

        public void SetPolls(List<PollModel> polls)
        {
            PollsListBox.ItemsSource = polls;
        }

        private void LoadPolls()
        {
            using var db = new Data.P4ProjektDbContext();
            List<PollModel> polls = db.Polls.ToList();
            PollsListBox.ItemsSource = polls;
        }

        private void PollsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PollSelected?.Invoke(PollsListBox.SelectedItem, new RoutedEventArgs());
        }
    }
}