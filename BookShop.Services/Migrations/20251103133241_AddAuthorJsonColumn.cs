using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorJsonColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorDataJson",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorDataJson",
                table: "Authors");
        }
    }
}
