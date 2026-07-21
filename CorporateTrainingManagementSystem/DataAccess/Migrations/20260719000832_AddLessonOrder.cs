using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateTrainingManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Lessons");
        }
    }
}
