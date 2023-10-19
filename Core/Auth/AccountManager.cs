using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PartyGameTime.Core.Model;

namespace PartyGameTime.Core.Auth;

public class AccountManager
{
    private readonly UserManager<Account> _accountManager;
    private readonly RoleManager<AccountRole> _accountRole;
    private readonly SignInManager<Account> _signInManager;
    private readonly NavigationManager _navigationManager;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AccountManager(UserManager<Account> account, RoleManager<AccountRole> accountRole, SignInManager<Account> signInManager, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
    {
        _accountManager = account;
        _accountRole = accountRole;
        _signInManager = signInManager;
        _navigationManager = navigationManager;
        _authenticationStateProvider = authenticationStateProvider;
        _authenticationStateProvider.AuthenticationStateChanged += Initialize;
    }

    private AuthenticationState? _authenticationState;
    private ClaimsPrincipal? _user;
    private string _userId;
    private Core.Model.Account _account;
    
    private async void Initialize(Task<AuthenticationState> t)
    {
        if ((_account is null || _user is null || _authenticationState is null))
        {
            _authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            _user = _authenticationState.User;
            if (_user?.Identity != null && _user.Identity.IsAuthenticated)
            {
                _userId = _user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (_userId is not null)
                {
                    _account = await GetAccount(_userId);
                    _authenticationStateProvider.AuthenticationStateChanged -= Initialize;
                }
            }
        }
    }

    public async Task<string> GetCurrentUserUsername()
    {
        if (_user?.Identity?.Name is not null)
        {
            return _user.Identity.Name;
        }

        return null;
    }

    public async Task<string> GetCurrentUserId()
    {
        return _userId;
    }

    public async Task<string> GetCurrentUserEmail()
    {
        return _account.Email ?? string.Empty;
    }
    
    public async Task SignOut()
    {
        _navigationManager.NavigateTo("/auth/logout",true);
    }
    
    public async Task<IdentityResult> CreateAccount(Account account, string password)
    {
        var result = await _accountManager.CreateAsync(account, password);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> CreateRole(AccountRole role)
    {
        var result = await _accountRole.CreateAsync(role);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> AddRoleToAccount(Account account, string role)
    {
        var result = await _accountManager.AddToRoleAsync(account, role);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> RemoveRoleFromAccount(Account account, string role)
    {
        var result = await _accountManager.RemoveFromRoleAsync(account, role);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> DeleteAccount(Account account)
    {
        var result = await _accountManager.DeleteAsync(account);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> DeleteRole(AccountRole role)
    {
        var result = await _accountRole.DeleteAsync(role);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<Account> GetAccount(string id)
    {
        var account = await _accountManager.FindByIdAsync(id);
        if (account != null)
        {
            return account;
        }

        return null;
    }
    
    public async Task<Account> GetAccountByName(string name)
    {
        var account = await _accountManager.FindByNameAsync(name);
        if (account != null)
        {
            return account;
        }

        return null;
    }
    
    public async Task<AccountRole> GetRole(string id)
    {
        var role = await _accountRole.FindByIdAsync(id);
        if (role != null)
        {
            return role;
        }

        return null;
    }
    
    public async Task<AccountRole> GetRoleByName(string name)
    {
        var role = await _accountRole.FindByNameAsync(name);
        if (role != null)
        {
            return role;
        }

        return null;
    }
    
    public async Task<IList<string>> GetRoles(Account account)
    {
        var roles = await _accountManager.GetRolesAsync(account);
        if (roles != null)
        {
            return roles;
        }

        return null;
    }
    
    public async Task<IList<Account>> GetAccountsInRole(string role)
    {
        var accounts = await _accountManager.GetUsersInRoleAsync(role);
        if (accounts != null)
        {
            return accounts;
        }

        return null;
    }
    
    public async Task<IList<Account>> GetAccounts()
    {
        var accounts = await _accountManager.Users.ToListAsync();
        if (accounts != null)
        {
            return accounts;
        }

        return null;
    }
    
    public async Task<IList<AccountRole>> GetRoles()
    {
        var roles = await _accountRole.Roles.ToListAsync();
        if (roles != null)
        {
            return roles;
        }

        return null;
    }
    
    public async Task<IdentityResult> UpdateAccount(Account account)
    {
        var result = await _accountManager.UpdateAsync(account);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> UpdateRole(AccountRole role)
    {
        var result = await _accountRole.UpdateAsync(role);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> ChangePassword(Account account, string currentPassword, string newPassword)
    {
        var result = await _accountManager.ChangePasswordAsync(account, currentPassword, newPassword);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }

    public async Task<bool> Checkpassword(Account account, string password)
    {
        var result = await _accountManager.CheckPasswordAsync(account, password);
        return result;
    }

}