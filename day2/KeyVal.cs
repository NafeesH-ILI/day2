using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace day2
{
    class KeyValContext : DbContext
    {
        public DbSet<KeyVal> KeyVals { get; set; }
        private string _connStr = "Host=127.0.0.1;Username=postgres;Password=abcd1234;Database=test";

        public KeyValContext(DbContextOptions<KeyValContext> options) : base(options){
        }
        public KeyValContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_connStr);
        }

        public bool ConnTest()
        {
            return Database.CanConnect();
        }
    }

    [Table("keyvals")]
    public class KeyVal
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("val")]
        public string? Val { get; set; }
    }

    public class KeyVal_ValOnly
    {
        public required string Val { get; set; }
    }
}
