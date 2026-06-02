using Microsoft.EntityFrameworkCore;
using TallerAutonova.DataAccess.Context;
using TallerAutonova.DataAccess.Repositories;
using TallerAutonova.Domain.Interfaces.Repositories;
using TallerAutonova.Domain.Interfaces.Services;
using TallerAutonova.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Entity Framework Core ──
builder.Services.AddDbContext<TallerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMechanicRepository, MechanicRepository>();
builder.Services.AddScoped<IVehiclesRepository, VehicleRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();


// ── Services ──
builder.Services.AddScoped<IMechanicService, MechanicService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();


// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ── Controllers ──
builder.Services.AddControllers();

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Middleware Pipeline ──
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();