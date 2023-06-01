using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class notificationtab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img_url",
                table: "NotificationQueue",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 16, 10, 25, 52, 269, DateTimeKind.Local).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 16, 10, 25, 52, 277, DateTimeKind.Local).AddTicks(3575));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "98b8d8a8-6639-41a9-9819-32035ee53066");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "de275ebd-ad17-42b2-9b86-8664ec6d0ac7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "93fc72ae-ac45-43d7-92d2-63621c010666");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img_url",
                table: "NotificationQueue");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 14, 14, 48, 57, 162, DateTimeKind.Local).AddTicks(7220));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 14, 14, 48, 57, 176, DateTimeKind.Local).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "1e312f82-7234-4eb1-9e70-0c4d89791c6a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "e9e11948-b0e8-40b3-bf6c-3c5a5d7fb7f6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "7a3b770f-5968-4d59-8993-b45f038df72f");
        }
    }
}
