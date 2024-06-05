using Fina.Core;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Fina.Web.Web.Extensions;

public static class BuildExtensions
{
    public static void AddConfiguration(this WebAssemblyHostBuilder builder)
    {
        Configuration.FrontendUrl = "http://localhost:5234";
        Configuration.BackendUrl = "http://localhost:5096";
    }
}