using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class initialmigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Cities_CityId",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.RenameColumn(
                name: "CityId",
                schema: "supusr",
                table: "Attractions",
                newName: "CityID");

            migrationBuilder.RenameIndex(
                name: "IX_Attractions_CityId",
                schema: "supusr",
                table: "Attractions",
                newName: "IX_Attractions_CityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Cities_CityID",
                schema: "supusr",
                table: "Attractions",
                column: "CityID",
                principalSchema: "supusr",
                principalTable: "Cities",
                principalColumn: "CityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Cities_CityID",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.RenameColumn(
                name: "CityID",
                schema: "supusr",
                table: "Attractions",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Attractions_CityID",
                schema: "supusr",
                table: "Attractions",
                newName: "IX_Attractions_CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Cities_CityId",
                schema: "supusr",
                table: "Attractions",
                column: "CityId",
                principalSchema: "supusr",
                principalTable: "Cities",
                principalColumn: "CityID");
        }
    }
}
