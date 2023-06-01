using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class paymenttypeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentTypeId",
                table: "GF_Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 7, 10, 19, 35, 782, DateTimeKind.Local).AddTicks(1733));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 7, 10, 19, 35, 785, DateTimeKind.Local).AddTicks(2596));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "9932c892-df28-41e0-af8f-a391784c451c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "d4ff3556-8472-460c-b239-562176accf4e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "6996d11b-f2ca-489a-a133-54b74d4db063");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_PaymentTypeId",
                table: "GF_Booking",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GF_Booking_C_PaymentType_PaymentTypeId",
                table: "GF_Booking",
                column: "PaymentTypeId",
                principalTable: "C_PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GF_Booking_C_PaymentType_PaymentTypeId",
                table: "GF_Booking");

            migrationBuilder.DropIndex(
                name: "IX_GF_Booking_PaymentTypeId",
                table: "GF_Booking");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "GF_Booking");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 5, 9, 20, 41, 543, DateTimeKind.Local).AddTicks(2611));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 5, 9, 20, 41, 546, DateTimeKind.Local).AddTicks(4543));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "986e374a-5591-4a3b-b7d6-dc3f020f5c68");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "bef6e53d-3b31-4cd4-a60b-7cd204300be5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "cc956acd-c798-4d71-8572-c747fcf88431");
        }
    }
}
