using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi42.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_createat_field_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "CreateAt",
                table: "Users",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Users");
        }
    }
}
