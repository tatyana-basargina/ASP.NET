using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerPromoCodesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPromoCodes_Customers_PromoCodeId",
                table: "CustomerPromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPromoCodes_PromoCodes_CustomerId",
                table: "CustomerPromoCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPromoCodes_Customers_CustomerId",
                table: "CustomerPromoCodes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPromoCodes_PromoCodes_PromoCodeId",
                table: "CustomerPromoCodes",
                column: "PromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPromoCodes_Customers_CustomerId",
                table: "CustomerPromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPromoCodes_PromoCodes_PromoCodeId",
                table: "CustomerPromoCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPromoCodes_Customers_PromoCodeId",
                table: "CustomerPromoCodes",
                column: "PromoCodeId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPromoCodes_PromoCodes_CustomerId",
                table: "CustomerPromoCodes",
                column: "CustomerId",
                principalTable: "PromoCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
