using Microsoft.EntityFrameworkCore;
using MyWorkBoard.Entities.Model;

// Tworzymy aplikacje web
var builder = WebApplication.CreateBuilder(args);

// Konfiguracja rozszerzenia swagera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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


app.Run();

// �eby odtworzy� baz� danych SQL w C# trzeba utworzy� klasy kt�re b�d� reprezentowa�y definicj�
// konkretnych tabel (co najmniej 5 wed�ug diagramu). B�d� znajdowa� si� w oddzielnym folderze Entities
// 1. Odwzoruj tabele
// 2. Zainstalowa� konkretne paczki entity framework aby skonfigurowa� po��czenie z baz� danych i aby doda� klas� reprezentuj�c�
// kontekst ca�ej bazy