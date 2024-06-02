using Fina.Api.Common;
using Fina.Api.Common.Endpoints;
using Fina.Api.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurations();
builder.AddLogger();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
    app.ConfigureDevEnviroment();

app.UseCors(ApiConfiguration.CorsPolicyName);
app.MapEndpoints();

app.Run();