using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Woody_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class addTeamMembersTableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "TeamMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "TeamMembers");
        }
    }
}
