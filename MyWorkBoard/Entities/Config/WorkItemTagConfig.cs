using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Tables.Linktable;

namespace MyWorkBoard.Entities.Config
{
    public class WorkItemTagConfig : IEntityTypeConfiguration<WorkItemTag>
    {
        public void Configure(EntityTypeBuilder<WorkItemTag> builder)
        {
            // Tworzenie złożonego klucza poprzez stworzenie typu anonimowego
            // Mając taką konfiguracje w encji tag jak i work item nalezy dodać referencje do tego typu
            builder.HasKey(x => new { x.TagId, x.WorkItemId });
        }
    }
}
