using System.Text;
using EstacionamentoV2;
using EstacionamentoV2.Business;
using EstacionamentoV2.Business.Interface;
using EstacionamentoV2.Context;
using EstacionamentoV2.Repository;
using EstacionamentoV2.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
    (options => options.UseNpgsql(conection));

// Escoped
builder.Services.AddScoped<PatioBusiness>();
builder.Services.AddScoped<PatioRepository>();
builder.Services.AddScoped<VeiculoBusiness>();
builder.Services.AddScoped<VeiculoRepository>();
builder.Services.AddScoped<RegistroVeiculoBusiness>();
builder.Services.AddScoped<RegistroVeiculoRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<UsuarioBusiness>();
builder.Services.AddScoped<TokenBusiness>();

// Transiente
builder.Services.AddTransient<IPatioBusiness, PatioBusiness>();
builder.Services.AddTransient<IPatioRepository, PatioRepository>();
builder.Services.AddTransient<IVeiculoBusiness, VeiculoBusiness>();
builder.Services.AddTransient<IVeiculoRepository, VeiculoRepository>();
builder.Services.AddTransient<IRegistroVeiculoBusiness, RegistroVeiculoBusiness>();
builder.Services.AddTransient<IRegistroVeiculoRepository, RegistroVeiculoRepository>();
builder.Services.AddTransient<IUsuarioBusiness, UsuarioBusiness>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<ITokenBusiness, TokenBusiness>();

var keyS = Encoding.ASCII.GetBytes(Key.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyS),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


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
