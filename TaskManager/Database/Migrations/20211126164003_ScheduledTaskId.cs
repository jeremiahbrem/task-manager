using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.Database.Migrations
{
    public partial class ScheduledTaskId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScheduledTaskId",
                table: "ScheduledTasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Name",
                table: "Tasks",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTasks_ScheduledTaskId",
                table: "ScheduledTasks",
                column: "ScheduledTaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_Name",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledTasks_ScheduledTaskId",
                table: "ScheduledTasks");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ScheduledTaskId",
                table: "ScheduledTasks");
        }
    }
}
