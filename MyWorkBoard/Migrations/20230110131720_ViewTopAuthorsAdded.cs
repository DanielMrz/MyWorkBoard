﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWorkBoard.Migrations
{
    /// <inheritdoc />
    public partial class ViewTopAuthorsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW View_TopAuthors AS
                SELECT TOP 5 u.FirstName, u.LastName, COUNT(*) as [WorkItemsCreated]
                FROM Users u
                JOIN WorkItems wi on wi.AuthorId = u.Id
                GROUP BY u.Id, u.FirstName, u.LastName
                ORDER BY [WorkItemsCreated] DESC
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP VIEW View_TopAuthors
            ");
        }
    }
}
