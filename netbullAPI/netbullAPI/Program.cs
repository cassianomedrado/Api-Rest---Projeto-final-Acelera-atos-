using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using netbullAPI.Interfaces;
using netbullAPI.MidwareDB;
using netbullAPI.Persistencia;
using netbullAPI.Repository;
using netbullAPI.Security.MidwareDB;
using netbullAPI.Security.Persistencia;
using netbullAPI.Security.Service;
using netbullAPI.Util;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("TokenConfigurations").GetSection("JwtKey").Value);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.OperationFilter<AuthResponsesOperationFilter>();

    s.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "NetBullAPI", 
        Version = "v1",
        Description = "ASP.NET Core Web API para controle de pedidos e clientes",
        TermsOfService = new Uri("https://example.com/terms")
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    s.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"Cabeçalho de autorização JWT usando o esquema Bearer.
                        Digite 'Bearer' [espaço] e então seu token na entrada de texto abaixo.
                        Exemplo:'Bearer 12345abcdef' "
    });
});

builder.Services.AddDbContext<netbullDBContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("NetBullConnection"));
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        builder => builder.WithOrigins("https://localhost:4200", "http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod());
});

builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearerOptions =>
{
    bearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

// INJEÇÃO DE DEPENDENCIAS

builder.Services.AddScoped<NE_User>();
builder.Services.AddScoped<NE_Endereco>();
builder.Services.AddScoped<NE_Telefone>();
builder.Services.AddScoped<NE_Produto>();
builder.Services.AddScoped<NE_Pedido>();
builder.Services.AddScoped<NE_Pessoa>();
builder.Services.AddScoped<NE_Item>();
builder.Services.AddScoped<INotificador, Notificador>(); // Por Requisição
builder.Services.AddTransient<UserDAO>();
builder.Services.AddTransient<DAOItem>();
builder.Services.AddTransient<DAO_Endereco>();
builder.Services.AddTransient<DAOTelefone>();
builder.Services.AddTransient<DAOPedido>();
builder.Services.AddTransient<DAO_Pessoa>();
builder.Services.AddTransient<DAOProduto>();
builder.Services.AddTransient<TokenService>(); // Por método

var app = builder.Build();

app.UseCors(builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }