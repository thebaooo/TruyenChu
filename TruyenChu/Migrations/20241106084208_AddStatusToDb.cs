using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruyenChu.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFull",
                table: "Stories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHot",
                table: "Stories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Stories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFull",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "IsHot",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Stories");
        }
    }
}
