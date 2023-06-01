﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class boookingidcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IdOrder",
                table: "GF_Booking",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 18, 10, 19, 56, 985, DateTimeKind.Local).AddTicks(8625));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 18, 10, 19, 56, 992, DateTimeKind.Local).AddTicks(6379));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "3ab0fed0-2b7e-483a-ad71-a18d6df9240d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "926d6699-cc9a-4456-8cd0-8a19e76da04b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "8e686771-c421-4534-be7a-d86e5e364f9c");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdOrder",
                table: "GF_Booking");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 16, 15, 36, 49, 411, DateTimeKind.Local).AddTicks(8463));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 16, 15, 36, 49, 419, DateTimeKind.Local).AddTicks(371));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "c3257ae1-0684-4f75-9aae-d4cd4245a1e7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "7835191f-d3eb-4e81-98c2-bff91c800a8f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "ed5bd043-da11-4215-acf6-c81892260018");
        }
    }
}
