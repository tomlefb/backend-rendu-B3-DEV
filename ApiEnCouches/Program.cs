using System.Text;
using ApiEnCouches.DataAccess;
using ApiEnCouches.DataAccess.DataAccess;
using ApiEnCouches.DataAccess.IDataAccess;
using ApiEnCouches.DataAccess.Seed;
using ApiEnCouches.Service.IService;
using ApiEnCouches.Service.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<ISeeder, UserSeeder>();
builder.Services.AddScoped<ISeeder, MeetingRoomSeeder>();
builder.Services.AddScoped<ISeeder, ReservationSeeder>();
builder.Services.AddScoped<DbSeeder>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<IMeetingRoomService, MeetingRoomsService>();
builder.Services.AddScoped<IReservationService, ReservationsService>();

builder.Services.AddScoped<IUsers, UserDataAccess>();
builder.Services.AddScoped<IMeetingRooms, MeetingRoomDataAccess>();
builder.Services.AddScoped<IReservations, ReservationDataAccess>();
builder.Services.AddScoped<IRefreshTokenDataAccess, RefreshTokenDataAccess>();

var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
    };
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString)
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    var isProduction = app.Environment.IsProduction();

    db.Database.Migrate();
    await seeder.Execute(db, isProduction);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
