using EstacionamentoV2;
using EstacionamentoV2.Business;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Context;
using EstacionamentoV2.Repository;
using EstacionamentoV2.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.Development.json", optional: false)
        .Build();
var conection = config.GetConnectionString("Default");
builder.Services.AddDbContext<EstacionamentoContext>
    (options => options.UseMySql(conection, ServerVersion.AutoDetect(conection)));

// Escoped
builder.Services.AddScoped<PatioBusiness>();
builder.Services.AddScoped<PatioRepository>();
builder.Services.AddScoped<VeiculoBusiness>();
builder.Services.AddScoped<VeiculoRepository>();
builder.Services.AddScoped<RegistroVeiculoBusiness>();
builder.Services.AddScoped<RegistroVeiculoRepository>();

// Transiente
builder.Services.AddTransient<IPatioBusiness, PatioBusiness>();
builder.Services.AddTransient<IPatioRepository, PatioRepository>();
builder.Services.AddTransient<IVeiculoBusiness, VeiculoBusiness>();
builder.Services.AddTransient<IVeiculoRepository, VeiculoRepository>();
builder.Services.AddTransient<IRegistroVeiculoBusiness, RegistroVeiculoBusiness>();
builder.Services.AddTransient<IRegistroVeiculoRepository, RegistroVeiculoRepository>();

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

app.Run();
