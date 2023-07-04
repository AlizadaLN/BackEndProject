using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndProject.Migrations
{
    public partial class AddColumnsImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Products_ProductId",
                table: "Image");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Image",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Image_Products_ProductId",
                table: "Image",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Sliders_SliderId",
                table: "Image",
                column: "SliderId",
                principalTable: "Sliders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Categories_CategoryId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Products_ProductId",
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

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Products_ProductId",
                table: "Image",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
