using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection;
using TimejApi.Data;
using TimejApi.Data.Mapping;
using TimejApi.Helpers;
using TimejApi.Services;
using TimejApi.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IJwtAuthentication, JwtAuthenticationService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new JwtAuthenticationService(builder.Configuration, null).CreateValidationParameters();
    });

builder.Services.AddAuthorization(options =>
{
    // TODO: Add policies
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Applies custom "Mapster" configuration
MappingConfig.Aplly();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
