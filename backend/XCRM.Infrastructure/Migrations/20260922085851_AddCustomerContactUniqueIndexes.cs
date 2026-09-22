using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerContactUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerContacts_CustomerId",
                table: "CustomerContacts");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContacts_CustomerId_Email",
                table: "CustomerContacts",
                columns: new[] { "CustomerId", "Email" },
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContacts_CustomerId_Phone",
                table: "CustomerContacts",
                columns: new[] { "CustomerId", "Phone" },
                unique: true,
                filter: "[Phone] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerContacts_CustomerId_Email",
                table: "CustomerContacts");

            migrationBuilder.DropIndex(
                name: "IX_CustomerContacts_CustomerId_Phone",
                table: "CustomerContacts");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContacts_CustomerId",
                table: "CustomerContacts",
                column: "CustomerId");
        }
    }
}
