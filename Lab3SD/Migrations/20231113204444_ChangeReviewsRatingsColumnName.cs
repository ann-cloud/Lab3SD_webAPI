using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab3SD.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReviewsRatingsColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_posted",
                table: "Reviews_Ratings",
                newName: "date_when_posted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_when_posted",
                table: "Reviews_Ratings",
                newName: "date_posted");
        }
    }
}
