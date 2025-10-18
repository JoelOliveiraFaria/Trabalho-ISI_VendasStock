using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura��o de servi�os
builder.Services.AddControllers(); // Adiciona suporte para controladores API
builder.Services.AddEndpointsApiExplorer(); // Habilita explora��o de endpoints
builder.Services.AddSwaggerGen(); // Adiciona documenta��o Swagger

// Configura��o da base de dados SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Configura��o CORS para frontend React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173") // Portas React/Vite
              .AllowAnyMethod() // Permite GET, POST, PUT, DELETE
              .AllowAnyHeader(); // Permite qualquer cabe�alhos
    });
});

var app = builder.Build();

// Pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Ativa Swagger UI em desenvolvimento
    app.UseSwaggerUI();
}

app.UseCors("AllowReact"); // Aplica pol�tica CORS
app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
app.UseAuthorization(); // Middleware de autoriza��o
app.MapControllers(); // Mapeia os controladores

app.Run(); // Inicia a aplica��o
