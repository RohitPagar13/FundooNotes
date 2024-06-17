using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class NoteLabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelNote_Label_noteLablesId",
                table: "LabelNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Label",
                table: "Label");

            migrationBuilder.RenameTable(
                name: "Label",
                newName: "labels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_labels",
                table: "labels",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "NoteLabel",
                columns: table => new
                {
                    noteId = table.Column<int>(type: "int", nullable: false),
                    labelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteLabel", x => new { x.noteId, x.labelId });
                });

            migrationBuilder.AddForeignKey(
                name: "FK_LabelNote_labels_noteLablesId",
                table: "LabelNote",
                column: "noteLablesId",
                principalTable: "labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelNote_labels_noteLablesId",
                table: "LabelNote");

            migrationBuilder.DropTable(
                name: "NoteLabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_labels",
                table: "labels");

            migrationBuilder.RenameTable(
                name: "labels",
                newName: "Label");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Label",
                table: "Label",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelNote_Label_noteLablesId",
                table: "LabelNote",
                column: "noteLablesId",
                principalTable: "Label",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
