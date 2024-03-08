using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crud_colaborativo.Migrations
{
    /// <inheritdoc />
    public partial class AddContractEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TipoCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoContrato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Socio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gerente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senior = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocioParticipacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocioComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropuestaContrato = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratos");
        }
    }
}
