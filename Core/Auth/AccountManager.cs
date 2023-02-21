using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PartyGameTime.Core.Model;

namespace PartyGameTime.Core.Auth;

public class AccountManager
{
    private readonly UserManager<Account> _account;
    private readonly RoleManager<AccountRole> _accountRole;
    private readonly SignInManager<Account> _signInManager;

    public AccountManager(UserManager<Account> account, RoleManager<AccountRole> accountRole, SignInManager<Account> signInManager)
    {
        _account = account;
        _accountRole = accountRole;
        _signInManager = signInManager;
    }
    
    public async Task<SignInResult> SignIn(string username, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, false);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
    }
    
    public async Task<IdentityResult> CreateAccount(Account account, string password)
    {
        var result = await _account.CreateAsync(account, password);
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
        var result = await _account.AddToRoleAsync(account, role);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> RemoveRoleFromAccount(Account account, string role)
    {
        var result = await _account.RemoveFromRoleAsync(account, role);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }
    
    public async Task<IdentityResult> DeleteAccount(Account account)
    {
        var result = await _account.DeleteAsync(account);
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
        var account = await _account.FindByIdAsync(id);
        if (account != null)
        {
            return account;
        }

        return null;
    }
    
    public async Task<Account> GetAccountByName(string name)
    {
        var account = await _account.FindByNameAsync(name);
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
        var roles = await _account.GetRolesAsync(account);
        if (roles != null)
        {
            return roles;
        }

        return null;
    }
    
    public async Task<IList<Account>> GetAccountsInRole(string role)
    {
        var accounts = await _account.GetUsersInRoleAsync(role);
        if (accounts != null)
        {
            return accounts;
        }

        return null;
    }
    
    public async Task<IList<Account>> GetAccounts()
    {
        var accounts = await _account.Users.ToListAsync();
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
        var result = await _account.UpdateAsync(account);
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
        var result = await _account.ChangePasswordAsync(account, currentPassword, newPassword);
        if (result.Succeeded)
        {
            return result;
        }

        return null;
    }

}