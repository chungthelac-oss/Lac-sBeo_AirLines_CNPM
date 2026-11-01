using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class ThemHangVaMoDatVe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HangHangKhongId",
                table: "ChuyenBays",
                type: "int",
                nullable: false,
                defaultValue: 1);   // ĐÃ SỬA: chuyến cũ mặc định thuộc hãng số 1

            migrationBuilder.AddColumn<bool>(
                name: "MoDatVe",
                table: "ChuyenBays",
                type: "bit",
                nullable: false,
                defaultValue: true);   // ĐÃ SỬA: chuyến cũ mặc định đang mở bán

            migrationBuilder.CreateTable(
                name: "HangHangKhongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangHangKhongs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GioCatCanh", "GioHaCanh", "HangHangKhongId", "MoDatVe" },
                values: new object[] { new DateTime(2026, 12, 5, 6, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 5, 8, 25, 0, 0, DateTimeKind.Unspecified), 1, true });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GioCatCanh", "GioHaCanh", "HangHangKhongId", "MoDatVe" },
                values: new object[] { new DateTime(2026, 12, 5, 11, 40, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 5, 13, 50, 0, 0, DateTimeKind.Unspecified), 1, true });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "GioCatCanh", "GioHaCanh", "HangHangKhongId", "MoDatVe" },
                values: new object[] { new DateTime(2026, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 5, 10, 20, 0, 0, DateTimeKind.Unspecified), 1, true });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "GioCatCanh", "GioHaCanh", "HangHangKhongId", "MoDatVe" },
                values: new object[] { new DateTime(2026, 12, 6, 14, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 6, 16, 45, 0, 0, DateTimeKind.Unspecified), 1, true });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "GioCatCanh", "GioHaCanh", "HangHangKhongId", "MoDatVe" },
                values: new object[] { new DateTime(2026, 12, 7, 18, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 7, 20, 20, 0, 0, DateTimeKind.Unspecified), 1, true });

            migrationBuilder.InsertData(
                table: "HangHangKhongs",
                columns: new[] { "Id", "Ma", "Ten" },
                values: new object[,]
                {
                    { 1, "CD", "Cánh Diều Airlines" },
                    { 2, "SV", "Sao Việt Air" },
                    { 3, "MT", "Mây Trời Airways" }
                });

            migrationBuilder.InsertData(
                table: "ChuyenBays",
                columns: new[] { "Id", "DiemDen", "DiemDi", "GiaVe", "GioCatCanh", "GioHaCanh", "HangHangKhongId", "MoDatVe", "SoGheTrong", "SoHieu" },
                values: new object[,]
                {
                    { 6, "HAN", "SGN", 1190000m, new DateTime(2026, 12, 5, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 5, 10, 10, 0, 0, DateTimeKind.Unspecified), 2, true, 30, "SV 211" },
                    { 7, "CXR", "SGN", 790000m, new DateTime(2026, 12, 6, 7, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 6, 8, 35, 0, 0, DateTimeKind.Unspecified), 3, true, 25, "MT 515" },
                    { 8, "DAD", "HAN", 950000m, new DateTime(2026, 12, 6, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 6, 17, 20, 0, 0, DateTimeKind.Unspecified), 2, false, 18, "SV 330" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenBays_HangHangKhongId",
                table: "ChuyenBays",
                column: "HangHangKhongId");

            migrationBuilder.CreateIndex(
                name: "IX_HangHangKhongs_Ma",
                table: "HangHangKhongs",
                column: "Ma",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChuyenBays_HangHangKhongs_HangHangKhongId",
                table: "ChuyenBays",
                column: "HangHangKhongId",
                principalTable: "HangHangKhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChuyenBays_HangHangKhongs_HangHangKhongId",
                table: "ChuyenBays");

            migrationBuilder.DropTable(
                name: "HangHangKhongs");

            migrationBuilder.DropIndex(
                name: "IX_ChuyenBays_HangHangKhongId",
                table: "ChuyenBays");

            migrationBuilder.DeleteData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "HangHangKhongId",
                table: "ChuyenBays");

            migrationBuilder.DropColumn(
                name: "MoDatVe",
                table: "ChuyenBays");

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GioCatCanh", "GioHaCanh" },
                values: new object[] { new DateTime(2026, 10, 28, 6, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 28, 8, 25, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GioCatCanh", "GioHaCanh" },
                values: new object[] { new DateTime(2026, 10, 28, 11, 40, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 28, 13, 50, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "GioCatCanh", "GioHaCanh" },
                values: new object[] { new DateTime(2026, 10, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 28, 10, 20, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "GioCatCanh", "GioHaCanh" },
                values: new object[] { new DateTime(2026, 10, 29, 14, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 29, 16, 45, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChuyenBays",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "GioCatCanh", "GioHaCanh" },
                values: new object[] { new DateTime(2026, 10, 30, 18, 10, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 30, 20, 20, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}