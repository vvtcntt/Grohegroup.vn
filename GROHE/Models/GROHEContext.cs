using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using GROHE.Models.Mapping;

namespace GROHE.Models
{
    public partial class GROHEContext : DbContext
    {
        static GROHEContext()
        {
            Database.SetInitializer<GROHEContext>(null);
        }

        public GROHEContext()
            : base("Name=GROHEContext")
        {
        }

        public DbSet<ImageProduct> ImageProducts { get; set; }
        public DbSet<ProductConnect> ProductConnects { get; set; }
        public DbSet<tblAddress> tblAddresses { get; set; }
        public DbSet<tblAgency> tblAgencies { get; set; }
        public DbSet<tblColorProduct> tblColorProducts { get; set; }
        public DbSet<tblConfig> tblConfigs { get; set; }
        public DbSet<tblConnectColorProduct> tblConnectColorProducts { get; set; }
        public DbSet<tblConnectFunProuduct> tblConnectFunProuducts { get; set; }
        public DbSet<tblContact> tblContacts { get; set; }
        public DbSet<tblCountOnline> tblCountOnlines { get; set; }
        public DbSet<tblFile> tblFiles { get; set; }
        public DbSet<tblFunctionProduct> tblFunctionProducts { get; set; }
        public DbSet<tblGroupAgency> tblGroupAgencies { get; set; }
        public DbSet<tblGroupImage> tblGroupImages { get; set; }
        public DbSet<tblGroupNew> tblGroupNews { get; set; }
        public DbSet<tblGroupProduct> tblGroupProducts { get; set; }
        public DbSet<tblHistoryLogin> tblHistoryLogins { get; set; }
        public DbSet<tblImage> tblImages { get; set; }
        public DbSet<tblManufacture> tblManufactures { get; set; }
        public DbSet<tblMap> tblMaps { get; set; }
        public DbSet<tblNew> tblNews { get; set; }
        public DbSet<tblOrder> tblOrders { get; set; }
        public DbSet<tblOrderDetail> tblOrderDetails { get; set; }
        public DbSet<tblPartner> tblPartners { get; set; }
        public DbSet<tblProduct> tblProducts { get; set; }
        public DbSet<tblProductSale> tblProductSales { get; set; }
        public DbSet<tblProductSyn> tblProductSyns { get; set; }
        public DbSet<tblRegister> tblRegisters { get; set; }
        public DbSet<tblSupport> tblSupports { get; set; }
        public DbSet<tblUrl> tblUrls { get; set; }
        public DbSet<tblUser> tblUsers { get; set; }
        public DbSet<tblVideo> tblVideos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ImageProductMap());
            modelBuilder.Configurations.Add(new ProductConnectMap());
            modelBuilder.Configurations.Add(new tblAddressMap());
            modelBuilder.Configurations.Add(new tblAgencyMap());
            modelBuilder.Configurations.Add(new tblColorProductMap());
            modelBuilder.Configurations.Add(new tblConfigMap());
            modelBuilder.Configurations.Add(new tblConnectColorProductMap());
            modelBuilder.Configurations.Add(new tblConnectFunProuductMap());
            modelBuilder.Configurations.Add(new tblContactMap());
            modelBuilder.Configurations.Add(new tblCountOnlineMap());
            modelBuilder.Configurations.Add(new tblFileMap());
            modelBuilder.Configurations.Add(new tblFunctionProductMap());
            modelBuilder.Configurations.Add(new tblGroupAgencyMap());
            modelBuilder.Configurations.Add(new tblGroupImageMap());
            modelBuilder.Configurations.Add(new tblGroupNewMap());
            modelBuilder.Configurations.Add(new tblGroupProductMap());
            modelBuilder.Configurations.Add(new tblHistoryLoginMap());
            modelBuilder.Configurations.Add(new tblImageMap());
            modelBuilder.Configurations.Add(new tblManufactureMap());
            modelBuilder.Configurations.Add(new tblMapMap());
            modelBuilder.Configurations.Add(new tblNewMap());
            modelBuilder.Configurations.Add(new tblOrderMap());
            modelBuilder.Configurations.Add(new tblOrderDetailMap());
            modelBuilder.Configurations.Add(new tblPartnerMap());
            modelBuilder.Configurations.Add(new tblProductMap());
            modelBuilder.Configurations.Add(new tblProductSaleMap());
            modelBuilder.Configurations.Add(new tblProductSynMap());
            modelBuilder.Configurations.Add(new tblRegisterMap());
            modelBuilder.Configurations.Add(new tblSupportMap());
            modelBuilder.Configurations.Add(new tblUrlMap());
            modelBuilder.Configurations.Add(new tblUserMap());
            modelBuilder.Configurations.Add(new tblVideoMap());
        }
    }
}
