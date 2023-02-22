using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PartyGameTime.Core.Model;

namespace PartyGameTime.Core.Data;

public class PgtDbContext : DbContext
{
    public PgtDbContext(DbContextOptions<PgtDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<IdentityUserRole<string>>(eb=>
            eb.HasNoKey()
            );
    }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<IdentityUserClaim<string>> IdentityUserClaims { get; set; }
    public DbSet<IdentityUserRole<string>> IdentityUserRoles { get; set; }
    
}