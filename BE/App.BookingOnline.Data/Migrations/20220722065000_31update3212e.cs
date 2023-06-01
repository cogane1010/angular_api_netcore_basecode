using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class _31update3212e : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GF_BookingLine_GF_Booking_GF_Booking_Id",
                table: "GF_BookingLine");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 7, 22, 13, 49, 59, 259, DateTimeKind.Local).AddTicks(5359));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 7, 22, 13, 49, 59, 261, DateTimeKind.Local).AddTicks(8575));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "360f247d-8350-44c6-b913-f7388390c761");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "4458a92f-f738-44ca-a46d-119f8aebf337");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "ad838d39-7560-414e-a380-4d8759f5e80e");

            migrationBuilder.AddForeignKey(
                name: "FK_GF_BookingLine_GF_Booking_GF_Booking_Id",
                table: "GF_BookingLine",
                column: "GF_Booking_Id",
                principalTable: "GF_Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GF_BookingLine_GF_Booking_GF_Booking_Id",
                table: "GF_BookingLine");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 7, 6, 13, 5, 6, 84, DateTimeKind.Local).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 7, 6, 13, 5, 6, 87, DateTimeKind.Local).AddTicks(1181));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "00717fc4-4dd0-49d5-8907-81653aca5421");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "b38b0581-d7d4-47c4-a0aa-907f3e5d9d8b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "2a4ff461-db14-4a30-b448-f77eebfe392f");

            migrationBuilder.AddForeignKey(
                name: "FK_GF_BookingLine_GF_Booking_GF_Booking_Id",
                table: "GF_BookingLine",
                column: "GF_Booking_Id",
                principalTable: "GF_Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
