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
            DataContext = new PollDetailsViewModel(poll);
        }
    }
}