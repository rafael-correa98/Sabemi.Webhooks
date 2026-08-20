using Microsoft.EntityFrameworkCore;
using Sabemi.Webhooks.Api.Filters;
using Sabemi.Webhooks.Application.Interfaces;
using Sabemi.Webhooks.Application.Services;
using Sabemi.Webhooks.Infrastructure.Persistence;
using Sabemi.Webhooks.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddScoped<IPagamentoWebhookService, PagamentoWebhookService>();
builder.Services.AddScoped<IEventoBrutoRepository, EventoBrutoRepository>();
builder.Services.AddScoped<IStatusContratoRepository, StatusContratoRepository>();

// Filtro de segurança
builder.Services.AddScoped<ApiKeyAuthFilter>();

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