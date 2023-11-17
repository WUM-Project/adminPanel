using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin_panel.Migrations
{
    public partial class ProductToMark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadedFile_Categories_CategoryId",
                table: "UploadedFile");

            migrationBuilder.DropIndex(
                name: "IX_UploadedFile_CategoryId",
                table: "UploadedFile");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "UploadedFile");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "UploadedFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MarkId",
                table: "ProductToMark",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductToMark",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "UploadedFile");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "UploadedFile",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MarkId",
                table: "ProductToMark",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductToMark",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFile_CategoryId",
                table: "UploadedFile",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadedFile_Categories_CategoryId",
                table: "UploadedFile",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
