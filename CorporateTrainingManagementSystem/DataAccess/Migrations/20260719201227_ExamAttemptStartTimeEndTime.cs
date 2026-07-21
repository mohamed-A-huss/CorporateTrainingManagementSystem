using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateTrainingManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ExamAttemptStartTimeEndTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttemptDate",
                table: "ExamAttempts",
                newName: "StartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "ExamAttempts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ExamAttempts");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ExamAttempts",
                newName: "AttemptDate");
        }
    }
}
