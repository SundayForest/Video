using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoInfrastructure.Migrations
{
    public partial class file_no_deleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "T_TheFile");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "T_TheFile",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
