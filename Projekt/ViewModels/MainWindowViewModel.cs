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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly P4ProjektDbContext _context;

        public MainWindowViewModel(P4ProjektDbContext context)
        {
            _context = context;
        }

        public ObservableCollection<PollModel> Polls
        {
            get => new ObservableCollection<PollModel>(_context.Polls.ToList());
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