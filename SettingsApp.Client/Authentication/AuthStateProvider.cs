// Authentication/AuthStateProvider.cs
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace SettingsApp.Client.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        public AuthStateProvider(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            var role = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userRole");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt", ClaimTypes.Name, ClaimTypes.Role);
            
            // Add role claim if not present in token
            if (!string.IsNullOrEmpty(role) && !identity.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var user = new ClaimsPrincipal(identity);
            
            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string token, string role)
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt", ClaimTypes.Name, ClaimTypes.Role);
            if (!string.IsNullOrEmpty(role))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            var user = new ClaimsPrincipal(identity);
            
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                foreach (var kvp in keyValuePairs)
                {
                    var claimType = kvp.Key switch
                    {
                        "name" or "unique_name" or ClaimTypes.Name => ClaimTypes.Name,
                        "email" or ClaimTypes.Email => ClaimTypes.Email,
                        "role" or ClaimTypes.Role => ClaimTypes.Role,
                        "nameid" or ClaimTypes.NameIdentifier => ClaimTypes.NameIdentifier,
                        _ => kvp.Key
                    };

                    var value = kvp.Value?.ToString() ?? string.Empty;

                    if (claimType == ClaimTypes.Role && value.Trim().StartsWith('['))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(value);
                        if (parsedRoles != null)
                        {
                            foreach (var parsedRole in parsedRoles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                            }
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(claimType, value));
                    }
                }
            }

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}