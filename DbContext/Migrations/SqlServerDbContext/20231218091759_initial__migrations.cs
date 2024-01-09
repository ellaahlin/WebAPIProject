using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class initial__migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Cities_CityID",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Email");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "supusr",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CityID",
                schema: "supusr",
                table: "Attractions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                schema: "supusr",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Cities_CityID",
                schema: "supusr",
                table: "Attractions",
                column: "CityID",
                principalSchema: "supusr",
                principalTable: "Cities",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                schema: "supusr",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Cities_CityID",
                schema: "supusr",
                table: "Attractions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                schema: "supusr",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                schema: "supusr",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "supusr",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Password");

            migrationBuilder.AlterColumn<Guid>(
                name: "CityID",
                schema: "supusr",
                table: "Attractions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
