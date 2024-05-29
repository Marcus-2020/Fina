using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fina.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToMovementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Movements",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Movements");
        }
    }
}
