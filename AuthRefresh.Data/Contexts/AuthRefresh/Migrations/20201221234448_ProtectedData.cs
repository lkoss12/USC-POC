using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthRefresh.Data.Contexts.AuthRefresh.Migrations
{
    public partial class ProtectedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProtectedDatas",
                columns: table => new
                {
                    ProtectedDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectedDatas", x => x.ProtectedDataId);
                });

            migrationBuilder.CreateTable(
                name: "UserProtectedDatas",
                columns: table => new
                {
                    UserProtectedDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProtectedDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProtectedDatas", x => x.UserProtectedDataId);
                    table.ForeignKey(
                        name: "FK_UserProtectedDatas_ProtectedDatas_ProtectedDataId",
                        column: x => x.ProtectedDataId,
                        principalTable: "ProtectedDatas",
                        principalColumn: "ProtectedDataId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProtectedDatas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProtectedDatas_ProtectedDataId",
                table: "UserProtectedDatas",
                column: "ProtectedDataId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProtectedDatas_UserId",
                table: "UserProtectedDatas",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProtectedDatas");

            migrationBuilder.DropTable(
                name: "ProtectedDatas");
        }
    }
}
