using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Tables;

namespace MyWorkBoard.Entities.Config
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.OwnsOne(a => a.Coordinate, cmb =>
            {
                cmb.Property(c => c.Latitude).HasPrecision(18, 7);
                cmb.Property(c => c.Longitude).HasPrecision(18, 7);
            });
        }
    }
}
