using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
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
            var info = BlazorCookieLoginMiddleware.Logins[key];

            //var result = await signInMgr.PasswordSignInAsync(info.Email, info.Password, true, lockoutOnFailure: true);
            //info.Password = null;
            var result = await signInMgr.PasswordSignInAsync(info.Email, info.Password, true, lockoutOnFailure: false);
            info.Password = null;
            
            if (result.Succeeded)
            {
                BlazorCookieLoginMiddleware.Logins.Remove(key);
                context.Response.Redirect("/");
                //_navigationManager.NavigateTo("/");
                
                //Logins.Remove(key);
                //context.Response.Redirect("/");
                //return;
            }
            else if (result.RequiresTwoFactor)
            {
                //TODO: redirect to 2FA razor component
                context.Response.Redirect("/auth/loginwith2fa/" + key);
                return;
            }
            else
            {
                //TODO: Proper error handling
                context.Response.Redirect("/auth/loginfailed");
                return;
            }    
        }     
        else
        {
            await _next.Invoke(context);
        }
    }
}