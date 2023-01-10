using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyWorkBoard.Entities.Model;
using MyWorkBoard.Entities.Tables;
using System.Text.Json.Serialization;

// Tworzymy aplikacje web
var builder = WebApplication.CreateBuilder(args);

// Konfiguracja rozszerzenia swagera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Taka konfiguracja spowoduje, �e podczas zwracania rezultatu z tego APi to mimoo tego, �e w obiekcie w programie b�d� zap�tlone referencje
// to serializator je zigonruje. Problem z powi�zanymi encjami z entity framework
builder.Services.Configure<JsonOptions>(options => {
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
    

// 2 spos�b po��czenia z baz� danych
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
// 1. Stworzenie obiektu. W kontenerze DI dbcontext jest rejestrowany z d�ugo�ci� �ycia scope
var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();

var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

// Zmienna users jest list� u�ytkownik�w, kt�rzy znajduj� si� w bazie danych
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
            Street = "W�oszczowa 34",
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
        // Najprostsza implementacja metody pobrania tag�w z bazy danych i wsadzenia ich do listy
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

        //var authorsCommentCounts = await db.Comments
        //    .GroupBy(g => g.AuthorId)
        //    .Select(s => new { s.Key, Count = s.Count() })
        //    .ToListAsync();

        //// Informacje o liczbie komentarzy
        //var topAuthor = authorsCommentCounts
        //    .First(f => f.Count == authorsCommentCounts.Max(m => m.Count));

        //// Szczeg�y o danym u�ytkowniku
        //var userDetails = db.Users.First(f => f.Id == topAuthor.Key);

        //return new { userDetails, commentCount = topAuthor.Count };

        var user = await db.Users
            // metoda include sprawi, �e automatycznie do obiektu zostan� do��czone powi�zane dane a po stronie sql b�dzie jedno zapytanie z join (szybsze)
            // w postaci lambdy wska�emy jak� w�a�ciwo�� chcemy do��czy� do encji user (pobranie komentarzy u�ytkownika)
            .Include(u => u.Comments).ThenInclude(c => c.WorkItem)
            .Include(u => u.Address)
            .FirstAsync(u => u.Id == Guid.Parse("91106229-4E24-4C9C-47C3-08DA10AB0E20"));

        return user;
    });

// 1 parametr to �cie�ka pod jak� ten endpoint b�dzie dost�pny, 2 parametr to delegata, kt�ra obs�u�y takie zapytanie 
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
    //Tag mvctag = new Tag()
    //{
    //    TagValue = "MVC"
    //};

    //Tag asptag = new Tag()
    //{
    //    TagValue = "ASP"
    //};

    //var tags = new List<Tag>() { mvctag, asptag };
    //await db.Tags.AddRangeAsync(tags);
    //await db.SaveChangesAsync();

    //return tags;

    // Utworzenie wierszy, kt�re s� powi�zane relacj�
    var adress = new Address()
    {
        Id = Guid.Parse("91106229-4E24-4C9C-47C3-08DA10AB0E20"),
        City = "Krak�w",
        Country = "Poland",
        Street ="D�uga"
    };

    var user = new User()
    {
        Email = "adduser22@gmail.com",
        FirstName = "Test",
        LastName = "User22",
        Address = adress,
    };

    // Entity ogarnie, �e user jest powi�zany z adresem i niejawnie doda do bazy danych
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return user;
});

app.MapPost("delete", async (MyBoardsContext db) =>
{
    var workitemTagsResult = await db.WorkItemTag.Where(w => w.WorkItemId == 12).ToListAsync();
    db.WorkItemTag.RemoveRange(workitemTagsResult);

    var workItemResult = await db.WorkItems.FirstAsync(w => w.Id == 5);
    db.RemoveRange(workItemResult);

    await db.SaveChangesAsync();
});

app.Run();

// �eby odtworzy� baz� danych SQL w C# trzeba utworzy� klasy kt�re b�d� reprezentowa�y definicj�
// konkretnych tabel (co najmniej 5 wed�ug diagramu). B�d� znajdowa� si� w oddzielnym folderze Entities
// 1. Odwzoruj tabele
// 2. Zainstalowa� konkretne paczki entity framework aby skonfigurowa� po��czenie z baz� danych i aby doda� klas� reprezentuj�c�
// kontekst ca�ej bazy