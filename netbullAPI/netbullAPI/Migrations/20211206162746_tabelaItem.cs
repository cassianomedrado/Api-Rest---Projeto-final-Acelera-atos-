using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netbullAPI.Migrations
{
    public partial class tabelaItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telefones_Pessoas_telefone_idPessoa",
                table: "Telefones");

            migrationBuilder.DropIndex(
                name: "IX_Telefones_telefone_idPessoa",
                table: "Telefones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Telefones_telefone_idPessoa",
                table: "Telefones",
                column: "telefone_idPessoa");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefones_Pessoas_telefone_idPessoa",
                table: "Telefones",
                column: "telefone_idPessoa",
                principalTable: "Pessoas",
                principalColumn: "pessoa_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
