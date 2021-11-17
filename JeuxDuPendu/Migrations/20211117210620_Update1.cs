using Microsoft.EntityFrameworkCore.Migrations;

namespace JeuxDuPendu.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AsyncClient",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AsyncServerName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsyncClient", x => x.Name);
                    table.ForeignKey(
                        name: "FK_AsyncClient_servers_AsyncServerName",
                        column: x => x.AsyncServerName,
                        principalTable: "servers",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsyncClient_AsyncServerName",
                table: "AsyncClient",
                column: "AsyncServerName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsyncClient");
        }
    }
}
