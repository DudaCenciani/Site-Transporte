using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendamentoTransporte.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposResponsavelEGaragem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Responsavel",
                table: "Agendamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "VaiAteGaragem",
                table: "Agendamentos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsavel",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "VaiAteGaragem",
                table: "Agendamentos");
        }
    }
}
