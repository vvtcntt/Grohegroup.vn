using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GROHE.Models.Mapping
{
    public class tblAddressMap : EntityTypeConfiguration<tblAddress>
    {
        public tblAddressMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("tblAddress");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Ord).HasColumnName("Ord");
            this.Property(t => t.Active).HasColumnName("Active");
        }
    }
}
