using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppLabKryptering
{
    public class EncryptionContext : DbContext
    {
        public DbSet<EncryptedData> EncryptedDataEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=EncryptionData.db");
        }
    }
}
