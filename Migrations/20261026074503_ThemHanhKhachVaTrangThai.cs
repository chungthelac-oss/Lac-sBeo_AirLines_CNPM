using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class ThemHanhKhachVaTrangThai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ĐÃ THÊM: trả lại ghế của các vé cũ, rồi xoá vé cũ (dạng cũ không có hành khách)
            migrationBuilder.Sql("UPDATE ChuyenBays SET SoGheTrong = SoGheTrong + (SELECT COUNT(*) FROM DatChos d WHERE d.ChuyenBayId = ChuyenBays.Id AND d.DaHuy = 0)");
            migrationBuilder.Sql("DELETE FROM DatChos");

            migrationBuilder.DropForeignKey(
                name: "FK_DatChos_ChuyenBays_ChuyenBayId",
                table: "DatChos");

            migrationBuilder.DropIndex(
                name: "IX_DatChos_MaDatCho",
                table: "DatChos");

            migrationBuilder.DropColumn(
                name: "DaHuy",
                table: "DatChos");

            migrationBuilder.DropColumn(
                name: "HoTen",
                table: "DatChos");

            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoai",
                table: "DatChos",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaDatCho",
                table: "DatChos",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "DatChos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "HanhKhachId",
                table: "DatChos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KhachHangId",
                table: "DatChos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "DatChos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HanhKhachs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoGiayTo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HanhKhachs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatChos_HanhKhachId",
                table: "DatChos",
                column: "HanhKhachId");

            migrationBuilder.CreateIndex(
                name: "IX_DatChos_KhachHangId",
                table: "DatChos",
                column: "KhachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_DatChos_MaDatCho",
                table: "DatChos",
                column: "MaDatCho",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DatChos_AspNetUsers_KhachHangId",
                table: "DatChos",
                column: "KhachHangId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatChos_ChuyenBays_ChuyenBayId",
                table: "DatChos",
                column: "ChuyenBayId",
                principalTable: "ChuyenBays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatChos_HanhKhachs_HanhKhachId",
                table: "DatChos",
                column: "HanhKhachId",
                principalTable: "HanhKhachs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatChos_AspNetUsers_KhachHangId",
                table: "DatChos");

            migrationBuilder.DropForeignKey(
                name: "FK_DatChos_ChuyenBays_ChuyenBayId",
                table: "DatChos");

            migrationBuilder.DropForeignKey(
                name: "FK_DatChos_HanhKhachs_HanhKhachId",
                table: "DatChos");

            migrationBuilder.DropTable(
                name: "HanhKhachs");

            migrationBuilder.DropIndex(
                name: "IX_DatChos_HanhKhachId",
                table: "DatChos");

            migrationBuilder.DropIndex(
                name: "IX_DatChos_KhachHangId",
                table: "DatChos");

            migrationBuilder.DropIndex(
                name: "IX_DatChos_MaDatCho",
                table: "DatChos");

            migrationBuilder.DropColumn(
                name: "HanhKhachId",
                table: "DatChos");

            migrationBuilder.DropColumn(
                name: "KhachHangId",
                table: "DatChos");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "DatChos");

            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoai",
                table: "DatChos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "MaDatCho",
                table: "DatChos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "DatChos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<bool>(
                name: "DaHuy",
                table: "DatChos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HoTen",
                table: "DatChos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DatChos_MaDatCho",
                table: "DatChos",
                column: "MaDatCho",
                unique: true,
                filter: "[MaDatCho] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DatChos_ChuyenBays_ChuyenBayId",
                table: "DatChos",
                column: "ChuyenBayId",
                principalTable: "ChuyenBays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}