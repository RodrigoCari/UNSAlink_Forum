using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForoUniversitario.Migrations
{
    /// <inheritdoc />
    public partial class AddSharedPostSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SharedPostId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SharedPostId",
                table: "Posts",
                column: "SharedPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_SharedPostId",
                table: "Posts",
                column: "SharedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_SharedPostId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SharedPostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SharedPostId",
                table: "Posts");
        }
    }
}
