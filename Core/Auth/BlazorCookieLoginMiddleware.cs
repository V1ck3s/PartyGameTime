using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using PartyGameTime.Core.Model;

namespace PartyGameTime.Core.Auth;

public class LoginInfo
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class BlazorCookieLoginMiddleware
{
    public static IDictionary<Guid, LoginInfo> Logins { get; private set; }
        = new ConcurrentDictionary<Guid, LoginInfo>();        


    private readonly RequestDelegate _next;

    public BlazorCookieLoginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, SignInManager<Account> signInMgr)
    {
        if (context.Request.Path == "/auth/login" && context.Request.Query.ContainsKey("key"))
        {
            var key = Guid.Parse(context.Request.Query["key"]);
            var info = Logins[key];
            var result = await signInMgr.PasswordSignInAsync(info.Email, info.Password, true, lockoutOnFailure: false);
            info.Password = null;
            
            if (result.Succeeded)
            {
                Logins.Remove(key);
                context.Response.Redirect("/");
            }
            else if (result.RequiresTwoFactor)
            {
                //TODO: redirect to 2FA razor component
                context.Response.Redirect("/auth/loginwith2fa/" + key);
            }
            else
            {
                //TODO: Proper error handling
                context.Response.Redirect("/auth/loginfailed");
            }    
        }     
        else
        {
            await _next.Invoke(context);
        }
    }
}