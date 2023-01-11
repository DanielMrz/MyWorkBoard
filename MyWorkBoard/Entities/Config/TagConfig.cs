using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Tables;

namespace MyWorkBoard.Entities.Config
{
    public class TagConfig : IEntityTypeConfiguration<Tag>
    {

        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasData(new Tag() { Id = 1, TagValue = "Web" },
            new Tag() { Id = 2, TagValue = "UI" },
            new Tag() { Id = 3, TagValue = "Desktop" },
            new Tag() { Id = 4, TagValue = "API" },
            new Tag() { Id = 5, TagValue = "Service" });
        }
    }
}
