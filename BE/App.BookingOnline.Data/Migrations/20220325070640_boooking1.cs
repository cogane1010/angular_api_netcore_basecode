using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class boooking1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GF_Booking_GF_BookingSession_BookingSessionId",
                table: "GF_Booking");

            migrationBuilder.DropIndex(
                name: "IX_GF_Booking_BookingSessionId",
                table: "GF_Booking");

            migrationBuilder.DropColumn(
                name: "BookingSessionId",
                table: "GF_Booking");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 25, 14, 6, 39, 580, DateTimeKind.Local).AddTicks(5212));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 25, 14, 6, 39, 583, DateTimeKind.Local).AddTicks(9233));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "9cc017aa-e895-4437-a1fb-1984bd04d634");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "a217094b-a8be-489e-b31e-23f364ff1757");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "1391b381-fdaf-44f0-b47c-b73b2b064d4e");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_GF_Booking_Session_Id",
                table: "GF_Booking",
                column: "GF_Booking_Session_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GF_Booking_GF_BookingSession_GF_Booking_Session_Id",
                table: "GF_Booking",
                column: "GF_Booking_Session_Id",
                principalTable: "GF_BookingSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GF_Booking_GF_BookingSession_GF_Booking_Session_Id",
                table: "GF_Booking");

            migrationBuilder.DropIndex(
                name: "IX_GF_Booking_GF_Booking_Session_Id",
                table: "GF_Booking");

            migrationBuilder.AddColumn<Guid>(
                name: "BookingSessionId",
                table: "GF_Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 24, 16, 4, 14, 335, DateTimeKind.Local).AddTicks(5424));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 3, 24, 16, 4, 14, 338, DateTimeKind.Local).AddTicks(8242));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "7877b831-e9fb-4fdf-95b9-a46e7f4a4c18");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "af8af05c-3041-471a-bf16-3b0f022785b8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "8ecc289a-7177-448f-9946-88b206fc0628");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_BookingSessionId",
                table: "GF_Booking",
                column: "BookingSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GF_Booking_GF_BookingSession_BookingSessionId",
                table: "GF_Booking",
                column: "BookingSessionId",
                principalTable: "GF_BookingSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
