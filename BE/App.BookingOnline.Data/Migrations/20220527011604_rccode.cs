using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class rccode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRc_code",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 27, 8, 16, 3, 731, DateTimeKind.Local).AddTicks(3535));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 27, 8, 16, 3, 733, DateTimeKind.Local).AddTicks(2828));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "2d0302b9-35ce-47bd-a8fc-16d1319ea09b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "7ebbbac2-a773-4272-9746-778d4f43ad5c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "0927ebee-789f-4dc4-a0b0-004dbe0dc49d");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRc_code",
                table: "SB_OutTransactionDetails");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 26, 8, 43, 13, 471, DateTimeKind.Local).AddTicks(7556));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 26, 8, 43, 13, 473, DateTimeKind.Local).AddTicks(9296));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "fc44d9c6-4ecf-4420-bc85-c61239633bd7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "48dbb16e-b124-4b94-a33a-65e1017b397d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "3cd237ff-ae25-4fa9-b52a-6fc2406ba3b3");
        }
    }
}
