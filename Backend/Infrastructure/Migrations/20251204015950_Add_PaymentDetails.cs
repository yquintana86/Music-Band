using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_PaymentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RangePlusRepository",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinExperience = table.Column<int>(type: "int", nullable: false),
                    MaxExperience = table.Column<int>(type: "int", nullable: false),
                    Plus = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangePlus", x => x.Id);
                    table.CheckConstraint("CHK_MinMax_Experience", "([MinExperience] >= 0 AND [MinExperience] < [MaxExperience])");
                });

            migrationBuilder.CreateTable(
                name: "MusicianPaymentDetailsRepository",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    MusicianId = table.Column<int>(type: "int", nullable: false),
                    RangePlusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicianPaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusicianPaymentDetails_Musicians_MusicianId",
                        column: x => x.MusicianId,
                        principalTable: "Musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicianPaymentDetails_RangePlus_RangePlusId",
                        column: x => x.RangePlusId,
                        principalTable: "RangePlusRepository",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusicianPaymentDetails_MusicianId",
                table: "MusicianPaymentDetailsRepository",
                column: "MusicianId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicianPaymentDetails_RangePlusId",
                table: "MusicianPaymentDetailsRepository",
                column: "RangePlusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicianPaymentDetailsRepository");

            migrationBuilder.DropTable(
                name: "RangePlusRepository");
        }
    }
}
