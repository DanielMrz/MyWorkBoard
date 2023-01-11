using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Tables;

namespace MyWorkBoard.Entities.Config
{
    public class EpicConfig : IEntityTypeConfiguration<Epic>
    {
        public void Configure(EntityTypeBuilder<Epic> builder)
        {
            builder.Property(x => x.EndDate).HasPrecision(3);
        }
    }
}
