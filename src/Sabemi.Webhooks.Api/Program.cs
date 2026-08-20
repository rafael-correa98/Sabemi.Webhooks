using Microsoft.EntityFrameworkCore;
using Sabemi.Webhooks.Application.Interfaces;
using Sabemi.Webhooks.Application.Services;
using Sabemi.Webhooks.Infrastructure.Persistence;
using Sabemi.Webhooks.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

// Injeção de dependência - camada de aplicação
builder.Services.AddScoped<IPagamentoWebhookService, PagamentoWebhookService>();

// Injeção de dependência - camada de infraestrutura
builder.Services.AddScoped<IEventoBrutoRepository, EventoBrutoRepository>();
builder.Services.AddScoped<IStatusContratoRepository, StatusContratoRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();