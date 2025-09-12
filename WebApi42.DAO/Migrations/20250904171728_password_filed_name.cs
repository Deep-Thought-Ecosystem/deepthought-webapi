using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi42.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class password_filed_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHashed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHashed",
                table: "Users",
                newName: "Password");
        }
    }
}
