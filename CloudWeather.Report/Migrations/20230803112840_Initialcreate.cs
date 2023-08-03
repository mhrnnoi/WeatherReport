using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudWeather.Report.Migrations
{
    /// <inheritdoc />
    public partial class Initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvarageHigh = table.Column<int>(type: "int", nullable: false),
                    AvarageLow = table.Column<int>(type: "int", nullable: false),
                    RainfallTotalInches = table.Column<int>(type: "int", nullable: false),
                    SnowTotalInches = table.Column<int>(type: "int", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherReports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherReports");
        }
    }
}
