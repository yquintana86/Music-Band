using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MusicianSalaryToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicianPaymentDetailsRepository_Musicians_MusicianId",
                table: "MusicianPaymentDetailsRepository");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicianPaymentDetailsRepository_RangePlusRepository_RangePlusId",
                table: "MusicianPaymentDetailsRepository");

            migrationBuilder.DropTable(
                name: "RangePlusRepository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MusicianPaymentDetailsRepository",
                table: "MusicianPaymentDetailsRepository");

            migrationBuilder.RenameTable(
                name: "MusicianPaymentDetailsRepository",
                newName: "MusicianPaymentDetails");

            migrationBuilder.RenameIndex(
                name: "IX_MusicianPaymentDetailsRepository_RangePlusId",
                table: "MusicianPaymentDetails",
                newName: "IX_MusicianPaymentDetails_RangePlusId");

            migrationBuilder.RenameIndex(
                name: "IX_MusicianPaymentDetailsRepository_MusicianId",
                table: "MusicianPaymentDetails",
                newName: "IX_MusicianPaymentDetails_MusicianId");

            migrationBuilder.AlterColumn<decimal>(
                name: "BasicSalary",
                table: "Musicians",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MusicianPaymentDetails",
                table: "MusicianPaymentDetails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RangePlus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinExperience = table.Column<int>(type: "int", nullable: false),
                    MaxExperience = table.Column<int>(type: "int", nullable: false),
                    Plus = table.Column<decimal>(type: "decimal(4,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangePlus", x => x.Id);
                    table.CheckConstraint("CHK_MinMax_Experience", "([MinExperience] >= 0 AND [MinExperience] < [MaxExperience])");
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MusicianPaymentDetails_Musicians_MusicianId",
                table: "MusicianPaymentDetails",
                column: "MusicianId",
                principalTable: "Musicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicianPaymentDetails_RangePlus_RangePlusId",
                table: "MusicianPaymentDetails",
                column: "RangePlusId",
                principalTable: "RangePlus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicianPaymentDetails_Musicians_MusicianId",
                table: "MusicianPaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicianPaymentDetails_RangePlus_RangePlusId",
                table: "MusicianPaymentDetails");

            migrationBuilder.DropTable(
                name: "RangePlus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MusicianPaymentDetails",
                table: "MusicianPaymentDetails");

            migrationBuilder.RenameTable(
                name: "MusicianPaymentDetails",
                newName: "MusicianPaymentDetailsRepository");

            migrationBuilder.RenameIndex(
                name: "IX_MusicianPaymentDetails_RangePlusId",
                table: "MusicianPaymentDetailsRepository",
                newName: "IX_MusicianPaymentDetailsRepository_RangePlusId");

            migrationBuilder.RenameIndex(
                name: "IX_MusicianPaymentDetails_MusicianId",
                table: "MusicianPaymentDetailsRepository",
                newName: "IX_MusicianPaymentDetailsRepository_MusicianId");

            migrationBuilder.AlterColumn<decimal>(
                name: "BasicSalary",
                table: "Musicians",
                type: "decimal(7,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MusicianPaymentDetailsRepository",
                table: "MusicianPaymentDetailsRepository",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RangePlusRepository",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxExperience = table.Column<int>(type: "int", nullable: false),
                    MinExperience = table.Column<int>(type: "int", nullable: false),
                    Plus = table.Column<decimal>(type: "decimal(4,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangePlusRepository", x => x.Id);
                    table.CheckConstraint("CHK_MinMax_Experience", "([MinExperience] >= 0 AND [MinExperience] < [MaxExperience])");
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MusicianPaymentDetailsRepository_Musicians_MusicianId",
                table: "MusicianPaymentDetailsRepository",
                column: "MusicianId",
                principalTable: "Musicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicianPaymentDetailsRepository_RangePlusRepository_RangePlusId",
                table: "MusicianPaymentDetailsRepository",
                column: "RangePlusId",
                principalTable: "RangePlusRepository",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
