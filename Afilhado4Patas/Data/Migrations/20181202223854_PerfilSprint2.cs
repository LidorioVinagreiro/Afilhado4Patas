using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Afilhado4Patas.Data.Migrations
{
    public partial class PerfilSprint2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PerfilId",
                table: "AspNetUsers",
                nullable: true);
            /*
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");
                */
            migrationBuilder.CreateTable(
                name: "PerfilTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UtilizadorId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Postalcode = table.Column<string>(nullable: true),
                    NIF = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Genre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilTable", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PerfilId",
                table: "AspNetUsers",
                column: "PerfilId",
                unique: true,
                filter: "[PerfilId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PerfilTable_PerfilId",
                table: "AspNetUsers",
                column: "PerfilId",
                principalTable: "PerfilTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PerfilTable_PerfilId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PerfilTable");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PerfilId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "AspNetUsers");
            /*
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");*/
        }
    }
}
