using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Afilhado4Patas.Data.Migrations
{
    public partial class Mario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorId",
                table: "PerfilTable",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EditarPerfilViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Postalcode = table.Column<string>(nullable: true),
                    NIF = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    OldPassword = table.Column<string>(nullable: false),
                    NewPassword = table.Column<string>(maxLength: 100, nullable: false),
                    ConfirmPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditarPerfilViewModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditarPerfilViewModel");

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorId",
                table: "PerfilTable",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
