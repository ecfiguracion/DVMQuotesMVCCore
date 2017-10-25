using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DaVinciMindsQuotes.Data.Migrations
{
    public partial class CorrectedTableNameQuoteCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_QuoteCategoy_CategoryId",
                table: "Quotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteCategoy",
                table: "QuoteCategoy");

            migrationBuilder.RenameTable(
                name: "QuoteCategoy",
                newName: "QuoteCategory");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Author",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteCategory",
                table: "QuoteCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_QuoteCategory_CategoryId",
                table: "Quotes",
                column: "CategoryId",
                principalTable: "QuoteCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_QuoteCategory_CategoryId",
                table: "Quotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuoteCategory",
                table: "QuoteCategory");

            migrationBuilder.RenameTable(
                name: "QuoteCategory",
                newName: "QuoteCategoy");

            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Author",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuoteCategoy",
                table: "QuoteCategoy",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_QuoteCategoy_CategoryId",
                table: "Quotes",
                column: "CategoryId",
                principalTable: "QuoteCategoy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
