using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingClinic.data.migrations
{
    /// <inheritdoc />
    public partial class UserBaseEmailPhoneUnqiue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserBase",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_UserBase_Email",
                table: "UserBase",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBase_Phone",
                table: "UserBase",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserBase_Email",
                table: "UserBase");

            migrationBuilder.DropIndex(
                name: "IX_UserBase_Phone",
                table: "UserBase");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserBase",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
