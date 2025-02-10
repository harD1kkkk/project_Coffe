using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Coffe.Migrations
{
    /// <inheritdoc />
    public partial class CorrectNameResponseFromMicroService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseFromMircoService",
                table: "UserPreferences",
                newName: "ResponseFromMicroService");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponseFromMicroService",
                table: "UserPreferences",
                newName: "ResponseFromMircoService");
        }
    }
}
