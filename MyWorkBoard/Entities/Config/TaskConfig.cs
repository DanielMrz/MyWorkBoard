using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyWorkBoard.Entities.Config
{
    public class TaskConfig : IEntityTypeConfiguration<Tables.Task>
    {
        public void Configure(EntityTypeBuilder<Tables.Task> builder)
        {
            builder.Property(x => x.Activity).HasMaxLength(200);
            builder.Property(x => x.RemainingWork).HasPrecision(14, 2);
        }
    }
}
