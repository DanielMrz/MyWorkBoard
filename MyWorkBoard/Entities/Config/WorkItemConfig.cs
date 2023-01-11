using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Model;
using MyWorkBoard.Entities.Tables;
using MyWorkBoard.Entities.Tables.Linktable;
using System.Reflection.Emit;

namespace MyWorkBoard.Entities.Config
{
    public class WorkItemConfig : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            // eb.Property(x => x.State).IsRequired();
            builder.Property(x => x.Area).HasColumnType("varchar(200)");
            builder.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
            builder.Property(x => x.Priority).HasDefaultValue(1);

            builder.HasMany(x => x.Comments)
                    .WithOne(y => y.WorkItem)
                    .HasForeignKey(y => y.WorkItemId);

            builder.HasOne(x => x.Author)
                    .WithMany(y => y.WorkItems)
                    .HasForeignKey(y => y.AuthorId);

            builder.HasMany(x => x.Tags)
                    .WithMany(y => y.WorkItems)
                    .UsingEntity<WorkItemTag>(              // Konfiguracja zależności workitemTag do typu Tag. Dla typu WorkItemTag mamy referencje do jednego elementu typu Tag znajdującego się pod kluczem obcym
                        x => x.HasOne(z => z.Tag)           // WorkitemTag ma jedną właściwość Tag
                        .WithMany()                         // Wiemy, że dany Tag ma wiele elementów workitemtag (nie mamy właściwości poziomu encji tag do tabeli łączącej)
                        .HasForeignKey(z => z.TagId),       // Wskażemy entity pod jaką właściwością znajduje się klucz obcy do tej tabeli

                        x => x.HasOne(z => z.WorkItem)      // WorkitemTag ma jeden WorkItem
                        .WithMany()
                        .HasForeignKey(z => z.WorkItemId),  // a jego klucz obcy znajduje się pod właściwością workitemid

                        x =>                                // Konfiguracja
                        {
                            x.HasKey(y => new { y.TagId, y.WorkItemId });
                            x.Property(y => y.PublicationDate).HasDefaultValueSql("getutcdate()");
                        });

            builder.HasOne(x => x.State)
                    .WithMany()
                    .HasForeignKey(y => y.StateId);
        }
    }
}


