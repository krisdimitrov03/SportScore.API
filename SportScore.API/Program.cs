using Microsoft.AspNetCore.Authentication.JwtBearer;
using SportScore.API.SportsDataOperators.Contracts;
using SportScore.API.SportsDataOperators.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportScore.Infrastructure.Data;
using SportScore.Infrastructure;
using SportScore.Infrastructure.Seeders;
using SportScore.Infrastructure.Data.Repositories;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
var keyVaultEndpoint = new Uri("https://sportscoreapivault.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
var connectionString = builder.Configuration.GetConnectionString("SportScoreContextConnection") ?? throw new InvalidOperationException("Connection string 'SportScoreContextConnection' not found.");

builder.Services.AddDbContext<SportScoreContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<SportScoreContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
});

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
    //.AddJwtBearer(options =>
    //{
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidateLifetime = true,
    //        ValidateIssuerSigningKey = true,
    //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
    //        ValidAudience = builder.Configuration["Jwt:Audience"],
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    //    };
    //});
    //.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services
    .AddScoped<IApplicationDbRepository, ApplicationDbRepository>()
    .AddScoped<ISportDataService, SportDataService>()
    .AddScoped<IFootballService, FootballService>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IFavoritesService, FavoritesService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SportScore", builder =>
    {
        //builder.WithOrigins(Environment.GetEnvironmentVariable("ACCESS_URL"))
        //builder.WithOrigins("http://localhost:3000")
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("SportScore");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Seeder.Seed(app);

app.Run();