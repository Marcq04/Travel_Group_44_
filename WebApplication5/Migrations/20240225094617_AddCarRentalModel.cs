using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication5.Migrations
{
    /// <inheritdoc />
    public partial class AddCarRentalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarRentalId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RentalDateMin = table.Column<DateTime>(type: "datetime2", nullable: false),
                RentalDateMax = table.Column<DateTime>(type: "datetime2", nullable: false),
                PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cars", x => x.CarRentalId);
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
