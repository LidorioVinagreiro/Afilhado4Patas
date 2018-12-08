using Microsoft.EntityFrameworkCore.Migrations;

namespace Afilhado4Patas.Migrations
{
    public partial class TabelaTarefa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UtilizadorId",
                table: "Tarefa",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tarefa_UtilizadorId",
                table: "Tarefa",
                column: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefa_AspNetUsers_UtilizadorId",
                table: "Tarefa",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tarefa_AspNetUsers_UtilizadorId",
                table: "Tarefa");

            migrationBuilder.DropIndex(
                name: "IX_Tarefa_UtilizadorId",
                table: "Tarefa");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "Tarefa");
        }
    }
}
