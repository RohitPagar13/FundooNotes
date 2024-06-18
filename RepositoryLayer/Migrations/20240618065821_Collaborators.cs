using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class Collaborators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NoteLabel",
                table: "NoteLabel");

            migrationBuilder.RenameTable(
                name: "NoteLabel",
                newName: "NoteLabels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoteLabels",
                table: "NoteLabels",
                columns: new[] { "noteId", "labelId" });

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collaborators_notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_NoteId",
                table: "Collaborators",
                column: "NoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collaborators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NoteLabels",
                table: "NoteLabels");

            migrationBuilder.RenameTable(
                name: "NoteLabels",
                newName: "NoteLabel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoteLabel",
                table: "NoteLabel",
                columns: new[] { "noteId", "labelId" });
        }
    }
}
