using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateentitiesCarModelandCarTrimforsingleIdreference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarMakeCarModel");

            migrationBuilder.DropTable(
                name: "CarMakeCarTrim");

            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "CarTrims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MakeId",
                table: "CarModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarTrims_ModelId",
                table: "CarTrims",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_MakeId",
                table: "CarModels",
                column: "MakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarMakes_MakeId",
                table: "CarModels",
                column: "MakeId",
                principalTable: "CarMakes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarTrims_CarModels_ModelId",
                table: "CarTrims",
                column: "ModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarMakes_MakeId",
                table: "CarModels");

            migrationBuilder.DropForeignKey(
                name: "FK_CarTrims_CarModels_ModelId",
                table: "CarTrims");

            migrationBuilder.DropIndex(
                name: "IX_CarTrims_ModelId",
                table: "CarTrims");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_MakeId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "CarTrims");

            migrationBuilder.DropColumn(
                name: "MakeId",
                table: "CarModels");

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
                name: "IX_CarMakeCarModel_ModelsId",
                table: "CarMakeCarModel",
                column: "ModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_CarMakeCarTrim_TrimsId",
                table: "CarMakeCarTrim",
                column: "TrimsId");
        }
    }
}
