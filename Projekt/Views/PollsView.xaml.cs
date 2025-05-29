using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Projekt.Models;
using Projekt.Services;
using Projekt.Data;

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
            
            // Create a new context and pass it to the service
            var dbContext = new P4ProjektDbContext();
            _pollService = new PollService(dbContext);
            
            LoadPolls();
        }

        public PollsView(PollService pollService)
        {
            InitializeComponent();
            _pollService = pollService;
            LoadPolls();
        }

        private void LoadPolls()
        {
            if (_pollService != null)
            {
                var polls = _pollService.GetPolls();
            }
            else
            {
                // If no service is available, load directly from DB
                using var db = new P4ProjektDbContext();
                List<PollModel> polls = db.Polls.ToList();
                PollsListViewControl.SetPolls(polls);
            }
        }

        private void PollsListViewControl_PollSelected(object sender, RoutedEventArgs e)
        {
            // Forward the event to any parent handlers
            PollSelected?.Invoke(sender, e);
        }
    }
}