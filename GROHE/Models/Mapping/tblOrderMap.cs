using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GROHE.Models.Mapping
{
    public class tblOrderMap : EntityTypeConfiguration<tblOrder>
    {
        public tblOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tblOrder");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Tolar).HasColumnName("Tolar");
            this.Property(t => t.DateByy).HasColumnName("DateByy");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Active).HasColumnName("Active");
        }
    }
}
