using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APiMakePerfect.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrationName1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
    name: "TeacherId",
    table: "Courses",
    nullable: true, // Change to true
    defaultValue: null); // Ensure default is null

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
