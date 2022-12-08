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

// Wstrzykiwanie MyBoardsContext (jest zarejstrowany wyzej) do endpointu. Mapp get -> pobieranie
app.MapGet("data", async (MyBoardsContext db) =>
    {
        // Najprostsza implementacja metody pobrania tagów z bazy danych i wsadzenia ich do listy
        //var tags = db.Tags.ToList();
        //return tags;

        //var epic = db.Epics.First();
        //var user = db.Users.First(u => u.LastName == "Two");
        //return new { epic, user };

        //var toDoWorkItems = db.WorkItems.Where(w => w.StateId == 1).ToList();
        //return new { toDoWorkItems };

        //var newComments = await db
        //    .Comments
        //    .Where(c => c.CreatedDate > new DateTime(2022, 7, 23))
        //    .ToListAsync();
        //return newComments;

        //var top5NewestComments = await db.Comments
        //    .OrderByDescending(c => c.CreatedDate)
        //    .Take(5)
        //    .ToListAsync();
        //return top5NewestComments;

        //var statesCount = await db.WorkItems
        //    .GroupBy(g => g.StateId)
        //    .Select(s => new { stateId = s.Key, count = s.Count() })
        //    .ToListAsync();
        //return statesCount;

        //var selectedEpics = await db.Epics
        //.Where(w => w.StateId == 4)
        //.OrderBy(o => o.Priority)
        //.ToListAsync();
        //return selectedEpics;

        var authorsCommentCounts = await db.Comments
            .GroupBy(g => g.AuthorId)
            .Select(s => new { s.Key, Count = s.Count() })
            .ToListAsync();

        // Informacje o liczbie komentarzy
        var topAuthor =  authorsCommentCounts
            .First(f => f.Count == authorsCommentCounts.Max(m => m.Count));

        // Szczegó³y o danym u¿ytkowniku
        var userDetails = db.Users.First(f => f.Id == topAuthor.Key);

        return new { userDetails, commentCount = topAuthor.Count };
    });

// 1 parametr to œcie¿ka pod jak¹ ten endpoint bêdzie dostêpny, 2 parametr to delegata, która obs³u¿y takie zapytanie 
app.MapPost("update", async (MyBoardsContext db) =>
    {
        Epic epic = await db.Epics.FirstAsync(epic => epic.Id == 1);

        var onHoldState = await db.States.FirstAsync(f => f.StateValue == "On Hold");

        //epic.Area = "Updated Area";
        //epic.Priority = 1;
        //epic.StartDate = DateTime.Now;
            
        epic.StateId = onHoldState.Id;

        await db.SaveChangesAsync();

        return epic;
    });

app.MapPost("create", async (MyBoardsContext db) =>
{
    Tag tag = new Tag()
    {
        TagValue = "EF"
    };

    // 1 sposob
    // await db.AddAsync(tag);

    // 2 sposob
    await db.Tags.AddAsync(tag);

    await db.SaveChangesAsync();

    return tag;
});

app.Run();

// ¯eby odtworzyæ bazê danych SQL w C# trzeba utworzyæ klasy które bêd¹ reprezentowa³y definicjê
// konkretnych tabel (co najmniej 5 wed³ug diagramu). Bêd¹ znajdowaæ siê w oddzielnym folderze Entities
// 1. Odwzoruj tabele
// 2. Zainstalowaæ konkretne paczki entity framework aby skonfigurowaæ po³¹czenie z baz¹ danych i aby dodaæ klasê reprezentuj¹c¹
// kontekst ca³ej bazy