using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using PartyGameTime.Core.Model;

namespace PartyGameTime.Core.Auth;

public class AuthStateProvider : AuthenticationStateProvider
{
private readonly UserManager<Account> _account;
    private readonly SignInManager<Account> _signInManager;

    public AuthStateProvider(UserManager<Account> account, SignInManager<Account> signInManager)
    {
        _account = account;
        _signInManager = signInManager;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await _signInManager.UserManager.GetUserAsync(_signInManager.Context.User);
        if (user != null && _signInManager.Context.User.Identity.IsAuthenticated)
        {
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            });
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);
        }
        else
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}