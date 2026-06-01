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

        public DbSet<Mechanic> Mechanics => Set<Mechanic>();
        public DbSet<Receptionist> Receptionists => Set<Receptionist>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Mechanic Configuration ──
            modelBuilder.Entity<Mechanic>(entity =>
            {
                entity.HasKey(m => m.Id); 
                entity.Property(m => m.Name)
                      .IsRequired() 
                      .HasMaxLength(100); 
                entity.Property(m => m.Email)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(m => m.Password)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(m => m.Role)
                      .IsRequired();
                entity.Property(m => m.CreatedAt)
                      .IsRequired();
                entity.Property(m=> m.UpdatedAt)
                      .IsRequired(false);

                // Relación 1:N con Administrator
                //entity.HasOne(m => m.Administrator) // Un mecanico fue creado por un administrador
                //      .WithMany(a => a.Users) // Un administrador crea muchos usuarios
                //      .HasForeignKey(m => m.AdministratorId) // La clave foránea en la tabla de usuario
                //      .OnDelete(DeleteBehavior.Cascade); 
            });


            // ── Receptionist Configuration ──
            modelBuilder.Entity<Receptionist>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(r => r.Email)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(r => r.Password)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(r => r.Role)
                      .IsRequired();
                entity.Property(r => r.CreatedAt)
                      .IsRequired();
                entity.Property(r => r.UpdatedAt)
                      .IsRequired(false);

                // Relación 1:N con Administrator
                //entity.HasOne(r => r.Administrator) // Un mecanico fue creado por un administrador
                //      .WithMany(a => a.Users) // Un administrador crea muchos usuarios
                //      .HasForeignKey(r => r.AdministratorId) // La clave foránea en la tabla de usuario
                //      .OnDelete(DeleteBehavior.Cascade); 
            });

        }
    }
}