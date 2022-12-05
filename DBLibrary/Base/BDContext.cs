using CRMLibs;

using Microsoft.EntityFrameworkCore;

using StructLibs;

using static Object_Description.DB_Access_Struct;

namespace Server
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<СontactPerson> СontactPerson { get; set; }
        public DbSet<Directory> Directory { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Partner> Partner { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventType { get; set; }
        public DbSet<ItemDBStruct> Item { get; set; }
        public DbSet<DetailValue> DetailValue { get; set; }
        public DbSet<Manufactor> Manufactor { get; set; }
        public DbSet<ManufactorSite> ManufactorSite { get; set; }
        public DbSet<Details> Details { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<PriceСhangeHistory> PriceСhangeHistory { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        { 
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=3011656;");
          
        }

    }
}
