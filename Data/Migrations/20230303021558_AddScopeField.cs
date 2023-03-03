using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpscaleTechnicalTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScopeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Scope",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scope",
                table: "Todos");
        }
    }
}
