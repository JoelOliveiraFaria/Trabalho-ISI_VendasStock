using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddControllers(); // Adiciona suporte para controladores API
builder.Services.AddEndpointsApiExplorer(); // Habilita exploração de endpoints
builder.Services.AddSwaggerGen(); // Adiciona documentação Swagger

// Configuração da base de dados SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Configuração CORS para frontend React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173") // Portas React/Vite
              .AllowAnyMethod() // Permite GET, POST, PUT, DELETE
              .AllowAnyHeader(); // Permite qualquer cabeçalhos
    });
});

var app = builder.Build();

// Pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Ativa Swagger UI em desenvolvimento
    app.UseSwaggerUI();
}

app.UseCors("AllowReact"); // Aplica política CORS
app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
app.UseAuthorization(); // Middleware de autorização
app.MapControllers(); // Mapeia os controladores

app.Run(); // Inicia a aplicação
