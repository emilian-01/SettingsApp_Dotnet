using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SettingsAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DebugModeEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DetailedLoggingEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    BetaFeaturesEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    PerformanceMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    RemoteAccessEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvancedSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasicSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnableNotifications = table.Column<bool>(type: "INTEGER", nullable: false),
                    DarkModeEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoSaveEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    SoundEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedSettings_UserId",
                table: "AdvancedSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicSettings_UserId",
                table: "BasicSettings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancedSettings");

            migrationBuilder.DropTable(
                name: "BasicSettings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
