using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APiMakePerfect.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherId",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "TeacherId", "Email", "Name", "Phone", "SubjectExpertise" },
                values: new object[] { 1, "default@teacher.com", "Default Teacher", "1234567890", "General" });
        }
    }
}
