using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class seabank6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "SB_OutTransactionHeader",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "SB_OutTransactionDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Rc_code",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrgId",
                table: "SB_InTransactionDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_SB_OutTransactionHeader_OrganizationId",
                table: "SB_OutTransactionHeader",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SB_OutTransactionDetails_OrganizationId",
                table: "SB_OutTransactionDetails",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SB_OutTransactionDetails_C_Org_OrganizationId",
                table: "SB_OutTransactionDetails",
                column: "OrganizationId",
                principalTable: "C_Org",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SB_OutTransactionHeader_C_Org_OrganizationId",
                table: "SB_OutTransactionHeader",
                column: "OrganizationId",
                principalTable: "C_Org",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SB_OutTransactionDetails_C_Org_OrganizationId",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SB_OutTransactionHeader_C_Org_OrganizationId",
                table: "SB_OutTransactionHeader");

            migrationBuilder.DropIndex(
                name: "IX_SB_OutTransactionHeader_OrganizationId",
                table: "SB_OutTransactionHeader");

            migrationBuilder.DropIndex(
                name: "IX_SB_OutTransactionDetails_OrganizationId",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "SB_OutTransactionHeader");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Rc_code",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "SB_InTransactionDetails");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 23, 11, 10, 17, 811, DateTimeKind.Local).AddTicks(1899));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 23, 11, 10, 17, 813, DateTimeKind.Local).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "e4572e6e-32ae-44a0-8992-6f9834a9ca4c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "e2cfd9ba-e565-4a13-a392-b93d63f8d68a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "a83f208d-b39c-4642-97d3-f93e68eb8f75");
        }
    }
}
