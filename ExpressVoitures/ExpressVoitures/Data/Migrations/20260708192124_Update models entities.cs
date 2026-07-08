using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updatemodelsentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Make",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Trim",
                table: "Cars");

            migrationBuilder.AlterColumn<float>(
                name: "RepairCost",
                table: "Repairs",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SaleDate",
                table: "CarSales",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PurchaseDate",
                table: "CarSales",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AvailableForSaleDate",
                table: "CarSales",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MakeId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrimId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarMakes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMakes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarTrims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTrims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarMakeCarModel",
                columns: table => new
                {
                    MakesId = table.Column<int>(type: "int", nullable: false),
                    ModelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMakeCarModel", x => new { x.MakesId, x.ModelsId });
                    table.ForeignKey(
                        name: "FK_CarMakeCarModel_CarMakes_MakesId",
                        column: x => x.MakesId,
                        principalTable: "CarMakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarMakeCarModel_CarModels_ModelsId",
                        column: x => x.ModelsId,
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarMakeCarTrim",
                columns: table => new
                {
                    MakesId = table.Column<int>(type: "int", nullable: false),
                    TrimsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMakeCarTrim", x => new { x.MakesId, x.TrimsId });
                    table.ForeignKey(
                        name: "FK_CarMakeCarTrim_CarMakes_MakesId",
                        column: x => x.MakesId,
                        principalTable: "CarMakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarMakeCarTrim_CarTrims_TrimsId",
                        column: x => x.TrimsId,
                        principalTable: "CarTrims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarSales_CarId",
                table: "CarSales",
                column: "CarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_MakeId",
                table: "Cars",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ModelId",
                table: "Cars",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_TrimId",
                table: "Cars",
                column: "TrimId");

            migrationBuilder.CreateIndex(
                name: "IX_CarMakeCarModel_ModelsId",
                table: "CarMakeCarModel",
                column: "ModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_CarMakeCarTrim_TrimsId",
                table: "CarMakeCarTrim",
                column: "TrimsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarMakes_MakeId",
                table: "Cars",
                column: "MakeId",
                principalTable: "CarMakes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars",
                column: "ModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarTrims_TrimId",
                table: "Cars",
                column: "TrimId",
                principalTable: "CarTrims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarSales_Cars_CarId",
                table: "CarSales",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarMakes_MakeId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarTrims_TrimId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_CarSales_Cars_CarId",
                table: "CarSales");

            migrationBuilder.DropTable(
                name: "CarMakeCarModel");

            migrationBuilder.DropTable(
                name: "CarMakeCarTrim");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.DropTable(
                name: "CarMakes");

            migrationBuilder.DropTable(
                name: "CarTrims");

            migrationBuilder.DropIndex(
                name: "IX_CarSales_CarId",
                table: "CarSales");

            migrationBuilder.DropIndex(
                name: "IX_Cars_MakeId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_ModelId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_TrimId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "MakeId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TrimId",
                table: "Cars");

            migrationBuilder.AlterColumn<float>(
                name: "RepairCost",
                table: "Repairs",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SaleDate",
                table: "CarSales",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseDate",
                table: "CarSales",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableForSaleDate",
                table: "CarSales",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Make",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Trim",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
