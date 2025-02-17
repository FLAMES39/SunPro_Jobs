using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SunPro_Jobs;
using SunPro_Jobs.Service;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7068") });

builder.Services.AddScoped<JobService>();
builder.Services.AddScoped<userServices>();
builder.Services.AddScoped<ApplicationService>();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF1cXmhLYVJ+WmFZfVtgfV9DaVZVTGYuP1ZhSXxWdkdiWH9XdX1RTmZcVUI=");
//builder.Services.AddScoped<NavigationManager>();



await builder.Build().RunAsync();
