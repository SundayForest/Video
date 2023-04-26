using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoInfrastructure.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "T_Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "T_Tag");

            migrationBuilder.AddColumn<string>(
                name: "Descrition",
                table: "T_TheFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_T_Users_UserName",
                table: "T_Users",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_T_Users_UserName",
                table: "T_Users");

            migrationBuilder.DropColumn(
                name: "Descrition",
                table: "T_TheFile");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "T_Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "T_Tag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
