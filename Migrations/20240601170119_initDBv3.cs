using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS_DAMH.Migrations
{
    /// <inheritdoc />
    public partial class initDBv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KeyTp",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyTp",
                table: "Products");
        }
    }
}
