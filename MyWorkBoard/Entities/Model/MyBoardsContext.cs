using Microsoft.EntityFrameworkCore;
using MyWorkBoard.Entities.Tables;
using MyWorkBoard.Entities.Tables.Linktable;

namespace MyWorkBoard.Entities.Model
{
    // Klasa reprezentująca kontekst całej bazy danych
    // Odwzorujemy konkretne tabele
    // Za pomocą stworzonych klas zdefiniujemy tabele, któe beda w tej bazie danych
    // Klasa, która dziedziczy po klasie DbContext będzie w stanie w odpowiedni dla nas sposób skonfigurować połączenie i definicje oraz schematy konkretnych tabel

    public class MyBoardsContext : DbContext
    {
        // Dodanie publicznego konstruktora, do którego przekażę zmienną typu DbContextOptions dla klasy MyBoardsContext
        // Dzięki temu konstruktor kontener dependency injection będzie w stanie skonfigurować klasę MyBoardsContext
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {

        }

        // Utworzenie właściwości o nazwie naszej nowej tabeli specjalnego typu DbSet przyjmujący generyczny parametr dla typ, który będzie reprezentować konkretną tabele

        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Tables.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<State> States { get; set; }

        public DbSet<WorkItemTag> WorkItemTag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 
            // modelBuilder.Entity<WorkItem>()
            //    .Property(x => x.State)
            //    .IsRequired();
            modelBuilder.Entity<Epic>(eb =>
            {
                eb.Property(x => x.EndDate).HasPrecision(3);
            });

            modelBuilder.Entity<Issue>(eb =>
            {
                eb.Property(x => x.Efford).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<Tables.Task>(eb =>
            {
                eb.Property(x => x.Activity).HasMaxLength(200);
                eb.Property(x => x.RemainingWork).HasPrecision(14, 2);
            });

            modelBuilder.Entity<WorkItem>(eb => // eb = entity builder
            {
                // eb.Property(x => x.State).IsRequired();
                eb.Property(x => x.Area).HasColumnType("varchar(200)");
                eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(x => x.Priority).HasDefaultValue(1);

                // Relacje
                eb.HasMany(x => x.Comments)
                    .WithOne(y => y.WorkItem)
                    .HasForeignKey(y => y.WorkItemId);

                eb.HasOne(x => x.Author)
                    .WithMany(y => y.WorkItems)
                    .HasForeignKey(y => y.AuthorId);

                eb.HasMany(x => x.Tags)
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

                eb.HasOne(x => x.State)
                    .WithMany()
                    .HasForeignKey(y => y.StateId);
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                // SQL po stronie serwera ustawia domyślną wartosc
                eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
                // Entity ustawia wartość
                eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
                // Każdy komentarz ma jednego autora. Autor może mieć wiele komentarzy
                eb.HasOne(x => x.Author)
                    .WithMany(y => y.Comments)
                    .HasForeignKey(x => x.AuthorId)
                    .OnDelete(DeleteBehavior.NoAction); // Aby nie było kaskadowego usuwania (jesli usuwamy autora to komentarze zostają)
            });

            // Konfiguracja relacji 1 do 1 w entity
            // Wskażemy do jakiej referencji ma on relacje (hasone)
            // i od strony adresu mamy tylko 1 usera
            // wskażemy w jakiej kolumnie znajduje się klucz obcy w tabeli User (w encji Address i wartosc UserId)
            modelBuilder.Entity<User>()
                .HasOne(x => x.Address)
                .WithOne(y => y.User)
                .HasForeignKey<Address>(y => y.UserId);

            // Tworzenie złożonego klucza poprzez stworzenie typu anonimowego
            // Mając taką konfiguracje w encji tag jak i work item nalezy dodać referencje do tego typu
            modelBuilder.Entity<WorkItemTag>()
                .HasKey(x => new { x.TagId, x.WorkItemId });

            modelBuilder.Entity<State>(eb =>
            {
                eb.Property(x => x.StateValue).IsRequired().HasMaxLength(60);
            });

            // Określamy jakie dane początkowe powinna mieć konkretna tabela
            modelBuilder.Entity<State>()
                .HasData(new State() { Id = 1, StateValue = "To Do" },
                         new State() { Id = 2, StateValue = "Doing" },
                         new State() { Id = 3, StateValue = "Done" },
                         new State() { Id = 4, StateValue = "On Hold" },
                         new State() { Id = 5, StateValue = "Rejected" });

            modelBuilder.Entity<Tag>()
                .HasData(new Tag() { Id = 1, TagValue = "Web" },
                         new Tag() { Id = 2, TagValue = "UI" },
                         new Tag() { Id = 3, TagValue = "Desktop" },
                         new Tag() { Id = 4, TagValue = "API" },
                         new Tag() { Id = 5, TagValue = "Service" });
        }

        // konfiguracja konkretnych encji bazy danych np do tworzenia złożonych kluczów głównych
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasKey(x => new { x.Email, x.LastName });
        //}

        // 1 sposób konfiguracji połączenia z bazą danych 
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Zwróć uwagę na wybór metody jeżeli wybrałeś MS Sql. To standardowy link do bazy którą tworzy visualstudio + nazwa bazy danych MyWorkBoardDB
        //    // Za pomocą trusted connection będę w stanie zalogować się bezpośrednio za pomocą użytkownika windowsowego przez którego ta aplikacja będzie uruchamiana 
        //    // Minus tego rozwiązania jest taki że powinno się trzymać dane konfiguracyjne w oddzielnym pliku
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MyWorkBoardDB;Trusted_Connection=True;");
        //}
    }
}
