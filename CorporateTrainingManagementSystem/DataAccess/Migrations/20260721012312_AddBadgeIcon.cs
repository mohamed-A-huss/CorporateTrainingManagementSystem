using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateTrainingManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBadgeIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Badges");
        }
    }
}
