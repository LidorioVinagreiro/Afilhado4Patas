using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Afilhado4Patas.Migrations
{
    public partial class tarefa3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Tarefa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FuncionarioId = table.Column<string>(nullable: true),
                    Inicio = table.Column<DateTime>(nullable: false),
                    Fim = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    Completada = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefa", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Perfil_PerfilId",
                table: "AspNetUsers",
                column: "PerfilId",
                principalTable: "Perfil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Perfil_PerfilId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tarefa");

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
    }
}
