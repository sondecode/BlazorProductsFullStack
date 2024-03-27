using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using BlazorProducts.Client.Features;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorProducts.Client.AuthProviders
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;
        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return _anonymous;

            var claims = JwtParser.ParseClaimsFromJwt(token);

            //Check the exp filed of the token
            var expriry = claims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();
            if (expriry != null && DateTimeOffset.FromUnixTimeSeconds(long.Parse(expriry.Value)) < DateTime.Now)
            {
                await _localStorage.RemoveItemAsync("authToken");
                return _anonymous;
            }
        
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType"))));
        }
        public void NotifyUserAuthentication(string token)
        {
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);

            _localStorage.SetItemAsync("authToken", token);
        }
        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);

            _localStorage.RemoveItemAsync("authToken");
        }

    }
}
