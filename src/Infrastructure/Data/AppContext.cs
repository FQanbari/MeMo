using Domain.Entities.MemoAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options) : base(options) { }

    public DbSet<Memo> Memos { get; set; }
}
