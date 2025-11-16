using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcusTel.PortalClientes.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjusteCampoNulo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastPartnerRawStatus",
                table: "TB_DidActivationRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastPartnerRawStatus",
                table: "TB_DidActivationRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
