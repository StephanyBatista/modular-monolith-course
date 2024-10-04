using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EGeek.Catalog.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeTracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeTracker",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChangedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NewStock = table.Column<int>(type: "integer", nullable: true),
                    NewPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    ProductId = table.Column<string>(type: "character varying(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeTracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeTracker_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog",
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeTracker_ProductId",
                schema: "catalog",
                table: "ChangeTracker",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeTracker",
                schema: "catalog");
        }
    }
}
