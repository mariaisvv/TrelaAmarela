using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trela_Amarela.Data.Migrations
{
    public partial class Box_RelacMN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Boxs",
                columns: new[] { "IdBox", "Dim_Box", "Nome" },
                values: new object[] { 1, "2x1", "Pequena" });

            migrationBuilder.InsertData(
                table: "Boxs",
                columns: new[] { "IdBox", "Dim_Box", "Nome" },
                values: new object[] { 2, "3x3", "Média" });

            migrationBuilder.InsertData(
                table: "Boxs",
                columns: new[] { "IdBox", "Dim_Box", "Nome" },
                values: new object[] { 3, "5x5", "Grande" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Boxs",
                keyColumn: "IdBox",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Boxs",
                keyColumn: "IdBox",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Boxs",
                keyColumn: "IdBox",
                keyValue: 3);
        }
    }
}
