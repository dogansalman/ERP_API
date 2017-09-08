using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace abkar_api.Models
{
    public class MigrationContexts : DbContext
    {
        //ConnectionStrings From Web.Config
        public MigrationContexts() : base("ConnectionStr") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Set index key ProductionPersonnel
            modelBuilder.Entity<ProductionPersonnel>()
                .HasKey(pp => new { pp.personel_id, pp.production_id });

            //Set index key ProductionStocks
            modelBuilder.Entity<ProductionProcessStocks>()
                .HasKey(pps => new { pps.production_id, pps.stock_id });

            //Set index key personnel username
            modelBuilder.Entity<Personnel>().HasKey(p => new {p.id, p.username });

            //Set index key customer email
            modelBuilder.Entity<Customers>().HasKey(c => new { c.id, c.email });


            //Set index key stock card code
            modelBuilder.Entity<StockCards>().HasKey(sc => new { sc.id, sc.code});
        }

        //Migrations
        public DbSet<Customers> Customer { get; set; }
        public DbSet<Departments> Deparments { get; set; }
        public DbSet<Personnel> Personnel { get; set; }
        public DbSet<ProductionPersonnel> ProductionPersonnel { get; set; }
        public DbSet<ProductionProcessStocks> ProductionProcessStocks { get; set; }
        public DbSet<Productions> Productions { get; set; }
        public DbSet<ProductionStates> ProductionStates { get; set; }
        public DbSet<ProductionStocks> ProductionStock { get; set; }
        public DbSet<StockCards> StockCards { get; set; }
        public DbSet<StockTypes> StockTypes { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<StockMovements> StockMovements { get; set; }
        public DbSet<SupplyRequisitions> SuppliyRequistions { get; set; }
        
    }
}