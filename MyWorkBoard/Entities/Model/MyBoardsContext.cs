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

        // Stworzenie dbseta dzięki, któremu będziemy mogli się dostać do naszego widoku
        public DbSet<TopAuthor> ViewTopAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Metoda szuka plików konfiguracyjnych
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
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
