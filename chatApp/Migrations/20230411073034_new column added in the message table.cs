using Microsoft.EntityFrameworkCore.Migrations;

namespace chatApp.Migrations
{
    public partial class newcolumnaddedinthemessagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileLink",
                schema: "Identity",
                table: "UserMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLink",
                schema: "Identity",
                table: "UserMessages");
        }
    }
}
