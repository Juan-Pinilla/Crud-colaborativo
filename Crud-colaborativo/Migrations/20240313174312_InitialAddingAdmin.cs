using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crud_colaborativo.Migrations
{
    /// <inheritdoc />
    public partial class InitialAddingAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Contratos_ContratoId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ContratoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Contratos_ContratoId",
                table: "AspNetUsers",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Contratos_ContratoId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ContratoId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Contratos_ContratoId",
                table: "AspNetUsers",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
