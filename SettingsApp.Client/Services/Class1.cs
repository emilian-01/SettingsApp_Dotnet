// // Services/IAuthService.cs
// using SettingsApp.Client.Models;
// using System.Net.Http.Headers;
// using System.Net.Http.Json;
// using System.Text;
// using System.Text.Json;
// using Microsoft.JSInterop;
// using System.Security.Claims;
// // using Microsoft.AspNetCore.Components.Authorization;
// using Microsoft.AspNetCore.Components;
// // using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

// namespace SettingsApp.Client.Services
// {
//     public interface IAuthService
//     {
//         Task<AuthResponse> Login(UserLogin userLogin);
//         Task<AuthResponse> Register(UserRegister userRegister);
//         Task Logout();
//         Task<bool> IsLoggedIn();
//         Task<string> GetUserRole();
//     }
// }

// // Services/AuthService.cs


// namespace SettingsApp.Client.Services
// {
//     public class AuthService : IAuthService
//     {
//         private readonly HttpClient _httpClient;
//         private readonly IJSRuntime _jsRuntime;

//         public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
//         {
//             _httpClient = httpClient;
//             _jsRuntime = jsRuntime;
//         }

//         public async Task<AuthResponse> Login(UserLogin userLogin)
//         {
//             var response = await _httpClient.PostAsJsonAsync("api/Auth/login", userLogin);
//             var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            
//             if (result != null && result.Success)
//             {
//                 await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
//                 await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userRole", result.Role);
//             }
            
//             return result ?? new AuthResponse { Success = false, Message = "An error occurred during login." };
//         }

//         public async Task<AuthResponse> Register(UserRegister userRegister)
//         {
//             var response = await _httpClient.PostAsJsonAsync("api/Auth/register", userRegister);
//             var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            
//             return result ?? new AuthResponse { Success = false, Message = "An error occurred during registration." };
//         }

//         public async Task Logout()
//         {
//             await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
//             await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userRole");
//         }

//         public async Task<bool> IsLoggedIn()
//         {
//             var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
//             return !string.IsNullOrEmpty(token);
//         }

//         public async Task<string> GetUserRole()
//         {
//             return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userRole") ?? string.Empty;
//         }
//     }
// }

// // Authentication/AuthStateProvider.cs


// namespace SettingsApp.Client.Authentication
// {
//     public class AuthStateProvider : AuthenticationStateProvider
//     {
//         private readonly IJSRuntime _jsRuntime;
//         private readonly HttpClient _httpClient;

//         public AuthStateProvider(IJSRuntime jsRuntime, HttpClient httpClient)
//         {
//             _jsRuntime = jsRuntime;
//             _httpClient = httpClient;
//         }

//         public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//         {
//             var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
//             var role = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userRole");

//             if (string.IsNullOrEmpty(token))
//             {
//                 return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//             }

//             _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

//             var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            
//             // Add role claim if not present in token
//             if (!string.IsNullOrEmpty(role) && !identity.HasClaim(c => c.Type == ClaimTypes.Role))
//             {
//                 identity.AddClaim(new Claim(ClaimTypes.Role, role));
//             }

//             var user = new ClaimsPrincipal(identity);
            
//             return new AuthenticationState(user);
//         }

//         public void NotifyUserAuthentication(string token, string role)
//         {
//             var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
//             if (!string.IsNullOrEmpty(role))
//             {
//                 identity.AddClaim(new Claim(ClaimTypes.Role, role));
//             }
//             var user = new ClaimsPrincipal(identity);
            
//             NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
//         }

//         public void NotifyUserLogout()
//         {
//             var identity = new ClaimsIdentity();
//             var user = new ClaimsPrincipal(identity);
            
//             NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
//         }

//         private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
//         {
//             var claims = new List<Claim>();
//             var payload = jwt.Split('.')[1];
//             var jsonBytes = ParseBase64WithoutPadding(payload);
//             var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

//             if (keyValuePairs != null)
//             {
//                 keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles);

//                 if (roles != null)
//                 {
//                     if (roles.ToString()!.Trim().StartsWith("["))
//                     {
//                         var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);
//                         if (parsedRoles != null)
//                         {
//                             foreach (var parsedRole in parsedRoles)
//                             {
//                                 claims.Add(new Claim(ClaimTypes.Role, parsedRole));
//                             }
//                         }
//                     }
//                     else
//                     {
//                         claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
//                     }

//                     keyValuePairs.Remove(ClaimTypes.Role);
//                 }

//                 claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));
//             }

//             return claims;
//         }

//         private byte[] ParseBase64WithoutPadding(string base64)
//         {
//             switch (base64.Length % 4)
//             {
//                 case 2: base64 += "=="; break;
//                 case 3: base64 += "="; break;
//             }
//             return Convert.FromBase64String(base64);
//         }
//     }
// }

// // Authentication/AuthorizationMessageHandler.cs


// namespace SettingsApp.Client.Authentication
// {
//     public class AuthorizationMessageHandler : DelegatingHandler
//     {
//         private readonly IJSRuntime _jsRuntime;
//         private readonly NavigationManager _navigationManager;

//         public AuthorizationMessageHandler(IJSRuntime jsRuntime, NavigationManager navigationManager)
//         {
//             _jsRuntime = jsRuntime;
//             _navigationManager = navigationManager;
//         }

//         protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//         {
//             var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

//             if (!string.IsNullOrEmpty(token))
//             {
//                 request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
//             }

//             var response = await base.SendAsync(request, cancellationToken);

//             if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//             {
//                 await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
//                 await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userRole");
//                 _navigationManager.NavigateTo("/login");
//             }

//             return response;
//         }
//     }
// }