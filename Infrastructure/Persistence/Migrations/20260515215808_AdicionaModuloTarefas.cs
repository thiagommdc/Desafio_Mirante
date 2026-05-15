using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioMirante.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaModuloTarefas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Sku = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Preco = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    QuantidadeEstoque = table.Column<int>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    CriadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CriadoPor = table.Column<string>(type: "TEXT", nullable: false),
                    AtualizadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AtualizadoPor = table.Column<string>(type: "TEXT", nullable: true),
                    ExcluidoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExcluidoPor = table.Column<string>(type: "TEXT", nullable: true),
                    Excluido = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 180, nullable: false),
                    CriadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CriadoPor = table.Column<string>(type: "TEXT", nullable: false),
                    AtualizadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AtualizadoPor = table.Column<string>(type: "TEXT", nullable: true),
                    ExcluidoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExcluidoPor = table.Column<string>(type: "TEXT", nullable: true),
                    Excluido = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Prioridade = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    UsuarioResponsavelId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CriadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CriadoPor = table.Column<string>(type: "TEXT", nullable: false),
                    AtualizadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AtualizadoPor = table.Column<string>(type: "TEXT", nullable: true),
                    ExcluidoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExcluidoPor = table.Column<string>(type: "TEXT", nullable: true),
                    Excluido = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItems_Users_UsuarioResponsavelId",
                        column: x => x.UsuarioResponsavelId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Conteudo = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    TaskItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AutorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CriadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CriadoPor = table.Column<string>(type: "TEXT", nullable: false),
                    AtualizadoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AtualizadoPor = table.Column<string>(type: "TEXT", nullable: true),
                    ExcluidoEmUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExcluidoPor = table.Column<string>(type: "TEXT", nullable: true),
                    Excluido = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskComments_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskComments_Users_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Nome",
                table: "Produtos",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Sku",
                table: "Produtos",
                column: "Sku");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_AutorId",
                table: "TaskComments",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_TaskItemId",
                table: "TaskComments",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_Status_Prioridade",
                table: "TaskItems",
                columns: new[] { "Status", "Prioridade" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_Titulo",
                table: "TaskItems",
                column: "Titulo");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_UsuarioResponsavelId",
                table: "TaskItems",
                column: "UsuarioResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
