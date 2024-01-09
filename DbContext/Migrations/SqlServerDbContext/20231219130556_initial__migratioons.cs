using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class initial__migratioons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csAttractionDbMcsCommentDbM",
                schema: "supusr");

            migrationBuilder.AddColumn<Guid>(
                name: "AttractionId",
                schema: "supusr",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AttractionId",
                schema: "supusr",
                table: "Comments",
                column: "AttractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Attractions_AttractionId",
                schema: "supusr",
                table: "Comments",
                column: "AttractionId",
                principalSchema: "supusr",
                principalTable: "Attractions",
                principalColumn: "AttractionID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Attractions_AttractionId",
                schema: "supusr",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AttractionId",
                schema: "supusr",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AttractionId",
                schema: "supusr",
                table: "Comments");

            migrationBuilder.CreateTable(
                name: "csAttractionDbMcsCommentDbM",
                schema: "supusr",
                columns: table => new
                {
                    AttractionsDbMAttractionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentsDbMCommentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csAttractionDbMcsCommentDbM", x => new { x.AttractionsDbMAttractionID, x.CommentsDbMCommentID });
                    table.ForeignKey(
                        name: "FK_csAttractionDbMcsCommentDbM_Attractions_AttractionsDbMAttractionID",
                        column: x => x.AttractionsDbMAttractionID,
                        principalSchema: "supusr",
                        principalTable: "Attractions",
                        principalColumn: "AttractionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_csAttractionDbMcsCommentDbM_Comments_CommentsDbMCommentID",
                        column: x => x.CommentsDbMCommentID,
                        principalSchema: "supusr",
                        principalTable: "Comments",
                        principalColumn: "CommentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_csAttractionDbMcsCommentDbM_CommentsDbMCommentID",
                schema: "supusr",
                table: "csAttractionDbMcsCommentDbM",
                column: "CommentsDbMCommentID");
        }
    }
}
