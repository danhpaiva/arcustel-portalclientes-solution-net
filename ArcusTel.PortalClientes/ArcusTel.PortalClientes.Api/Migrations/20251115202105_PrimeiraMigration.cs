using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcusTel.PortalClientes.Api.Migrations
{
    /// <inheritdoc />
    public partial class PrimeiraMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_DidActivationRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrefixCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CurrentStatus = table.Column<int>(type: "int", nullable: false),
                    LastPartnerRawStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_DidActivationRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_PartnerResponseLog",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RawResponsePayload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnerStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizerOutputStatus = table.Column<int>(type: "int", nullable: false),
                    PartnerDetailMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PartnerResponseLog", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "TB_PartnerStatusMapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    PartnerExternalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalNormalizedStatus = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PartnerStatusMapping", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_DidActivationRequest");

            migrationBuilder.DropTable(
                name: "TB_PartnerResponseLog");

            migrationBuilder.DropTable(
                name: "TB_PartnerStatusMapping");
        }
    }
}
