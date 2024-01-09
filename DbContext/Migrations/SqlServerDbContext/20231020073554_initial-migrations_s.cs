using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class initialmigrations_s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Cities_CityID",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.DropIndex(
                name: "IX_Attractions_CityID",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "CityID",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "supusr",
                table: "Attractions",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "supusr",
                table: "Attractions",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.AddColumn<Guid>(
                name: "CityID",
                schema: "supusr",
                table: "Attractions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_CityID",
                schema: "supusr",
                table: "Attractions",
                column: "CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Cities_CityID",
                schema: "supusr",
                table: "Attractions",
                column: "CityID",
                principalSchema: "supusr",
                principalTable: "Cities",
                principalColumn: "CityID");
        }
    }
}
