using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Tables;

namespace MyWorkBoard.Entities.Config
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // SQL po stronie serwera ustawia domyślną wartosc
            builder.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
            // Entity ustawia wartość
            builder.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
            // Każdy komentarz ma jednego autora. Autor może mieć wiele komentarzy
            builder.HasOne(x => x.Author)
                .WithMany(y => y.Comments)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.NoAction); // Aby nie było kaskadowego usuwania (jesli usuwamy autora to komentarze zostają)
        }
    }
}
