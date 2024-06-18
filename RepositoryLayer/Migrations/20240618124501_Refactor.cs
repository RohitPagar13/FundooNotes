using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelNote");

            migrationBuilder.CreateIndex(
                name: "IX_NoteLabels_labelId",
                table: "NoteLabels",
                column: "labelId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLabels_labels_labelId",
                table: "NoteLabels",
                column: "labelId",
                principalTable: "labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLabels_notes_noteId",
                table: "NoteLabels",
                column: "noteId",
                principalTable: "notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteLabels_labels_labelId",
                table: "NoteLabels");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLabels_notes_noteId",
                table: "NoteLabels");

            migrationBuilder.DropIndex(
                name: "IX_NoteLabels_labelId",
                table: "NoteLabels");

            migrationBuilder.CreateTable(
                name: "LabelNote",
                columns: table => new
                {
                    labelNotesId = table.Column<int>(type: "int", nullable: false),
                    noteLablesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelNote", x => new { x.labelNotesId, x.noteLablesId });
                    table.ForeignKey(
                        name: "FK_LabelNote_labels_noteLablesId",
                        column: x => x.noteLablesId,
                        principalTable: "labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelNote_notes_labelNotesId",
                        column: x => x.labelNotesId,
                        principalTable: "notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelNote_noteLablesId",
                table: "LabelNote",
                column: "noteLablesId");
        }
    }
}
