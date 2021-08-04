using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlcIO.Data.Migrations
{
    public partial class Mapping_Adjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdRequest",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdRequest",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Messages");
        }
    }
}
