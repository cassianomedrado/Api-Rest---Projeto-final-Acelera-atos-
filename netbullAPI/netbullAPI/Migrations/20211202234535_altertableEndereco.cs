using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace netbullAPI.Migrations
{
    public partial class altertableEndereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_Pessoas_endereco_idpessoa",
                table: "Enderecos");

            migrationBuilder.DropIndex(
                name: "IX_Enderecos_endereco_idpessoa",
                table: "Enderecos");

            migrationBuilder.AddColumn<int>(
                name: "pessoa_id",
                table: "Enderecos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_pessoa_id",
                table: "Enderecos",
                column: "pessoa_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_Pessoas_pessoa_id",
                table: "Enderecos",
                column: "pessoa_id",
                principalTable: "Pessoas",
                principalColumn: "pessoa_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_Pessoas_pessoa_id",
                table: "Enderecos");

            migrationBuilder.DropIndex(
                name: "IX_Enderecos_pessoa_id",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "pessoa_id",
                table: "Enderecos");

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
    }
}
