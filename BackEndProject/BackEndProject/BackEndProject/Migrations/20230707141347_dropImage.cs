using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndProject.Migrations
{
    public partial class dropImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Categories_CategoryId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Sliders_SliderId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_CategoryId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_SliderId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "SliderId",
                table: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SliderId",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_CategoryId",
                table: "Image",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_SliderId",
                table: "Image",
                column: "SliderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Categories_CategoryId",
                table: "Image",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Sliders_SliderId",
                table: "Image",
                column: "SliderId",
                principalTable: "Sliders",
                principalColumn: "Id");
        }
    }
}
