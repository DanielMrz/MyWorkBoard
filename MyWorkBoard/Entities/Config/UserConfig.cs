using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWorkBoard.Entities.Tables;

namespace MyWorkBoard.Entities.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
                                                            // Konfiguracja relacji 1 do 1 w entity
            builder.HasOne(x => x.Address)                  // Wskażemy do jakiej referencji ma on relacje (hasone)
                .WithOne(y => y.User)                       // i od strony adresu mamy tylko 1 usera
                .HasForeignKey<Address>(y => y.UserId);     // wskażę w jakiej kolumnie znajduje się klucz obcy w tabeli User (w encji Address i wartosc UserId)
        }
    }
}
