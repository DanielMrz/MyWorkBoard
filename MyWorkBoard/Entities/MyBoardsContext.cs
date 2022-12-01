using Microsoft.EntityFrameworkCore;

namespace MyWorkBoard.Entities
{
    // Klasa reprezentująca kontekst całej bazy danych
    // Odwzorujemy konkretne tabele
    // Za pomocą stworzonych klas zdefiniujemy tabele, któe beda w tej bazie danych
    // Klasa, która dziedziczy po klasie DbContext będzie w stanie w odpowiedni dla nas sposób skonfigurować połączenie i definicje oraz schematy konkretnych tabel

    public class MyBoardsContext : DbContext
    {
        // Dodanie publicznego konstruktora do któego przekażę zmienną typu DbContextOptions dla klasy MyBoardsCOntext
        // Dzięki temu konstruktorowie kontener dependency injection będzie w stanie skonfigurować klasę MyBoardsContext
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {

        }


        // Utworzenie właściwości o nazwie naszej nowej tabeli specjalnego typu DbSet przyjmujący generyczny parametr dla typ, który będzie reprezentować konkretną tabele
        
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }


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
