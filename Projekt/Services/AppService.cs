using Microsoft.EntityFrameworkCore;
using Projekt.Data;

namespace Projekt.Services
{
    public class AppService : IService
    {
        private static readonly P4ProjektDbContext _dbContext = new P4ProjektDbContext();
        private static readonly AppService _instance = new AppService();

        public AppService()
        {
            
        }

        public static IService Instance { get { return _instance; } }

        public static bool IsValid()
        {
            return _instance != null;
        }

        public static P4ProjektDbContext DbContext { get { return _dbContext; } }
    }
}
