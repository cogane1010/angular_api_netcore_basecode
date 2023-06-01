using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class _090492022e : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoneyDate",
                table: "SB_OutTransactionDetails",
                newName: "MoneyChangeDate");

            migrationBuilder.AddColumn<string>(
                name: "InMoneyUser",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MoneyAtAcc",
                table: "SB_BookingTransactionPayment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MoneyChangeDate",
                table: "SB_BookingTransactionPayment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalMoneyAcc",
                table: "SB_BookingTransactionPayment",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 9, 10, 8, 51, 37, 274, DateTimeKind.Local).AddTicks(7639));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 9, 10, 8, 51, 37, 276, DateTimeKind.Local).AddTicks(9038));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "fd200f46-f3b6-4cd5-8667-8601fb8457a9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "901382b6-ebe9-4fc5-b407-9108fd91ba30");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "c6bfc90d-13e1-472b-b135-8015046d7821");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InMoneyUser",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "MoneyAtAcc",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "MoneyChangeDate",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "TotalMoneyAcc",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.RenameColumn(
                name: "MoneyChangeDate",
                table: "SB_OutTransactionDetails",
                newName: "MoneyDate");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 9, 9, 15, 3, 30, 173, DateTimeKind.Local).AddTicks(6914));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 9, 9, 15, 3, 30, 175, DateTimeKind.Local).AddTicks(7888));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "1657dfc9-b2d2-4b3c-bc04-20f436f84dfb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "bd62e22e-aaa9-41ff-a715-8d3cb99c28be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "5c2fcf19-410a-414c-b691-977389151bb8");
        }
    }
}
