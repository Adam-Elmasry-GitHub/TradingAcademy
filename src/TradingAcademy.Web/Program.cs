using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TradingAcademy.Web.Components;
using TradingAcademy.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<CurriculumService>();
builder.Services.AddScoped<ProgressService>();
builder.Services.AddScoped<SimulatorSession>();

await builder.Build().RunAsync();
