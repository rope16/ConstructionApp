using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConstructionApp.Migrations
{
    /// <inheritdoc />
    public partial class Adding_ProjectTask_Title : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProjectTasks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProjectTasks");
        }
    }
}
