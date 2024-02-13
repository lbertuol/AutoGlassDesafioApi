using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoGlassDesafioApi.Infrastructure.Migrations
{
    public partial class v100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SequencialProduto");

            migrationBuilder.CreateTable(
                name: "produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR SequencialProduto"),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    DataFabricacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FornecedorCodigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FornecedorDescricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FornecedorCNPJ = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produtos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produtos");

            migrationBuilder.DropSequence(
                name: "SequencialProduto");
        }
    }
}
