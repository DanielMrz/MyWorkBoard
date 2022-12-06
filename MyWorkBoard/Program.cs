using Microsoft.EntityFrameworkCore;
using MyWorkBoard.Entities.Model;
using MyWorkBoard.Entities.Tables;

// Tworzymy aplikacje web
var builder = WebApplication.CreateBuilder(args);

// Konfiguracja rozszerzenia swagera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// 2 sposób po³¹czenia z baz¹ danych
// Nadpisanie definicji konfiguracji dbcontext na poziomie kontenera dependency injection
// Generyczny parametr przyjmuje typ danego dbcontextu
builder.Services.AddDbContext<MyBoardsContext>(
        option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyWorkBoardsConnectionString")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Automatyzacja migracji
// 1. Stworzenie obiektu. W kontenerze DI dbcontext jest rejestrowany z d³ugoœci¹ ¿ycia scope
var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

// Zmienna users jest list¹ u¿ytkowników, którzy znajduj¹ siê w bazie danych
var users = dbContext.Users.ToList(); 
if (!users.Any())
{
    var user1 = new User()
    {
        FirstName = "User",
        LastName = "One",
        Email = "example@gmail.com",
        Address = new Address()
        {
            Country = "Poland",
            City = "Warszawa",
            Street = "W³oszczowa 34",
            PostalCode ="55-105"
        }
    };

    var user2 = new User()
    {
        FirstName = "User",
        LastName = "Two",
        Email = "elpmaxe@gmail.com",
        Address = new Address()
        {
            Country = "England",
            City = "London",
            Street = "Baker Street 66",
            PostalCode = "102-305"
        }
    };

    dbContext.Users.AddRange(user1, user2);
    dbContext.SaveChanges();
}

// Wstrzykiwanie MyBoardsContext (jest zarejstrowany wyzej) do endpointu
app.MapGet("data", (MyBoardsContext db) =>
    {
        // Najprostsza implementacja metody pobrania tagów z bazy danych i wsadzenia ich do listy
        //var tags = db.Tags.ToList();
        //return tags;

        var epic = db.Epics.First();
        var user = db.Users.First(u => u.LastName == "Two");
        return new { epic, user };
    });


app.Run();

// ¯eby odtworzyæ bazê danych SQL w C# trzeba utworzyæ klasy które bêd¹ reprezentowa³y definicjê
// konkretnych tabel (co najmniej 5 wed³ug diagramu). Bêd¹ znajdowaæ siê w oddzielnym folderze Entities
// 1. Odwzoruj tabele
// 2. Zainstalowaæ konkretne paczki entity framework aby skonfigurowaæ po³¹czenie z baz¹ danych i aby dodaæ klasê reprezentuj¹c¹
// kontekst ca³ej bazy