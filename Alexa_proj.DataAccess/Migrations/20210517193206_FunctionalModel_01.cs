using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexa_proj.Migrations
{
    public partial class FunctionalModel_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Executables",
                columns: table => new
                {
                    executable_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    executable_name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executables", x => x.executable_id);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    function_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutableId = table.Column<int>(type: "int", nullable: false),
                    function_name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    function_endpoint = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.function_id);
                    table.ForeignKey(
                        name: "FK_Functions_Executables_ExecutableId",
                        column: x => x.ExecutableId,
                        principalTable: "Executables",
                        principalColumn: "executable_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    keyword_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutableId = table.Column<int>(type: "int", nullable: false),
                    keyword_value = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IsCritical = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.keyword_id);
                    table.ForeignKey(
                        name: "FK_Keywords_Executables_ExecutableId",
                        column: x => x.ExecutableId,
                        principalTable: "Executables",
                        principalColumn: "executable_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FunctionResults",
                columns: table => new
                {
                    result_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionId = table.Column<int>(type: "int", nullable: false),
                    result_value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionResults", x => x.result_id);
                    table.ForeignKey(
                        name: "FK_FunctionResults_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "function_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionResults_FunctionId",
                table: "FunctionResults",
                column: "FunctionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functions_ExecutableId",
                table: "Functions",
                column: "ExecutableId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_ExecutableId",
                table: "Keywords",
                column: "ExecutableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunctionResults");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Executables");
        }
    }
}
