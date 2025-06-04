using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keyper.Api.Migrations
{
    /// <inheritdoc />
    public partial class mig_04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SitePassword",
                table: "SiteInfos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SitePassword",
                table: "SiteInfos");
        }
    }
}
