using System.Collections.Generic;
using System.Windows.Controls;
using Projekt.Models;
using Projekt.ViewModels;
using System.Windows;

namespace Projekt.Views
{
    public partial class PollsListView : UserControl
    {
        public event EventHandler<RoutedEventArgs> PollSelected;

        public PollsListView()
        {
            InitializeComponent();
            DataContext = new PollsListViewModel();
        }
    }
}