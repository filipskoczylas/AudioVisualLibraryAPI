using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudiovisualLibraryAPI.Migrations
{
    /// <inheritdoc />
    public partial class Version1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "VideoFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "MusicAlbums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRecords",
                table: "MusicAlbums",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "VideoFiles");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "MusicAlbums");

            migrationBuilder.DropColumn(
                name: "NumberOfRecords",
                table: "MusicAlbums");
        }
    }
}
