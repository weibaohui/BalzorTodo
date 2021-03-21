using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddAntDesign();
            builder.Services.AddScoped(
                sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
                // sp => new HttpClient { BaseAddress = new Uri("https://127.0.0.1:9001/")});

            builder.Services.AddScoped<TaskDetailServices>();

            builder.Services.AddAuthorizationCore(option =>
            {
                option.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
            });
            builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();

            await builder.Build().RunAsync();
        }
    }
}