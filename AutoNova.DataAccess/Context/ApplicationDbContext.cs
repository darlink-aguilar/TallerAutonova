using AutoNova.Domain.Entities;
using AutoNova.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutoNova.DataAccess.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // ─── DbSets ───────────────────────────────────────────────────────────────
    public DbSet<User>               Users                { get; set; }
    public DbSet<Administrador>      Administradores      { get; set; }
    public DbSet<Mecanico>           Mecanicos            { get; set; }
    public DbSet<Recepcionista>      Recepcionistas       { get; set; }
    public DbSet<Vehicle>            Vehicles             { get; set; }
    public DbSet<Owner>              Owners               { get; set; }
    public DbSet<Appointment>        Appointments         { get; set; }
    public DbSet<SparePart>          SpareParts           { get; set; }
    public DbSet<MaintenanceHistory> MaintenanceHistories { get; set; }
    public DbSet<MecanicoRepuesto>   MecanicoRepuestos    { get; set; }
    public DbSet<MecanicoHistorial>  MecanicoHistoriales  { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── TPH: User (abstracta) → Administrador / Mecanico / Recepcionista ──
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired().HasConversion<int>();
            entity.Property(u => u.IsActive).HasDefaultValue(true);
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("TipoUsuario")
            .HasValue<Administrador>("Administrador")
            .HasValue<Mecanico>("Mecanico")
            .HasValue<Recepcionista>("Recepcionista");

        // ── Vehicle ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Plate).IsRequired().HasMaxLength(20);
            entity.HasIndex(v => v.Plate).IsUnique();
            entity.Property(v => v.Brand).IsRequired().HasMaxLength(100);
            entity.Property(v => v.Model).IsRequired().HasMaxLength(100);
            entity.Property(v => v.Year).IsRequired();
            entity.Property(v => v.Color).HasMaxLength(50);
            entity.Property(v => v.IsActive).HasDefaultValue(true);
            entity.Property(v => v.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // ── Owner ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.FullName).IsRequired().HasMaxLength(200);
            entity.Property(o => o.DocumentNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(o => o.DocumentNumber).IsUnique();
            entity.Property(o => o.Email).HasMaxLength(200);
            entity.Property(o => o.Phone).HasMaxLength(20);
            entity.Property(o => o.Address).HasMaxLength(300);
        });

        // ── Vehicle ↔ Owner (1:1 Cascade) ─────────────────────────────────────
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Owner)
            .WithOne(o => o.Vehicle)
            .HasForeignKey<Owner>(o => o.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Appointment ───────────────────────────────────────────────────────
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Date).IsRequired();
            entity.Property(a => a.Time).IsRequired();
            entity.Property(a => a.Description).HasMaxLength(500);
            entity.Property(a => a.Status).IsRequired().HasConversion<int>();
            entity.Property(a => a.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(a => a.Vehicle)
                  .WithMany(v => v.Appointments)
                  .HasForeignKey(a => a.VehicleId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Mechanic)
                  .WithMany(u => u.Appointments)
                  .HasForeignKey(a => a.MechanicId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // ── SparePart ─────────────────────────────────────────────────────────
        modelBuilder.Entity<SparePart>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Code).IsRequired().HasMaxLength(50);
            entity.HasIndex(s => s.Code).IsUnique();
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.HasIndex(s => s.Name).IsUnique();
            entity.Property(s => s.Quantity).IsRequired();
            entity.Property(s => s.MinimumStock).HasDefaultValue(8);
            entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // ── MaintenanceHistory ────────────────────────────────────────────────
        modelBuilder.Entity<MaintenanceHistory>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Observation).HasMaxLength(1000);
            entity.Property(m => m.ServicePerformed).IsRequired().HasMaxLength(500);
            entity.Property(m => m.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(m => m.Vehicle)
                  .WithMany(v => v.MaintenanceHistories)
                  .HasForeignKey(m => m.VehicleId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Mechanic)
                  .WithMany(u => u.MaintenanceHistories)
                  .HasForeignKey(m => m.MechanicId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── MecanicoRepuesto (Mecánico ↔ Repuesto, intermedia) ───────────────
        modelBuilder.Entity<MecanicoRepuesto>(entity =>
        {
            entity.HasKey(mr => mr.Id);
            entity.Property(mr => mr.Quantity).IsRequired();
            entity.Property(mr => mr.Date).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(mr => mr.Mecanico)
                  .WithMany(m => m.MecanicoRepuestos)
                  .HasForeignKey(mr => mr.MecanicoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(mr => mr.SparePart)
                  .WithMany()
                  .HasForeignKey(mr => mr.SparePartId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── MecanicoHistorial (Mecánico ↔ Historial, M:N) ────────────────────
        modelBuilder.Entity<MecanicoHistorial>(entity =>
        {
            entity.HasKey(mh => new { mh.MecanicoId, mh.MaintenanceHistoryId });
            entity.Property(mh => mh.Date).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(mh => mh.Mecanico)
                  .WithMany(m => m.MecanicoHistoriales)
                  .HasForeignKey(mh => mh.MecanicoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(mh => mh.MaintenanceHistory)
                  .WithMany()
                  .HasForeignKey(mh => mh.MaintenanceHistoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Seed ─────────────────────────────────────────────────────────────
        // Contraseñas: admin@autonova.com/Admin123! | mecanico@autonova.com/Mec123! | recepcionista@autonova.com/Recep123!
        modelBuilder.Entity<Administrador>().HasData(new Administrador
        {
            Id           = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Name         = "Administrador General",
            Email        = "admin@autonova.com",
            PasswordHash = "$2a$11$VKmN.jrudroCnqkEqYDVG.e9OPNerAzec5cFI8bp4W88l4mdcXiv2",
            Role         = UserRole.Administrador,
            IsActive     = true,
            CreatedAt    = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        modelBuilder.Entity<Mecanico>().HasData(new Mecanico
        {
            Id           = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name         = "Carlos Rodríguez",
            Email        = "mecanico@autonova.com",
            PasswordHash = "$2a$11$3U8jTxr6XmssYiO/riCjWeFFYOSzQXO6WGrhcZ4nqYGQhM.hXwl9S",
            Role         = UserRole.Mecanico,
            IsActive     = true,
            CreatedAt    = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        modelBuilder.Entity<Recepcionista>().HasData(new Recepcionista
        {
            Id           = Guid.Parse("00000000-0000-0000-0000-000000000003"),
            Name         = "Ana García",
            Email        = "recepcionista@autonova.com",
            PasswordHash = "$2a$11$nCxsq/a/hXpkhKdJ1pwI.OBI9pOq3TOWze7N09gngdSyrMqgCcdkK",
            Role         = UserRole.Recepcionista,
            IsActive     = true,
            CreatedAt    = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
