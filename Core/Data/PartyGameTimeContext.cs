using Microsoft.EntityFrameworkCore;

namespace PartyGameTime.Core.Data;

public class PgtDbContext : DbContext
{
    public PgtDbContext(DbContextOptions<PgtDbContext> options) : base(options)
    {
        
    }
}