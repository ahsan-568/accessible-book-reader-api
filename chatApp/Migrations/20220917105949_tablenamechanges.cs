using Microsoft.EntityFrameworkCore.Migrations;

namespace chatApp.Migrations
{
    public partial class tablenamechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInteractions_AspNetUsers_MessageSenderId",
                schema: "Identity",
                table: "UserInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInteractions_AspNetUsers_RecieverId",
                schema: "Identity",
                table: "UserInteractions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInteractions",
                schema: "Identity",
                table: "UserInteractions");

            migrationBuilder.DropIndex(
                name: "IX_UserInteractions_MessageSenderId",
                schema: "Identity",
                table: "UserInteractions");

            migrationBuilder.DropColumn(
                name: "MessageSenderId",
                schema: "Identity",
                table: "UserInteractions");

            migrationBuilder.RenameTable(
                name: "UserInteractions",
                schema: "Identity",
                newName: "UserMessages",
                newSchema: "Identity");

            migrationBuilder.RenameIndex(
                name: "IX_UserInteractions_RecieverId",
                schema: "Identity",
                table: "UserMessages",
                newName: "IX_UserMessages_RecieverId");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                schema: "Identity",
                table: "UserMessages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMessages",
                schema: "Identity",
                table: "UserMessages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_SenderId",
                schema: "Identity",
                table: "UserMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_RecieverId",
                schema: "Identity",
                table: "UserMessages",
                column: "RecieverId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_SenderId",
                schema: "Identity",
                table: "UserMessages",
                column: "SenderId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_AspNetUsers_RecieverId",
                schema: "Identity",
                table: "UserMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_AspNetUsers_SenderId",
                schema: "Identity",
                table: "UserMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMessages",
                schema: "Identity",
                table: "UserMessages");

            migrationBuilder.DropIndex(
                name: "IX_UserMessages_SenderId",
                schema: "Identity",
                table: "UserMessages");

            migrationBuilder.RenameTable(
                name: "UserMessages",
                schema: "Identity",
                newName: "UserInteractions",
                newSchema: "Identity");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessages_RecieverId",
                schema: "Identity",
                table: "UserInteractions",
                newName: "IX_UserInteractions_RecieverId");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                schema: "Identity",
                table: "UserInteractions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageSenderId",
                schema: "Identity",
                table: "UserInteractions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInteractions",
                schema: "Identity",
                table: "UserInteractions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserInteractions_MessageSenderId",
                schema: "Identity",
                table: "UserInteractions",
                column: "MessageSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInteractions_AspNetUsers_MessageSenderId",
                schema: "Identity",
                table: "UserInteractions",
                column: "MessageSenderId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInteractions_AspNetUsers_RecieverId",
                schema: "Identity",
                table: "UserInteractions",
                column: "RecieverId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
