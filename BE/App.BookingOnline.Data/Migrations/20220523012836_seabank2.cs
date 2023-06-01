using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class seabank2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseCode",
                table: "SB_BookingTransactionPayment",
                newName: "OrgCode");

            migrationBuilder.AddColumn<string>(
                name: "ApproveCooments",
                table: "SB_OutTransactionHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "SB_OutTransactionHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ApproverUserId",
                table: "SB_OutTransactionHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SB_OutTransactionHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SB_InTransactionHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 23, 8, 28, 34, 956, DateTimeKind.Local).AddTicks(1277));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 23, 8, 28, 34, 958, DateTimeKind.Local).AddTicks(3991));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "602a7f1d-74b1-4139-ae10-34127f568fec");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "8f6b3d50-e2a5-42da-9974-65ba31216ab5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "a8c3b39a-6f2b-498e-bc24-e1b5e89a098e");

            migrationBuilder.CreateIndex(
                name: "IX_SB_BookingTransactionPayment_BookingId",
                table: "SB_BookingTransactionPayment",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_SB_BookingTransactionPayment_OrgId",
                table: "SB_BookingTransactionPayment",
                column: "OrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_SB_BookingTransactionPayment_C_Org_OrgId",
                table: "SB_BookingTransactionPayment",
                column: "OrgId",
                principalTable: "C_Org",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SB_BookingTransactionPayment_GF_Booking_BookingId",
                table: "SB_BookingTransactionPayment",
                column: "BookingId",
                principalTable: "GF_Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SB_BookingTransactionPayment_C_Org_OrgId",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_SB_BookingTransactionPayment_GF_Booking_BookingId",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropIndex(
                name: "IX_SB_BookingTransactionPayment_BookingId",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropIndex(
                name: "IX_SB_BookingTransactionPayment_OrgId",
                table: "SB_BookingTransactionPayment");

            migrationBuilder.DropColumn(
                name: "ApproveCooments",
                table: "SB_OutTransactionHeader");

            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "SB_OutTransactionHeader");

            migrationBuilder.DropColumn(
                name: "ApproverUserId",
                table: "SB_OutTransactionHeader");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SB_OutTransactionHeader");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SB_InTransactionHeader");

            migrationBuilder.RenameColumn(
                name: "OrgCode",
                table: "SB_BookingTransactionPayment",
                newName: "CourseCode");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 21, 11, 43, 4, 242, DateTimeKind.Local).AddTicks(3106));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 21, 11, 43, 4, 244, DateTimeKind.Local).AddTicks(3911));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "55d28236-72a1-4d1c-a350-5a85b96b6623");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "c642de2b-88a4-458f-b486-a3366d13c803");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "ddcfba6d-8efe-42a2-a82e-64822ebc9147");
        }
    }
}
