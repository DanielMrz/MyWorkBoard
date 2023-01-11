using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWorkBoard.Migrations
{
    /// <inheritdoc />
    public partial class TestingNewConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Coordinate_Longitude",
                table: "Addresses",
                type: "decimal(18,7)",
                precision: 18,
                scale: 7,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Coordinate_Latitude",
                table: "Addresses",
                type: "decimal(18,7)",
                precision: 18,
                scale: 7,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Coordinate_Longitude",
                table: "Addresses",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,7)",
                oldPrecision: 18,
                oldScale: 7,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Coordinate_Latitude",
                table: "Addresses",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,7)",
                oldPrecision: 18,
                oldScale: 7,
                oldNullable: true);
        }
    }
}
