using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class seabank2022060101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TraceId",
                table: "SB_OutTransactionDetails",
                newName: "Trans_Id");

            migrationBuilder.RenameColumn(
                name: "TraceId",
                table: "SB_InTransactionDetails",
                newName: "Trans_Id");

            migrationBuilder.AddColumn<decimal>(
                name: "Charge_Amt",
                table: "SB_OutTransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Charge_Type",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RRN",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total_Amt",
                table: "SB_OutTransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Charge_Amt",
                table: "SB_InTransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Charge_Type",
                table: "SB_InTransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "SB_InTransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RRN",
                table: "SB_InTransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total_Amt",
                table: "SB_InTransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 6, 1, 11, 39, 57, 202, DateTimeKind.Local).AddTicks(8999));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 6, 1, 11, 39, 57, 205, DateTimeKind.Local).AddTicks(2654));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "5b7dd487-c547-40f9-96c6-813128d2efbe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "8f0251e4-6afc-4e50-b4d8-5201cbdfc940");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "1a3442d7-70ad-4fb2-82d0-b2942dbabc28");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Charge_Amt",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Charge_Type",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "RRN",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Total_Amt",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Charge_Amt",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Charge_Type",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "RRN",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Total_Amt",
                table: "SB_InTransactionDetails");

            migrationBuilder.RenameColumn(
                name: "Trans_Id",
                table: "SB_OutTransactionDetails",
                newName: "TraceId");

            migrationBuilder.RenameColumn(
                name: "Trans_Id",
                table: "SB_InTransactionDetails",
                newName: "TraceId");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 30, 15, 13, 8, 757, DateTimeKind.Local).AddTicks(2993));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 30, 15, 13, 8, 759, DateTimeKind.Local).AddTicks(5009));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "985b7016-dced-4bcb-a043-5f29ae5b807c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "c7fbf2bc-bdeb-4bb2-be5a-53cd9e0c6e05");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "9fc84245-7057-40c5-8ec8-c56526b734e4");
        }
    }
}
