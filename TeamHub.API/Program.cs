using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TeamHub.Application.Interfaces;
using TeamHub.Application.Services;
using TeamHub.Domain.Entities;
using TeamHub.Infrastructure.Data;
using TeamHub.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Bind Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("Cors"));
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
});
// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Cors
var corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins(corsSettings.AllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await Seed.SeedRolesAndAdminUser(services);
}

// Serve React
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");
app.Run();