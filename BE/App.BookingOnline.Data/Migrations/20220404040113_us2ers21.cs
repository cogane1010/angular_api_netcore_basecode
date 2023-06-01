using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class us2ers21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Languague",
                table: "AspNetUsers",
                newName: "Language");

            migrationBuilder.AddColumn<string>(
                name: "AvatarFileId",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Img_Url",
                table: "AspNetUsers",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMember",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 4, 11, 1, 12, 176, DateTimeKind.Local).AddTicks(333));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 4, 11, 1, 12, 179, DateTimeKind.Local).AddTicks(3640));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "6f7a1371-e2ea-438e-b642-585f3e1dea62");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "9146fa9e-a58c-44df-a03f-0623deed33fa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "0684f866-ec3e-4ba2-8407-6e5420aec98d");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarFileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Img_Url",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsMember",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "AspNetUsers",
                newName: "Languague");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 4, 10, 30, 41, 199, DateTimeKind.Local).AddTicks(1630));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 4, 4, 10, 30, 41, 202, DateTimeKind.Local).AddTicks(3252));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "fb144de2-68b3-427f-aead-020259641e2a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "84c2e6e6-b08f-4068-a3b7-25862249b46f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "ae044b82-b113-486e-a70c-7d1c7cb98b5e");
        }
    }
}
