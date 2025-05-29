using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelProperty.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Rooms_RoomId",
                table: "Residents");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Residents",
                newName: "RoomNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Residents_RoomId",
                table: "Residents",
                newName: "IX_Residents_RoomNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Rooms_RoomNumber",
                table: "Residents",
                column: "RoomNumber",
                principalTable: "Rooms",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Residents_Rooms_RoomNumber",
                table: "Residents");

            migrationBuilder.RenameColumn(
                name: "RoomNumber",
                table: "Residents",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Residents_RoomNumber",
                table: "Residents",
                newName: "IX_Residents_RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Residents_Rooms_RoomId",
                table: "Residents",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
