using Microsoft.EntityFrameworkCore.Migrations;

namespace chatApp.Migrations
{
    public partial class ADDNEWPROPERTYTOMESG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                schema: "Identity",
                table: "UserMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                schema: "Identity",
                table: "UserMessages");
        }
    }
}
