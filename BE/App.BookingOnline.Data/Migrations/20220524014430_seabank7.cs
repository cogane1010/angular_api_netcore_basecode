using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class seabank7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrgId",
                table: "SB_InTransactionDetails",
                newName: "OrganizationId");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 24, 8, 44, 28, 945, DateTimeKind.Local).AddTicks(8635));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 24, 8, 44, 28, 948, DateTimeKind.Local).AddTicks(2424));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "cbda97ba-c037-4813-92dc-024da638b78e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "3393a383-cea4-44c3-b74e-548f6e76b56e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "71a43765-fadc-4561-a1b2-8c911d71036a");

            migrationBuilder.CreateIndex(
                name: "IX_SB_InTransactionDetails_OrganizationId",
                table: "SB_InTransactionDetails",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SB_InTransactionDetails_C_Org_OrganizationId",
                table: "SB_InTransactionDetails",
                column: "OrganizationId",
                principalTable: "C_Org",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SB_InTransactionDetails_C_Org_OrganizationId",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_SB_InTransactionDetails_OrganizationId",
                table: "SB_InTransactionDetails");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "SB_InTransactionDetails",
                newName: "OrgId");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 24, 8, 27, 6, 231, DateTimeKind.Local).AddTicks(6019));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 24, 8, 27, 6, 233, DateTimeKind.Local).AddTicks(9080));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "3d66d514-b7a1-45e4-a2c5-531de51610e3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "367d972a-9b3d-49f9-b6a9-7967f9e7ca71");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "43f3a7ff-1aa2-4b0a-907b-0f1550760f62");
        }
    }
}
