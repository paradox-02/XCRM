using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerFollowUps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerFollowUps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    PhoneSnapshot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUps_CustomerContacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "CustomerContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUps_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUps_ContactId",
                table: "CustomerFollowUps",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUps_CustomerId",
                table: "CustomerFollowUps",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerFollowUps");
        }
    }
}
