using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddPublisherXmlColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublisherDataXml",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublisherDataXml",
                table: "Publishers");
        }
    }
}
