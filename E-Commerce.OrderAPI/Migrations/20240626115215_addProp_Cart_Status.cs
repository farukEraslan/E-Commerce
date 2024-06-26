using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class addProp_Cart_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Carts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Carts");
        }
    }
}
