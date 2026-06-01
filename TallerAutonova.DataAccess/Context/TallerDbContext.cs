using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TallerAutonova.Domain.Entities;
using System.Numerics;
using System.Text.RegularExpressions;

namespace TallerAutonova.DataAccess.Context
{
    public class TallerDbContext : DbContext
    {
        public TallerDbContext(DbContextOptions<TallerDbContext> options)
            : base(options)
        {
        }

        //public DbSet<Team> Teams => Set<Team>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
    }
}