using Microsoft.EntityFrameworkCore;

namespace AgendamentoTransporte.Data
{
    public class DataProtectionKeyContext : DbContext
    {
        public DataProtectionKeyContext(DbContextOptions<DataProtectionKeyContext> options)
            : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }

    public class DataProtectionKey
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string XmlData { get; set; }
    }
}
