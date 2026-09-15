using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveModelandMakefromCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarMakes_MakeId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "Cars",
                newName: "CarModelId");

            migrationBuilder.RenameColumn(
                name: "MakeId",
                table: "Cars",
                newName: "CarMakeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_ModelId",
                table: "Cars",
                newName: "IX_Cars_CarModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_MakeId",
                table: "Cars",
                newName: "IX_Cars_CarMakeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarMakes_CarMakeId",
                table: "Cars",
                column: "CarMakeId",
                principalTable: "CarMakes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModels_CarModelId",
                table: "Cars",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarMakes_CarMakeId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_CarModelId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "CarModelId",
                table: "Cars",
                newName: "ModelId");

            migrationBuilder.RenameColumn(
                name: "CarMakeId",
                table: "Cars",
                newName: "MakeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarModelId",
                table: "Cars",
                newName: "IX_Cars_ModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_CarMakeId",
                table: "Cars",
                newName: "IX_Cars_MakeId");

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
        }
    }
}
