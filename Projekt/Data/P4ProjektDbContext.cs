using Microsoft.EntityFrameworkCore;
using Projekt.Models;

namespace Projekt.Data
{
    public class P4ProjektDbContext : DbContext
    {
        public P4ProjektDbContext(DbContextOptions<P4ProjektDbContext> options) : base(options) {}

        // Add parameterless constructor for design-time migrations
        public P4ProjektDbContext() {}

        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<OptionModel> Options { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<PollModel> Polls { get; set; }
        public DbSet<VoteModel> Votes { get; set; }

        // Add OnConfiguring for design-time migrations
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=Spectre\\SQLEXPRESS;Initial Catalog=P4Projekt;Integrated Security=True;TrustServerCertificate=True");
            }
        }
    }
}
