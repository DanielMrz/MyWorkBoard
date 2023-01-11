using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Model;
using System.Reflection.Emit;

namespace MyWorkBoard.Entities.Config
{
    public class TopAuthorConfig : IEntityTypeConfiguration<TopAuthor>
    {
        public void Configure(EntityTypeBuilder<TopAuthor> builder)
        {
            // Stworzenie widoku bazodanowego (po stronie bazy danych)
            // model dla konkretnego widoku, który nie ma swojego klucza (trzeba utworzyć migracje)
            builder.ToView("View_TopAuthors");
            builder.HasNoKey();
        }
    }
}
