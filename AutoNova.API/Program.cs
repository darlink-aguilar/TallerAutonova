using System.Text;
using AutoNova.DataAccess.Context;
using AutoNova.DataAccess.Repositories;
using AutoNova.Domain.Interfaces.Repositories;
using AutoNova.Domain.Interfaces.Services;
using AutoNova.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository,               UserRepository>();
builder.Services.AddScoped<IVehicleRepository,            VehicleRepository>();
builder.Services.AddScoped<IOwnerRepository,              OwnerRepository>();
builder.Services.AddScoped<IAppointmentRepository,        AppointmentRepository>();
builder.Services.AddScoped<ISparePartRepository,          SparePartRepository>();
builder.Services.AddScoped<IMaintenanceHistoryRepository, MaintenanceHistoryRepository>();

// ── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService,               AuthService>();
builder.Services.AddScoped<IUserService,               UserService>();
builder.Services.AddScoped<IVehicleService,            VehicleService>();
builder.Services.AddScoped<IOwnerService,              OwnerService>();
builder.Services.AddScoped<IAppointmentService,        AppointmentService>();
builder.Services.AddScoped<ISparePartService,          SparePartService>();
builder.Services.AddScoped<IMaintenanceHistoryService, MaintenanceHistoryService>();

// ── AutoMapper ───────────────────────────────────────────────────────────────
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
        opts.JsonSerializerOptions.PropertyNamingPolicy      = System.Text.Json.JsonNamingPolicy.CamelCase;
        opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// ── CORS (React frontend) ─────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ── Swagger ───────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "AutoNova API",
        Version     = "v1",
        Description = "Backend del sistema de gestión Taller AutoNova"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description  = "JWT Authorization header. Formato: 'Bearer {token}'",
        Name         = "Authorization",
        In           = ParameterLocation.Header,
        Type         = SecuritySchemeType.ApiKey,
        Scheme       = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoNova API v1"));
}

app.UseHttpsRedirection();
app.UseCors("ReactFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
