using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateTrainingManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserBadges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO UserBadges
(
    UserId,
    BadgeId,
    AwardedDate
)
VALUES

('1000802d-2a41-43a7-8e4d-7eef21edad53',4,GETDATE()),
('1000802d-2a41-43a7-8e4d-7eef21edad53',5,GETDATE()),
('1000802d-2a41-43a7-8e4d-7eef21edad53',6,GETDATE()),
('1000802d-2a41-43a7-8e4d-7eef21edad53',7,GETDATE()),
('1000802d-2a41-43a7-8e4d-7eef21edad53',8,GETDATE()),
('1000802d-2a41-43a7-8e4d-7eef21edad53',9,GETDATE());
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM UserBadges
WHERE UserId='1000802d-2a41-43a7-8e4d-7eef21edad53';
");
        }
    }
}
