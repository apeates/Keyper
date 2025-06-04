using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keyper.Api.Migrations
{
    /// <inheritdoc />
    public partial class mig_05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteUserName",
                table: "SiteInfos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteUserName",
                table: "SiteInfos");
        }
    }
}
