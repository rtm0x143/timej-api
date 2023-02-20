using Microsoft.AspNetCore.Authentication.JwtBearer;
using TimejApi.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.UseRouting()
    .UseAuthentication()
    .UseAuthorization();

// app.MapGrpcService< TestService >();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
