using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class ThemChangDung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangDungs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChuyenBayId = table.Column<int>(type: "int", nullable: false),
                    SanBayMa = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    GioDen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioDi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangDungs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangDungs_ChuyenBays_ChuyenBayId",
                        column: x => x.ChuyenBayId,
                        principalTable: "ChuyenBays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangDungs_SanBays_SanBayMa",
                        column: x => x.SanBayMa,
                        principalTable: "SanBays",
                        principalColumn: "Ma",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ChuyenBays",
                columns: new[] { "Id", "DiemDen", "DiemDi", "GiaVe", "GioCatCanh", "GioHaCanh", "HangHangKhongId", "MoDatVe", "SoGheTrong", "SoHieu" },
                values: new object[,]
                {
                    { 101, "HAN", "SGN", 990000m, new DateTime(2026, 12, 8, 6, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 8, 9, 30, 0, 0, DateTimeKind.Unspecified), 3, true, 40, "MT 701" },
                    { 102, "PQC", "HAN", 1450000m, new DateTime(2026, 12, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 9, 14, 10, 0, 0, DateTimeKind.Unspecified), 2, true, 22, "SV 455" }
                });

            migrationBuilder.InsertData(
                table: "ChangDungs",
                columns: new[] { "Id", "ChuyenBayId", "GioDen", "GioDi", "SanBayMa" },
                values: new object[,]
                {
                    { 1, 101, new DateTime(2026, 12, 8, 7, 20, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 8, 8, 0, 0, 0, DateTimeKind.Unspecified), "DAD" },
                    { 2, 102, new DateTime(2026, 12, 9, 11, 50, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 9, 12, 30, 0, 0, DateTimeKind.Unspecified), "DLI" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangDungs_ChuyenBayId",
                table: "ChangDungs",
                column: "ChuyenBayId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangDungs_SanBayMa",
                table: "ChangDungs",
                column: "SanBayMa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangDungs");

            migrationBuilder.DeleteData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 102);
        }
    }
}
