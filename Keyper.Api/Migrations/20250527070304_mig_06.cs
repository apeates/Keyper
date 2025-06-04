using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keyper.Api.Migrations
{
    /// <inheritdoc />
    public partial class mig_06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteInfos_AspNetUsers_AppUserId1",
                table: "SiteInfos");

            migrationBuilder.DropIndex(
                name: "IX_SiteInfos_AppUserId1",
                table: "SiteInfos");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "SiteInfos");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "SiteInfos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.InsertData(
                table: "SiteInfos",
                columns: new[] { "Id", "AppUserId", "SiteName", "SitePassword", "SiteUserName" },
                values: new object[] { 1, "d05a8283-9ca6-4a5e-885d-0acc78c3e47c", "Keyper is a password management system.", "Keyper1234", "Keyper" });

            migrationBuilder.CreateIndex(
                name: "IX_SiteInfos_AppUserId",
                table: "SiteInfos",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteInfos_AspNetUsers_AppUserId",
                table: "SiteInfos",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteInfos_AspNetUsers_AppUserId",
                table: "SiteInfos");

            migrationBuilder.DropIndex(
                name: "IX_SiteInfos_AppUserId",
                table: "SiteInfos");

            migrationBuilder.DeleteData(
                table: "SiteInfos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "SiteInfos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "SiteInfos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteInfos_AppUserId1",
                table: "SiteInfos",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteInfos_AspNetUsers_AppUserId1",
                table: "SiteInfos",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
