using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretSanta.Domain.Migrations
{
    public partial class PairingcontainsgroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Pairings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Pairings");
        }
    }
}
