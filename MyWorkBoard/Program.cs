using Microsoft.EntityFrameworkCore;
using MyWorkBoard.Entities;



// Bazowy projekt do kt�ego dodam encje i konfiguracje baz danych przez entity
// Tworzymy aplikacje web
var builder = WebApplication.CreateBuilder(args);

// Konfiguracja rozszerzenia swagera
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

// 2 spos�b po��czenia z baz� danych
// nadpisanie definicji konfiguracji dbcontext na poziomie kontenera dependency injection
// generyczny parametr przyjmuje typ danego dbcontextu
builder.Services.AddDbContext<MyBoardsContext>(
        option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyWorkBoardConnectionString"))
    ); ;







// �eby odtworzy� baz� danych SQL w C# trzeba utworzy� klasy kt�re b�d� reprezentowa�y definicj�
// konkretnych tabel (co najmniej 5 wed�ug diagramu). B�d� znajdowa� si� w oddzielnym folderze Entities
// 1. Odwzoruj tabele
// 2. Zainstalowa� konkretne paczki entity framework aby skonfigurowa� po��czenie z baz� danych i aby doda� klas� reprezentuj�c�
// kontekst ca�ej bazy