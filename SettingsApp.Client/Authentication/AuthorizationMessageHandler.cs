// Authentication/AuthorizationMessageHandler.cs
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

namespace SettingsApp.Client.Authentication
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;

        public AuthorizationMessageHandler(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userRole");
                _navigationManager.NavigateTo("/login");
            }

            return response;
        }
    }
}