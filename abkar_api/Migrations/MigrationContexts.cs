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
        }

        //Migrations
        public DbSet<Customers> Customer { get; set; }
        public DbSet<Departments> Deparments { get; set; }
        public DbSet<Personnel> Personnel { get; set; }
        public DbSet<ProductionPersonnel> ProductionPersonnel { get; set; }
        public DbSet<ProductionPersonnelOperation> ProductionProcessStocks { get; set; }
        public DbSet<Productions> Productions { get; set; }
        public DbSet<StockCards> StockCards { get; set; }
        public DbSet<StockTypes> StockTypes { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<StockMovements> StockMovements { get; set; }
        public DbSet<SupplyRequisitions> SuppliyRequistions { get; set; }
        public DbSet<OrderStocks> OrderStocks { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Machines> Machines { get; set; }
        public DbSet<Operations> Operations { get; set; }
        public DbSet<StockCardProcessNo> StockCardProcessNo { get; set; }


    }
}