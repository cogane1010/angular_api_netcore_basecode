using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class _081212022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SdkStatus",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.AddColumn<string>(
                name: "SdkRefundCode",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SdkRefundStatus",
                table: "SB_BookingTransactionPayment",
                type: "bit",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionNo",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 12, 8, 14, 24, 42, 589, DateTimeKind.Local).AddTicks(641));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 12, 8, 14, 24, 42, 591, DateTimeKind.Local).AddTicks(3006));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "2bee2c01-fdc5-4710-be27-02932c102257");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "2e46ff51-6f0b-40e0-b999-0aaa1948ce0f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "ee24001a-35f8-4608-b8f2-d254ebeb5780");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SdkRefundCode",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "SdkRefundStatus",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "TransactionNo",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.AddColumn<string>(
                name: "SdkStatus",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 12, 1, 10, 3, 47, 780, DateTimeKind.Local).AddTicks(5286));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 12, 1, 10, 3, 47, 782, DateTimeKind.Local).AddTicks(8140));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "ced6dbf5-9420-4e96-94d5-2e683756b407");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "2448c5e3-b181-43ec-b7a8-c73c24f3b2ed");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "ac741543-b905-4732-b09a-f688ee65df76");
        }
    }
}
