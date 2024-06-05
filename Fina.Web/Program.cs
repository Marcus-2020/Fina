using Fina.Core;
using Fina.Core.Categories.Handlers;
using Fina.Core.Transactions.Handlers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Fina.Web;
using Fina.Web.Features.Categories.Handlers;
using Fina.Web.Features.Transactions.Handlers;
using Fina.Web.Web.Extensions;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.AddConfiguration();

builder.Services.AddMudServices();

builder.Services.AddHttpClient(
    WebConfiguration.HttpClientName,
    opt =>
    {
        opt.BaseAddress = new Uri(Configuration.BackendUrl);
    });

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

await builder.Build().RunAsync();