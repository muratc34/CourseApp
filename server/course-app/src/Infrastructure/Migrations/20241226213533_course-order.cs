using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class courseorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOrder_Courses_CoursesId",
                table: "CourseOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseOrder_Orders_OrdersId",
                table: "CourseOrder");

            migrationBuilder.RenameColumn(
                name: "OrdersId",
                table: "CourseOrder",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "CoursesId",
                table: "CourseOrder",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseOrder_OrdersId",
                table: "CourseOrder",
                newName: "IX_CourseOrder_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOrder_Courses_CourseId",
                table: "CourseOrder",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOrder_Orders_OrderId",
                table: "CourseOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOrder_Courses_CourseId",
                table: "CourseOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseOrder_Orders_OrderId",
                table: "CourseOrder");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "CourseOrder",
                newName: "OrdersId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CourseOrder",
                newName: "CoursesId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseOrder_OrderId",
                table: "CourseOrder",
                newName: "IX_CourseOrder_OrdersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOrder_Courses_CoursesId",
                table: "CourseOrder",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOrder_Orders_OrdersId",
                table: "CourseOrder",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
