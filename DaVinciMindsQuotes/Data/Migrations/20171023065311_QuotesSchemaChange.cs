using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DaVinciMindsQuotes.Data.Migrations
{
    public partial class QuotesSchemaChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Author_AuthorId",
                table: "Quotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_QuoteSource_SourceId",
                table: "Quotes");

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "Quotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Quotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Author_AuthorId",
                table: "Quotes",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_QuoteSource_SourceId",
                table: "Quotes",
                column: "SourceId",
                principalTable: "QuoteSource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Author_AuthorId",
                table: "Quotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_QuoteSource_SourceId",
                table: "Quotes");

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "Quotes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Quotes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Author_AuthorId",
                table: "Quotes",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_QuoteSource_SourceId",
                table: "Quotes",
                column: "SourceId",
                principalTable: "QuoteSource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
