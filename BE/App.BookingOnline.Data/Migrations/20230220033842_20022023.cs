using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class _20022023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoneyAtAcc",
                table: "SB_BookingTransactionPayment",
                newName: "OutMoneyDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "InMoneyAcc",
                table: "SB_BookingTransactionPayment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OutMoneyAcc",
                table: "SB_BookingTransactionPayment",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutMoneyUser",
                table: "SB_BookingTransactionPayment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2023, 2, 20, 10, 38, 40, 614, DateTimeKind.Local).AddTicks(5056));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2023, 2, 20, 10, 38, 40, 616, DateTimeKind.Local).AddTicks(6884));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "d19a6cb4-767e-4f77-a41d-9d4c4fd132aa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "5e454200-4abf-40a2-b4d9-f2ea7e498a51");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "1a2fd1d2-f02d-4118-8cf4-984b018a7e1c");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InMoneyAcc",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "OutMoneyAcc",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "OutMoneyUser",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.RenameColumn(
                name: "OutMoneyDate",
                table: "SB_BookingTransactionPayment",
                newName: "MoneyAtAcc");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2023, 1, 11, 14, 59, 24, 495, DateTimeKind.Local).AddTicks(1557));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2023, 1, 11, 14, 59, 24, 497, DateTimeKind.Local).AddTicks(2886));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "f268254d-118d-4f3a-954b-b72ffe646cce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "1bad6fd2-1e1f-4f0d-9eba-80f91dfa6f0c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "167b576b-fbde-4141-8132-5661175d0d59");
        }
    }
}
