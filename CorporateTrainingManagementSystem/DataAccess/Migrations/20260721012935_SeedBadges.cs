using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateTrainingManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedBadges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql(@"
INSERT INTO Badges
(
    Name,
    Description,
    RequiredPoints,
    Icon,
    Color
)
VALUES

(
'First Step',
'Awarded for successfully completing your first course.',
100,
'fa-seedling',
'text-success'
),

(
'Dedicated Learner',
'Earned after collecting 300 learning points.',
300,
'fa-book-open',
'text-primary'
),

(
'Skilled Trainee',
'Earned after collecting 500 learning points.',
500,
'fa-graduation-cap',
'text-info'
),

(
'Professional',
'Awarded for reaching 800 learning points.',
800,
'fa-medal',
'text-warning'
),

(
'Expert',
'Awarded for reaching 1200 learning points.',
1200,
'fa-trophy',
'text-danger'
),

(
'Training Master',
'The highest achievement awarded after collecting 2000 learning points.',
2000,
'fa-crown',
'text-purple'
);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Badges");
            migrationBuilder.Sql(@"
DELETE FROM Badges
WHERE Name IN
(
'First Step',
'Dedicated Learner',
'Skilled Trainee',
'Professional',
'Expert',
'Training Master'
);
");
        }
    }
}
