using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using TimejApi.Data.Entities;
using System.Reflection;
using TimejApi.Data;
using TimejApi.Data.Mapping;
using TimejApi.Helpers;
using TimejApi.Services;
using TimejApi.Services.Auth;
using TimejApi.Services.Auth.Policies;
using TimejApi.Services.User;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IJwtAuthentication, JwtAuthenticationService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();

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

    // TODO ���������� �� ���������� �������� 
    // ��� ���������� �����, �� ������ ������� �� ���, ���� ��� ���� ���������
    // ����� �����  https://stackoverflow.com/questions/43447688/setting-up-swagger-asp-net-core-using-the-authorization-headers-bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token for authentification",
        Name = "Auth",
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
            new string[] {}
        }
    });
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
    options.ClaimsIdentity.RoleClaimType = IJwtAuthentication.RoleClaimType;
});

// Applies custom "Mapster" configuration
MappingConfig.Apply();

var connectionString = builder.GetConnectionString();
builder.Services.AddDbContext<ScheduleDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.UseExceptionProcessor();
});

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
