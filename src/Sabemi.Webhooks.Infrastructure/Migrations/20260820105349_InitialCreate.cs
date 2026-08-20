using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sabemi.Webhooks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventosBrutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTransacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdContrato = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusRecebido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecebidoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Processado = table.Column<bool>(type: "bit", nullable: false),
                    ErroProcessamento = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosBrutos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusContratos",
                columns: table => new
                {
                    IdContrato = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UltimoIdTransacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusAtual = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValorPago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataUltimoPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusContratos", x => x.IdContrato);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventosBrutos_IdTransacao",
                table: "EventosBrutos",
                column: "IdTransacao",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventosBrutos");

            migrationBuilder.DropTable(
                name: "StatusContratos");
        }
    }
}
