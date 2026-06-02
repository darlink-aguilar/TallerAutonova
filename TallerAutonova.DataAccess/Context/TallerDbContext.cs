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
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Owner> Owners => Set<Owner>();



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

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Brand)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(v => v.Plate)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(v => v.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(v => v.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(v => v.Year)
                    .IsRequired();

                entity.Property(v => v.State)
                    .IsRequired();

                entity.HasIndex(v => v.Plate)
                    .IsUnique();

                entity.HasOne(v => v.Owner)
                    .WithMany(o => o.Vehicles)
                    .HasForeignKey(v => v.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
                    /*.HasForeignKey(v => v.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict)*/;

                entity.HasIndex(p => new { p.Plate, p.OwnerId })
            .IsUnique();
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.Name)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(o => o.Phone)
                    .IsRequired()
                    .HasMaxLength(20);
            });

        }
    }
}