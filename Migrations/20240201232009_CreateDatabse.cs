using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoFinal.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cargos",
                columns: table => new
                {
                    codCargo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomeCargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    salarioBase = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargos", x => x.codCargo);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    codCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomeCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefoneCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    emailCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    enderecoCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descricaoCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PessFCPFCliente = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PessJCNPJCliente = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    statusCliente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.codCliente);
                });

            migrationBuilder.CreateTable(
                name: "departamentos",
                columns: table => new
                {
                    codDepartamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomeDepartamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    responsavelDepartamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departamentos", x => x.codDepartamento);
                });

            migrationBuilder.CreateTable(
                name: "funcionarios",
                columns: table => new
                {
                    codFuncionario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fkCodCargocodCargo = table.Column<int>(type: "int", nullable: true),
                    idDepartamento = table.Column<int>(type: "int", nullable: false),
                    fkCodDepartamentocodDepartamento = table.Column<int>(type: "int", nullable: true),
                    nomeFuncionario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefoneFuncionario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    emailFuncionario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    enderecoFuncionario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CPFFuncionario = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    tipoContrFuncionario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    modoTrabFuncionario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    formacaoRelevanteFuncionario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    statusFuncionario = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_funcionarios", x => x.codFuncionario);
                    table.ForeignKey(
                        name: "FK_funcionarios_cargos_fkCodCargocodCargo",
                        column: x => x.fkCodCargocodCargo,
                        principalTable: "cargos",
                        principalColumn: "codCargo");
                    table.ForeignKey(
                        name: "FK_funcionarios_departamentos_fkCodDepartamentocodDepartamento",
                        column: x => x.fkCodDepartamentocodDepartamento,
                        principalTable: "departamentos",
                        principalColumn: "codDepartamento");
                });

            migrationBuilder.CreateTable(
                name: "projetos",
                columns: table => new
                {
                    codProjeto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codDepartamento = table.Column<int>(type: "int", nullable: false),
                    idCliente = table.Column<int>(type: "int", nullable: false),
                    nomeProjeto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descricaoProjeto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    statusProjeto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    valorProjeto = table.Column<float>(type: "real", nullable: false),
                    dataEntregaProjeto = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projetos", x => x.codProjeto);
                    table.ForeignKey(
                        name: "FK_projetos_clientes_idCliente",
                        column: x => x.idCliente,
                        principalTable: "clientes",
                        principalColumn: "codCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_projetos_departamentos_codDepartamento",
                        column: x => x.codDepartamento,
                        principalTable: "departamentos",
                        principalColumn: "codDepartamento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contasBancarias",
                columns: table => new
                {
                    codContaB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codFuncionario = table.Column<int>(type: "int", nullable: false),
                    fkCodFuncionariocodFuncionario = table.Column<int>(type: "int", nullable: true),
                    agenciaContaB = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    numeroContaB = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    tipoContaB = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contasBancarias", x => x.codContaB);
                    table.ForeignKey(
                        name: "FK_contasBancarias_funcionarios_fkCodFuncionariocodFuncionario",
                        column: x => x.fkCodFuncionariocodFuncionario,
                        principalTable: "funcionarios",
                        principalColumn: "codFuncionario");
                });

            migrationBuilder.CreateTable(
                name: "ProjetoFuncionario",
                columns: table => new
                {
                    idProjeto = table.Column<int>(type: "int", nullable: false),
                    idFuncionario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkProjetoFuncionario", x => new { x.idProjeto, x.idFuncionario });
                    table.ForeignKey(
                        name: "fkFuncionario",
                        column: x => x.idFuncionario,
                        principalTable: "funcionarios",
                        principalColumn: "codFuncionario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fkProjeto",
                        column: x => x.idProjeto,
                        principalTable: "projetos",
                        principalColumn: "codProjeto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoFuncionario_idFuncionario",
                table: "ProjetoFuncionario",
                column: "idFuncionario");

            migrationBuilder.CreateIndex(
                name: "IX_contasBancarias_fkCodFuncionariocodFuncionario",
                table: "contasBancarias",
                column: "fkCodFuncionariocodFuncionario");

            migrationBuilder.CreateIndex(
                name: "IX_funcionarios_fkCodCargocodCargo",
                table: "funcionarios",
                column: "fkCodCargocodCargo");

            migrationBuilder.CreateIndex(
                name: "IX_funcionarios_fkCodDepartamentocodDepartamento",
                table: "funcionarios",
                column: "fkCodDepartamentocodDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_projetos_codDepartamento",
                table: "projetos",
                column: "codDepartamento");

            migrationBuilder.CreateIndex(
                name: "IX_projetos_idCliente",
                table: "projetos",
                column: "idCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjetoFuncionario");

            migrationBuilder.DropTable(
                name: "contasBancarias");

            migrationBuilder.DropTable(
                name: "projetos");

            migrationBuilder.DropTable(
                name: "funcionarios");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "cargos");

            migrationBuilder.DropTable(
                name: "departamentos");
        }
    }
}
