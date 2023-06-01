using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class seabank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SB_BookingTransactionPayment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Transid = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TokenSource = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    DebitAccount = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TotalMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatePayment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_BookingTransactionPayment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SB_InTransactionHeader",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DateTrans = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_InTransactionHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SB_OutTransactionHeader",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DateTrans = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_OutTransactionHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SB_InTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    InTransactionHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Service = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PartnerNo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CourseCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DebitAccount = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TotalMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Trans_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SBTransNo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Rc_code = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_InTransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SB_InTransactionDetails_SB_InTransactionHeader_InTransactionHeaderId",
                        column: x => x.InTransactionHeaderId,
                        principalTable: "SB_InTransactionHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SB_OutTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OutTransactionHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Service = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PartnerNo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CourseCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DebitAccount = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TotalMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Trans_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SBTransNo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Rc_code = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_OutTransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SB_OutTransactionDetails_SB_OutTransactionHeader_OutTransactionHeaderId",
                        column: x => x.OutTransactionHeaderId,
                        principalTable: "SB_OutTransactionHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 21, 10, 21, 21, 111, DateTimeKind.Local).AddTicks(8304));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 21, 10, 21, 21, 113, DateTimeKind.Local).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "34783874-3d7e-4d43-8e04-45c9e83ed5d3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "0a0543c1-8f7d-4d7a-8033-f94809f67fb5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "4ed8149a-48c8-4e2c-8450-c94ef6b64d02");

            migrationBuilder.CreateIndex(
                name: "IX_SB_InTransactionDetails_InTransactionHeaderId",
                table: "SB_InTransactionDetails",
                column: "InTransactionHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SB_OutTransactionDetails_OutTransactionHeaderId",
                table: "SB_OutTransactionDetails",
                column: "OutTransactionHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SB_BookingTransactionPayment");

            migrationBuilder.DropTable(
                name: "SB_InTransactionDetails");

            migrationBuilder.DropTable(
                name: "SB_OutTransactionDetails");

            migrationBuilder.DropTable(
                name: "SB_InTransactionHeader");

            migrationBuilder.DropTable(
                name: "SB_OutTransactionHeader");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 27, 15, 32, 43, 571, DateTimeKind.Local).AddTicks(3510));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 27, 15, 32, 43, 573, DateTimeKind.Local).AddTicks(5953));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "3fb895f0-37cd-4c75-9e2e-55ed98faded3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "2cf3a2da-be58-42cb-80be-13387c06a24b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "76d50213-80f4-42d3-bb3e-18cbec9cdd4c");
        }
    }
}
