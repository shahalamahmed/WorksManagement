using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorksManagement.Migrations
{
    /// <inheritdoc />
    public partial class fixWorkTaskDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTasks_Projects_ProjectId",
                table: "WorkTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkTasks_ProjectId",
                table: "WorkTasks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WorkTasks");

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "WorkTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "WorkTasks");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "WorkTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_ProjectId",
                table: "WorkTasks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTasks_Projects_ProjectId",
                table: "WorkTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
