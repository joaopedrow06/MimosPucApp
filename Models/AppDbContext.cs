public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    private static bool _created = false;
    //public DbSet<AppointmentNames>? AppointmentNames { get; set; }
    public DbSet<Appointments>? Appointments { get; set; }
    //public DbSet<ClientAdress>? ClientAdresses { get; set; }
    public DbSet<Clients>? Clients { get; set; }
    public DbSet<ClientsPets>? ClientsPets { get; set; }
    public DbSet<Pets>? Pets { get; set; }
    public DbSet<TransactionHistories>? TransactionHistories { get; set; }
    public DbSet<Users>? Users { get; set; }
    //public DbSet<PetsVacciness>? PetsVaccinesses { get; set; }
    //public DbSet<Vaccines>? Vaccines { get; set; }
    //public DbSet<VaccinesHistories>? VaccinesHistories { get; set; }

    //public AppDbContext()
    //{
    //    if (!_created)
    //    {
    //        _created = true;
    //        //Database.EnsureDeleted();
    //        Database.EnsureCreated();
    //    }
    //}
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseMySql(connectionString:"DataSource=app.db;Cache=Shared");
    //}

}