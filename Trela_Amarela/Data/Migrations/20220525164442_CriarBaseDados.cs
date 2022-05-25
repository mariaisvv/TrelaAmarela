using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trela_Amarela.Data.Migrations
{
    public partial class CriarBaseDados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boxs",
                columns: table => new
                {
                    IdBox = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dim_Box = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxs", x => x.IdBox);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    D_Nasc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CodPostal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NIF = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Animais",
                columns: table => new
                {
                    IdAnimal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DataNasc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Porte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Raca = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Vacinacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desparasitacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    N_Especiais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nr_registo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Nr_chip = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animais", x => x.IdAnimal);
                    table.ForeignKey(
                        name: "FK_Animais_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    IdReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    D_Entrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    D_Saida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nr_animais = table.Column<int>(type: "int", nullable: false),
                    Nr_registo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdBox = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK_Reservas_Boxs_IdBox",
                        column: x => x.IdBox,
                        principalTable: "Boxs",
                        principalColumn: "IdBox",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimaisReservas",
                columns: table => new
                {
                    ListaAnimaisIdAnimal = table.Column<int>(type: "int", nullable: false),
                    ListaReservasIdReserva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimaisReservas", x => new { x.ListaAnimaisIdAnimal, x.ListaReservasIdReserva });
                    table.ForeignKey(
                        name: "FK_AnimaisReservas_Animais_ListaAnimaisIdAnimal",
                        column: x => x.ListaAnimaisIdAnimal,
                        principalTable: "Animais",
                        principalColumn: "IdAnimal",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimaisReservas_Reservas_ListaReservasIdReserva",
                        column: x => x.ListaReservasIdReserva,
                        principalTable: "Reservas",
                        principalColumn: "IdReserva",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animais_IdCliente",
                table: "Animais",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_AnimaisReservas_ListaReservasIdReserva",
                table: "AnimaisReservas",
                column: "ListaReservasIdReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdBox",
                table: "Reservas",
                column: "IdBox");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdCliente",
                table: "Reservas",
                column: "IdCliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimaisReservas");

            migrationBuilder.DropTable(
                name: "Animais");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Boxs");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
