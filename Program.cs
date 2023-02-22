using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using TimejApi.Data;
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
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");
var contextOptions = new DbContextOptionsBuilder<ScheduleDbContext>().UseNpgsql(connectionString).Options;
builder.Services.AddDbContext<ScheduleDbContext>(options=>options.UseNpgsql(connectionString));
var app = builder.Build();
using (var context = new ScheduleDbContext(contextOptions))
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
