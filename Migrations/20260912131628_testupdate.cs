using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRecruitment.API.Migrations
{
    /// <inheritdoc />
    public partial class testupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_Jobs_JobId1",
                table: "JobSkills");

            migrationBuilder.DropIndex(
                name: "IX_JobSkills_JobId1",
                table: "JobSkills");

            migrationBuilder.DropColumn(
                name: "JobId1",
                table: "JobSkills");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "JobSkills",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SkillName",
                table: "JobSkills",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "JobSkills",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "SkillName",
                table: "JobSkills",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "JobId1",
                table: "JobSkills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_JobId1",
                table: "JobSkills",
                column: "JobId1");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkills_Jobs_JobId1",
                table: "JobSkills",
                column: "JobId1",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}
