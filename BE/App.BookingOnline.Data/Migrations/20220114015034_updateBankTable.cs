using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class updateBankTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardType",
                table: "UserBankInfo");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "UserBankInfo",
                newName: "Start_Month");

            migrationBuilder.RenameColumn(
                name: "NameCard",
                table: "UserBankInfo",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "UserBankInfo",
                newName: "Expire_Year");

            migrationBuilder.AlterColumn<string>(
                name: "Cvv",
                table: "UserBankInfo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<Guid>(
                name: "C_PaymentType_Id",
                table: "UserBankInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Expire_Month",
                table: "UserBankInfo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Start_Year",
                table: "UserBankInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 14, 8, 50, 32, 941, DateTimeKind.Local).AddTicks(8574));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 14, 8, 50, 32, 946, DateTimeKind.Local).AddTicks(6631));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "70be6ac5-b86f-4c88-8991-b17378bea8db");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "be0bc547-3fe9-4f6d-8915-b5190a23a705");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "991ad71f-e38b-412b-91b8-7c182f1f2237");

            migrationBuilder.CreateIndex(
                name: "IX_UserBankInfo_C_PaymentType_Id",
                table: "UserBankInfo",
                column: "C_PaymentType_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBankInfo_C_PaymentType_C_PaymentType_Id",
                table: "UserBankInfo",
                column: "C_PaymentType_Id",
                principalTable: "C_PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBankInfo_C_PaymentType_C_PaymentType_Id",
                table: "UserBankInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserBankInfo_C_PaymentType_Id",
                table: "UserBankInfo");

            migrationBuilder.DropColumn(
                name: "C_PaymentType_Id",
                table: "UserBankInfo");

            migrationBuilder.DropColumn(
                name: "Expire_Month",
                table: "UserBankInfo");

            migrationBuilder.DropColumn(
                name: "Start_Year",
                table: "UserBankInfo");

            migrationBuilder.RenameColumn(
                name: "Start_Month",
                table: "UserBankInfo",
                newName: "PublishDate");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "UserBankInfo",
                newName: "NameCard");

            migrationBuilder.RenameColumn(
                name: "Expire_Year",
                table: "UserBankInfo",
                newName: "EndDate");

            migrationBuilder.AlterColumn<string>(
                name: "Cvv",
                table: "UserBankInfo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "UserBankInfo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 15, 47, 36, 745, DateTimeKind.Local).AddTicks(4338));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 15, 47, 36, 751, DateTimeKind.Local).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "b952ad97-df9c-47f2-8fdd-dfdd4ca2c73d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "59292a22-d378-43e2-ac30-44493fcea917");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "40120efd-31a7-4ff7-81de-5dc174b7adc2");
        }
    }
}
