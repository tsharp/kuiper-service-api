using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kuiper.Clustering.ServiceApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreObjects",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<long>(type: "INTEGER", nullable: false),
                    Value = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreObjects", x => x.Key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreObjects");
        }
    }
}
