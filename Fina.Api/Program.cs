using Fina.Api.Accounts.Endpoints;
using Fina.Api.Categories.Endpoints;
using Fina.Api.Common.Data;
using Fina.Api.Common.Extensions;
using Fina.Api.Common.Models;
using Fina.Api.Transactions.Endpoints;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config
        .MinimumLevel.Information()
        .WriteTo.Console();
});

builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddApplicationServices();

// Add services to the container.
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

app.UseSerilogRequestLogging();

app.MapIdentityApi<User>();

app.MapEndpoints();

app.Run();