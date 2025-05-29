
// ViewModels/MainViewModel.cs  
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Projekt.Models;
using Projekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Projekt.ViewModels
{
    public class MainViewModel(P4ProjektDbContext context) : INotifyPropertyChanged
    {
        private readonly P4ProjektDbContext _context = context;

        public ObservableCollection<PollModel> Polls
        {
            get => new([.. _context.Polls]);
        }

        public void Vote(UserModel user, PollModel poll, OptionModel option)
        {
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}