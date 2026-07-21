using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateTrainingManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRewardPointsToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RewardPoints",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RewardPoints",
                table: "Courses");
        }
    }
}
