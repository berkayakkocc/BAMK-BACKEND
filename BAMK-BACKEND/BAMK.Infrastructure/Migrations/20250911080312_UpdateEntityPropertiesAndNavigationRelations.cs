using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BAMK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityPropertiesAndNavigationRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_UserId1",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_TShirts_TShirtId1",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_UserId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_UserId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_ProductDetails_TShirtId",
                table: "ProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_TShirtId1",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Answers_UserId1",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TShirtId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Questions",
                newName: "QuestionTitle");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Questions",
                newName: "QuestionContent");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "OrderStatus");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Orders",
                newName: "OrderNotes");

            migrationBuilder.RenameColumn(
                name: "IsAccepted",
                table: "Answers",
                newName: "IsAcceptedAnswer");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Answers",
                newName: "AnswerContent");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_TShirtId",
                table: "ProductDetails",
                column: "TShirtId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductDetails_TShirtId",
                table: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "QuestionTitle",
                table: "Questions",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "QuestionContent",
                table: "Questions",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "OrderNotes",
                table: "Orders",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "IsAcceptedAnswer",
                table: "Answers",
                newName: "IsAccepted");

            migrationBuilder.RenameColumn(
                name: "AnswerContent",
                table: "Answers",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TShirtId1",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId1",
                table: "Questions",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_TShirtId",
                table: "ProductDetails",
                column: "TShirtId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId1",
                table: "Orders",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_TShirtId1",
                table: "OrderItems",
                column: "TShirtId1");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UserId1",
                table: "Answers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Users_UserId1",
                table: "Answers",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_TShirts_TShirtId1",
                table: "OrderItems",
                column: "TShirtId1",
                principalTable: "TShirts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId1",
                table: "Orders",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_UserId1",
                table: "Questions",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
