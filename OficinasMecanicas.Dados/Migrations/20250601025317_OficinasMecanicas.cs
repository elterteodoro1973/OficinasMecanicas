using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class OficinasMecanicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OficinaMecanica",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Nome = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    Endereco = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    Servicos = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OficinaMecanica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicosPrestados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Username = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(512)", unicode: false, maxLength: 512, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgendamentoVisita",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataHora = table.Column<DateTime>(type: "datetime", nullable: true),
                    Descricao = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    IdOficina = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendamentosVisita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgendamentoVisita_OficinaMecanica",
                        column: x => x.IdOficina,
                        principalTable: "OficinaMecanica",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AgendamentoVisita_Usuarios",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResetarSenha",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Token = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "datetime", nullable: false),
                    Efetivado = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Excluido = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetarSenha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResetarSenha_Usuarios",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoVisita_IdOficina",
                table: "AgendamentoVisita",
                column: "IdOficina");

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoVisita_IdUsuario",
                table: "AgendamentoVisita",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ResetarSenha_UsuarioId",
                table: "ResetarSenha",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgendamentoVisita");

            migrationBuilder.DropTable(
                name: "ResetarSenha");

            migrationBuilder.DropTable(
                name: "ServicosPrestados");

            migrationBuilder.DropTable(
                name: "OficinaMecanica");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
