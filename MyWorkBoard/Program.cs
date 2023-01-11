using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyWorkBoard.Entities.Model;
using MyWorkBoard.Entities.Model.Dto;
using MyWorkBoard.Entities.Tables;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

// Tworzymy aplikacje web
var builder = WebApplication.CreateBuilder(args);

// Konfiguracja rozszerzenia swagera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Taka konfiguracja spowoduje, ¿e podczas zwracania rezultatu z tego APi to mimoo tego, ¿e w obiekcie w programie bêd¹ zapêtlone referencje
// to serializator je zigonruje. Problem z powi¹zanymi encjami z entity framework
builder.Services.Configure<JsonOptions>(options => {
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
    

// 2 sposób po³¹czenia z baz¹ danych
// Nadpisanie definicji konfiguracji dbcontext na poziomie kontenera dependency injection
// Generyczny parametr przyjmuje typ danego dbcontextu
builder.Services.AddDbContext<MyBoardsContext>(
        option => option
        .UseLazyLoadingProxies(true)
        .UseSqlServer(builder.Configuration.GetConnectionString("MyWorkBoardsConnectionString"))
);

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


app.MapGet("pagination", async (MyBoardsContext db) =>
{
    // Informacje od u¿ytkownika
    // 1. Filter (wartoœæ któr¹ poda u¿ytkownik przez input do filtrowania)
    // 2. Pod stringiem mo¿e ustawiæ informacje o kolumnie po której chcia³by sortowaæ
    // 3. Czy sortowanie jest malej¹ce czy rosn¹ce
    // 4. Numer strony
    // 5. Liczba wyników, któr¹ chce wyœwietliæ u¿ytkownik

    var filter = "a";
    string sortBy = "LastName";
    bool sortByDescending = false;
    int pageNumber = 1; 
    int pageSize = 10;

    var query = db.Users
        .Where(u => filter == null || (u.Email.ToLower().Contains(filter.ToLower()) ||
                                       u.FirstName.ToLower().Contains(filter.ToLower()) ||
                                       u.LastName.ToLower().Contains(filter.ToLower())));

    // Ca³kowita liczba elementów spe³niaj¹cych filtrowanie
    var totalCount = query.Count();

    // Sortowanie
    if(sortBy != null)
    {
        // Przyk³adowa ekspresja
        // Expression<Func<User, object>> sortByExpression = user => user.Email;

        // S³ownik dla typu string dla typu expression func user object
        // Okreœla po jakich kolumnach mo¿emy sortowaæ
        // W tym s³owniku kluczem by³a nazwa kloumny a wartoœci¹ by³o wyra¿enie czyli expression dla typu Func typu User i typu Object
        // które jest prawid³owym parametrem zarówno dla metody orderBy i orderByDescending
        var columnSelector = new Dictionary<string, Expression<Func<User, object>>>
        {
            { nameof(User.Email), user => user.Email },
            { nameof(User.FirstName), user => user.FirstName },
            { nameof(User.LastName), user => user.LastName }
        };

        var sortByExpression = columnSelector[sortBy];
        query = sortByDescending
            ? query.OrderByDescending(sortByExpression)
            : query.OrderBy(sortByExpression);
    }

    var result = query.Skip(pageSize * (pageNumber - 1))
                 .Take(pageSize)
                 .ToList();

    var pagedResult = new PagedResult<User>(result, totalCount, pageSize, pageNumber);
    return pagedResult;
});

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

        //var authorsCommentCounts = await db.Comments
        //    .GroupBy(g => g.AuthorId)
        //    .Select(s => new { s.Key, Count = s.Count() })
        //    .ToListAsync();

        //// Informacje o liczbie komentarzy
        //var topAuthor = authorsCommentCounts
        //    .First(f => f.Count == authorsCommentCounts.Max(m => m.Count));

        //// Szczegó³y o danym u¿ytkowniku
        //var userDetails = db.Users.First(f => f.Id == topAuthor.Key);

        //return new { userDetails, commentCount = topAuthor.Count };

        //var user = await db.Users
        //    // metoda include sprawi, ¿e automatycznie do obiektu zostan¹ do³¹czone powi¹zane dane a po stronie sql bêdzie jedno zapytanie z join (szybsze)
        //    // w postaci lambdy wska¿emy jak¹ w³aœciwoœæ chcemy do³¹czyæ do encji user (pobranie komentarzy u¿ytkownika)
        //    .Include(u => u.Comments).ThenInclude(c => c.WorkItem)
        //    .Include(u => u.Address)
        //    .FirstAsync(u => u.Id == Guid.Parse("91106229-4E24-4C9C-47C3-08DA10AB0E20"));

        var topAuthors = db.ViewTopAuthors.ToList();

        return topAuthors;
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

    // Utworzenie wierszy, które s¹ powi¹zane relacj¹
    var adress = new Address()
    {
        Id = Guid.Parse("91106229-4E24-4C9C-47C3-08DA10AB0E20"),
        City = "Kraków",
        Country = "Poland",
        Street ="D³uga"
    };

    var user = new User()
    {
        Email = "adduser22@gmail.com",
        FirstName = "Test",
        LastName = "User22",
        Address = adress,
    };

    // Entity ogarnie, ¿e user jest powi¹zany z adresem i niejawnie doda do bazy danych
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

// ¯eby odtworzyæ bazê danych SQL w C# trzeba utworzyæ klasy które bêd¹ reprezentowa³y definicjê
// konkretnych tabel (co najmniej 5 wed³ug diagramu). Bêd¹ znajdowaæ siê w oddzielnym folderze Entities
// 1. Odwzoruj tabele
// 2. Zainstalowaæ konkretne paczki entity framework aby skonfigurowaæ po³¹czenie z baz¹ danych i aby dodaæ klasê reprezentuj¹c¹
// kontekst ca³ej bazy