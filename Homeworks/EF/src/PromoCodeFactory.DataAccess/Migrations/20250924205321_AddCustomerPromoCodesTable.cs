using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerPromoCodesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Customers_CustomerId",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_CustomerId",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "PromoCodes");

            migrationBuilder.CreateTable(
                name: "CustomerPromoCodes",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PromoCodeId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPromoCodes", x => new { x.CustomerId, x.PromoCodeId });
                    table.ForeignKey(
                        name: "FK_CustomerPromoCodes_Customers_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerPromoCodes_PromoCodes_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "PromoCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPromoCodes_PromoCodeId",
                table: "CustomerPromoCodes",
                column: "PromoCodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPromoCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "PromoCodes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_CustomerId",
                table: "PromoCodes",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Customers_CustomerId",
                table: "PromoCodes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
