using Microsoft.EntityFrameworkCore;
using WebApiRoulette.Context;
using WebApiRoulette.Interfaces;
using WebApiRoulette.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>   options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy", configurePolicy: policyBuilder =>
     {
         policyBuilder.WithOrigins("http://localhost:5173");
         policyBuilder.AllowAnyHeader();
         policyBuilder.AllowAnyMethod();
         policyBuilder.AllowCredentials();
     }
     );
});

// Register the RouletteService
builder.Services.AddScoped<IRouletteService, RouletteService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("MyPolicy");

app.Run();
