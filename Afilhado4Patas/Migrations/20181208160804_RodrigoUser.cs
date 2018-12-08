using Microsoft.EntityFrameworkCore.Migrations;

namespace Afilhado4Patas.Migrations
{
    public partial class RodrigoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Perfil_PerfilId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Perfil",
                table: "Perfil");

            migrationBuilder.RenameTable(
                name: "Perfil",
                newName: "PerfilTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerfilTable",
                table: "PerfilTable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PerfilTable_PerfilId",
                table: "AspNetUsers",
                column: "PerfilId",
                principalTable: "PerfilTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PerfilTable_PerfilId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerfilTable",
                table: "PerfilTable");

            migrationBuilder.RenameTable(
                name: "PerfilTable",
                newName: "Perfil");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Perfil",
                table: "Perfil",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Perfil_PerfilId",
                table: "AspNetUsers",
                column: "PerfilId",
                principalTable: "Perfil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
