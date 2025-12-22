using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditoryToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Products",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "Products",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "CreatedOnUtc");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "Products",
                newName: "LastModifiedAt");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "Products",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
