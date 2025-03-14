using Microsoft.EntityFrameworkCore;

namespace mac;

public class MacTableDataContext : DbContext
{
    public MacTableDataContext(DbContextOptions<MacTableDataContext> options)
        : base(options)
    {
    }

    public DbSet<MacTableData> MacTableDatas { get; set; }
}
