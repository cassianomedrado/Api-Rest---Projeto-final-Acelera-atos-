using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netbullAPI.Migrations
{
    public partial class AlterTableforekeyEndereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_endereco_idpessoa",
                table: "Enderecos",
                column: "endereco_idpessoa");

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_Pessoas_endereco_idpessoa",
                table: "Enderecos",
                column: "endereco_idpessoa",
                principalTable: "Pessoas",
                principalColumn: "pessoa_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_Pessoas_endereco_idpessoa",
                table: "Enderecos");

            migrationBuilder.DropIndex(
                name: "IX_Enderecos_endereco_idpessoa",
                table: "Enderecos");
        }
    }
}
