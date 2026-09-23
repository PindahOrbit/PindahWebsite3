using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PindahWebsite3.Migrations
{
    /// <inheritdoc />
    public partial class AddCmsFieldsToNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "News",
                type: "TEXT",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "News",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePublished",
                table: "News",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeaturedRank",
                table: "News",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "News",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "News",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_News_AuthorId",
                table: "News",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_News_IsFeatured_FeaturedRank",
                table: "News",
                columns: new[] { "IsFeatured", "FeaturedRank" });

            migrationBuilder.CreateIndex(
                name: "IX_News_Slug",
                table: "News",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_Status",
                table: "News",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AspNetUsers_AuthorId",
                table: "News",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AspNetUsers_AuthorId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_AuthorId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_IsFeatured_FeaturedRank",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_Slug",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_Status",
                table: "News");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "News");

            migrationBuilder.DropColumn(
                name: "DatePublished",
                table: "News");

            migrationBuilder.DropColumn(
                name: "FeaturedRank",
                table: "News");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "News");
        }
    }
}
