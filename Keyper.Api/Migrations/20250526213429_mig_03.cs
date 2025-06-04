using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keyper.Api.Migrations
{
    /// <inheritdoc />
    public partial class mig_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteInfos_AspNetUsers_AppUserId1",
                table: "SiteInfos");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId1",
                table: "SiteInfos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteInfos_AspNetUsers_AppUserId1",
                table: "SiteInfos",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteInfos_AspNetUsers_AppUserId1",
                table: "SiteInfos");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId1",
                table: "SiteInfos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SiteInfos_AspNetUsers_AppUserId1",
                table: "SiteInfos",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
