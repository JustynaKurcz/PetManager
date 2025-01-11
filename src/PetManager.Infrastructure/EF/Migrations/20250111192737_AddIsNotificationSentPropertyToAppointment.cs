using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetManager.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddIsNotificationSentPropertyToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotificationSent",
                table: "Appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotificationSent",
                table: "Appointments");
        }
    }
}
