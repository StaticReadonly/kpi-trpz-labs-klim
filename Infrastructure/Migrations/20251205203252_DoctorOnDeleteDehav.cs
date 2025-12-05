using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingClinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DoctorOnDeleteDehav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBase_Clinic_ClinicId",
                table: "UserBase");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBase_Speciality_SpecialityId",
                table: "UserBase");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBase_Clinic_ClinicId",
                table: "UserBase",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBase_Speciality_SpecialityId",
                table: "UserBase",
                column: "SpecialityId",
                principalTable: "Speciality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBase_Clinic_ClinicId",
                table: "UserBase");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBase_Speciality_SpecialityId",
                table: "UserBase");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBase_Clinic_ClinicId",
                table: "UserBase",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBase_Speciality_SpecialityId",
                table: "UserBase",
                column: "SpecialityId",
                principalTable: "Speciality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
