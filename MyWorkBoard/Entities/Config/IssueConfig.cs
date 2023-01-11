using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Tables;

namespace MyWorkBoard.Entities.Config
{
    public class IssueConfig : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.Property(x => x.Efford).HasColumnType("decimal(5, 2)");
        }
    }
}