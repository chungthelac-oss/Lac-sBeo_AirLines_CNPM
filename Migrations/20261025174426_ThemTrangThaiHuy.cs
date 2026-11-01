using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatVeMayBay.Migrations
{
    /// <inheritdoc />
    public partial class ThemTrangThaiHuy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaHuy",
                table: "DatChos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaHuy",
                table: "DatChos");
        }
    }
}
