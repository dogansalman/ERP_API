using System.Data.Entity;
using abkar_api.Models;

namespace abkar_api.Contexts
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext() : base("Name=ConnectionStr")
        {
            Database.SetInitializer<DatabaseContext>(null);

        }
        public DbSet<Customers> customers { get; set; }
        public DbSet<Personnel> personnels { get; set; }
        public DbSet<Departments> departments { get; set; }
        public DbSet<Suppliers> suppliers { get; set; }
        public DbSet<StockTypes> stocktypes { get; set; }
        public DbSet<StockCards> stockcards { get; set; }
        public DbSet<StockMovements> stockmovements { get; set; }
        public DbSet<SupplyRequisitions> supplyrequisitions { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<OrderStocks> orderstocks { get; set; }
        public DbSet<Machines> machines { get; set; }
        public DbSet<Operations> operations { get; set; }
        public DbSet<Productions> productions { get; set; }
        public DbSet<ProductionPersonnel> production_personnels { get; set; }
        public DbSet<ProductionPersonnelOperation> production_personnel_operation { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customers>().ToTable("customers");
            modelBuilder.Entity<Personnel>().ToTable("personnel");
            modelBuilder.Entity<Departments>().ToTable("departments");
            modelBuilder.Entity<Suppliers>().ToTable("suppliers");
            modelBuilder.Entity<StockTypes>().ToTable("stocktypes");
            modelBuilder.Entity<StockCards>().ToTable("stockcards");
            modelBuilder.Entity<StockMovements>().ToTable("stockmovements");
            modelBuilder.Entity<SupplyRequisitions>().ToTable("supplyrequisitions");
            modelBuilder.Entity<Orders>().ToTable("orders");
            modelBuilder.Entity<OrderStocks>().ToTable("orderstocks");
            modelBuilder.Entity<Machines>().ToTable("machines");
            modelBuilder.Entity<Operations>().ToTable("operations");
            modelBuilder.Entity<Productions>().ToTable("productions");
            modelBuilder.Entity<ProductionPersonnel>().ToTable("productionpersonnel");
            modelBuilder.Entity<ProductionPersonnelOperation>().ToTable("productionpersonneloperation");

        }
    }
}