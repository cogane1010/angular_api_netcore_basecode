using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class _23082022e : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenSource",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.AddColumn<string>(
                name: "AccountID",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardProduct",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardProductDesc",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbosingName",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkCardAcctId",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 8, 23, 15, 25, 55, 956, DateTimeKind.Local).AddTicks(6841));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 8, 23, 15, 25, 55, 958, DateTimeKind.Local).AddTicks(8269));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "6c7f742b-3351-4600-b1bb-888a026761c6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "6246ab9c-83ff-4d38-b933-182ade591452");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "99430d8d-d08b-4868-8b61-d892b233ca79");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "CardProduct",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "CardProductDesc",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "EmbosingName",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "LinkCardAcctId",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.AddColumn<string>(
                name: "TokenSource",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 8, 15, 14, 23, 36, 3, DateTimeKind.Local).AddTicks(6319));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 8, 15, 14, 23, 36, 5, DateTimeKind.Local).AddTicks(7550));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "8c5762a1-6cc5-4f1a-b032-fcc567aa137f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "1b104663-7a07-4c4d-bb79-fd2fc4847891");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "b6d47ecb-2790-46a7-bfb8-9a32ded465b0");
        }
    }
}
