using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APiMakePerfect.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherToCours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
       name: "IX_Courses_TeacherId1",
       table: "Courses",
       column: "TeacherId1");



            migrationBuilder.AddColumn<int>(
            name: "TeacherId1",
            table: "Courses",
            nullable: true);


            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses",
                column: "TeacherId1",
                principalTable: "Teachers",
                principalColumn: "TeacherId1",
                onDelete: ReferentialAction.Cascade);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_TeacherId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeacherId1",
                table: "Courses");
        }

    }
}
