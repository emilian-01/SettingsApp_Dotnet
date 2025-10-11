// using Microsoft.AspNetCore.Components.Authorization;
// using Microsoft.AspNetCore.Components.Web;
// using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
// using SettingsApp.Client;
// using SettingsApp.Client.Authentication;
// using SettingsApp.Client.Services;

// var builder = WebAssemblyHostBuilder.CreateDefault(args);
// builder.RootComponents.Add<App>("#app");
// builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


// // Add Authentication services
// builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
// builder.Services.AddAuthorizationCore();

// // Add our services
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<ISettingsService, SettingsService>();

// await builder.Build().RunAsync();


using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using SettingsApp.Client;
using SettingsApp.Client.Authentication;
using SettingsApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Base address for API calls
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5036/") });

// Add authorization message handler
builder.Services.AddScoped<AuthorizationMessageHandler>();

// Configure authorized HttpClient
builder.Services.AddHttpClient("AuthAPI", client => 
{
    client.BaseAddress = new Uri("http://localhost:5036/");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("AuthAPI"));

// Add Authentication services
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthorizationCore();

// Add our services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();

await builder.Build().RunAsync();