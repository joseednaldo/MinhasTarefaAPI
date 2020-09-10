using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MinhasTarefaAPI.Migrations
{
    public partial class token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RefleshToken = table.Column<string>(nullable: true),
                    usuarioId = table.Column<string>(nullable: true),
                    Utilizado = table.Column<bool>(nullable: false),
                    Criado = table.Column<DateTime>(nullable: false),
                    Atualizado = table.Column<DateTime>(nullable: true),
                    ExpirationToken = table.Column<DateTime>(nullable: false),
                    ExpirationRefleshToken = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_AspNetUsers_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Token_usuarioId",
                table: "Token",
                column: "usuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Token");
        }
    }
}
