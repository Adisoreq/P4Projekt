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
            DataContext = new PollsListViewModel(PollService.Instance);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem != null)
            {
                if (listView.SelectedItem is PollModel poll)
                {
                    // Create and show PollDetailsView with the selected poll
                    var detailsView = new PollDetailsView(poll);
                    detailsView.ShowDialog();
                }
                else if (listView.SelectedItem is PollListItemViewModel viewModel)
                {
                    // If using view models, extract the Poll property
                    var detailsView = new PollDetailsView(viewModel.Poll);
                    detailsView.ShowDialog();
                }
            }
        }
    }
}