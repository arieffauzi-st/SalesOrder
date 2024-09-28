using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesOrder.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COM_CUSTOMER",
                columns: table => new
                {
                    COM_CUSTOMER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CUSTOMER_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COM_CUSTOMER", x => x.COM_CUSTOMER_ID);
                });

            migrationBuilder.CreateTable(
                name: "SO_ORDER",
                columns: table => new
                {
                    SO_ORDER_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORDER_NO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ORDER_DATE = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "'1900-01-01'"),
                    COM_CUSTOMER_ID = table.Column<int>(type: "int", nullable: false, defaultValue: -99),
                    ADDRESS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_ORDER", x => x.SO_ORDER_ID);
                    table.ForeignKey(
                        name: "FK_SO_ORDER_COM_CUSTOMER_COM_CUSTOMER_ID",
                        column: x => x.COM_CUSTOMER_ID,
                        principalTable: "COM_CUSTOMER",
                        principalColumn: "COM_CUSTOMER_ID");
                });

            migrationBuilder.CreateTable(
                name: "SO_ITEM",
                columns: table => new
                {
                    SO_ITEM_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SO_ORDER_ID = table.Column<long>(type: "bigint", nullable: false, defaultValue: -99L),
                    ITEM_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false, defaultValue: -99),
                    PRICE = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_ITEM", x => x.SO_ITEM_ID);
                    table.ForeignKey(
                        name: "FK_SO_ITEM_SO_ORDER_SO_ORDER_ID",
                        column: x => x.SO_ORDER_ID,
                        principalTable: "SO_ORDER",
                        principalColumn: "SO_ORDER_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SO_ITEM_SO_ORDER_ID",
                table: "SO_ITEM",
                column: "SO_ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SO_ORDER_COM_CUSTOMER_ID",
                table: "SO_ORDER",
                column: "COM_CUSTOMER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SO_ITEM");

            migrationBuilder.DropTable(
                name: "SO_ORDER");

            migrationBuilder.DropTable(
                name: "COM_CUSTOMER");
        }
    }
}
