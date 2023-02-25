using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using TimejApi.Data.Entities;
using System.Data.Common;
using System.Reflection;
using TimejApi.Data;
using TimejApi.Data.Mapping;
using TimejApi.Helpers;
using TimejApi.Services;
using TimejApi.Services.Auth;
using TimejApi.Services.Auth.Policies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IJwtAuthentication, JwtAuthenticationService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new JwtAuthenticationService(builder.Configuration, null).CreateValidationParameters();
    });

builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("ScheduleEditor", configurePolicy =>
    {
        configurePolicy.RequireRole(nameof(User.Role.SCHEDULE_EDITOR), nameof(User.Role.MODERATOR));
        configurePolicy.AddRequirements(new FacultyEditorRequirement());
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Applies custom "Mapster" configuration
MappingConfig.Apply();

var connectionString = builder.GetConnectionString();
builder.Services.AddDbContext<ScheduleDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

var contextOptionsBuilder = new DbContextOptionsBuilder<ScheduleDbContext>().UseNpgsql(connectionString);
using (var context = new ScheduleDbContext(contextOptionsBuilder.Options))
{
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
