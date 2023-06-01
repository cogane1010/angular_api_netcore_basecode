using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class seabank5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebitAccount",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "PartnerNo",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Rc_code",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "SBTransNo",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "DebitAccount",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "PartnerNo",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Rc_code",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "SBTransNo",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "SB_InTransactionDetails");

            migrationBuilder.RenameColumn(
                name: "Trans_time",
                table: "SB_OutTransactionDetails",
                newName: "Trans_Time");

            migrationBuilder.RenameColumn(
                name: "TotalMoney",
                table: "SB_OutTransactionDetails",
                newName: "Trans_Amt");

            migrationBuilder.RenameColumn(
                name: "Trans_time",
                table: "SB_InTransactionDetails",
                newName: "Trans_Time");

            migrationBuilder.RenameColumn(
                name: "TotalMoney",
                table: "SB_InTransactionDetails",
                newName: "Trans_Amt");

            migrationBuilder.AlterColumn<string>(
                name: "OrgCode",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Co_Code",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreditAcc",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DebitAcc",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FT_Id",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrgName",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PAN_NUMBER",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payment_Detail",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone_Number",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Return_Acc",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraceId",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Trans_Date",
                table: "SB_OutTransactionDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "OrgCode",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Co_Code",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreditAcc",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "SB_InTransactionDetails",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DebitAcc",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FT_Id",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrgName",
                table: "SB_InTransactionDetails",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PAN_NUMBER",
                table: "SB_InTransactionDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payment_Detail",
                table: "SB_InTransactionDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone_Number",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Return_Acc",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraceId",
                table: "SB_InTransactionDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Trans_Date",
                table: "SB_InTransactionDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 23, 11, 41, 10, 430, DateTimeKind.Local).AddTicks(1322));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 23, 11, 41, 10, 432, DateTimeKind.Local).AddTicks(4467));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "5fc1a1f0-c7ec-4b7a-aa35-33e427e36141");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "40374af1-7752-4b7c-8601-392f87036e3b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "62b57798-0150-49bb-b26b-edda1b103bf8");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Co_Code",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "CreditAcc",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "DebitAcc",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "FT_Id",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "OrgName",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "PAN_NUMBER",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Payment_Detail",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Phone_Number",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Return_Acc",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "TraceId",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Trans_Date",
                table: "SB_OutTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Co_Code",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "CreditAcc",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "DebitAcc",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "FT_Id",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "OrgName",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "PAN_NUMBER",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Payment_Detail",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Phone_Number",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Return_Acc",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "TraceId",
                table: "SB_InTransactionDetails");

            migrationBuilder.DropColumn(
                name: "Trans_Date",
                table: "SB_InTransactionDetails");

            migrationBuilder.RenameColumn(
                name: "Trans_Time",
                table: "SB_OutTransactionDetails",
                newName: "Trans_time");

            migrationBuilder.RenameColumn(
                name: "Trans_Amt",
                table: "SB_OutTransactionDetails",
                newName: "TotalMoney");

            migrationBuilder.RenameColumn(
                name: "Trans_Time",
                table: "SB_InTransactionDetails",
                newName: "Trans_time");

            migrationBuilder.RenameColumn(
                name: "Trans_Amt",
                table: "SB_InTransactionDetails",
                newName: "TotalMoney");

            migrationBuilder.AlterColumn<string>(
                name: "OrgCode",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DebitAccount",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrgId",
                table: "SB_OutTransactionDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerNo",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rc_code",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBTransNo",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "SB_OutTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "OrgCode",
                table: "SB_InTransactionDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DebitAccount",
                table: "SB_InTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrgId",
                table: "SB_InTransactionDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerNo",
                table: "SB_InTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rc_code",
                table: "SB_InTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBTransNo",
                table: "SB_InTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "SB_InTransactionDetails",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "App_Sequence",
                keyColumn: "Id",
                keyValue: new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 23, 9, 50, 30, 192, DateTimeKind.Local).AddTicks(1958));

            migrationBuilder.UpdateData(
                table: "App_SequenceLine",
                keyColumn: "Id",
                keyValue: new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                column: "CreatedDate",
                value: new DateTime(2022, 5, 23, 9, 50, 30, 194, DateTimeKind.Local).AddTicks(4212));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                column: "ConcurrencyStamp",
                value: "8757049d-702f-4742-b59f-2ec3e84caae1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                column: "ConcurrencyStamp",
                value: "1c696597-d7d1-47ec-a414-4f850a434da2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db29c853-03ea-4328-9553-83676192aeed",
                column: "ConcurrencyStamp",
                value: "fc13ac0f-8c44-4ff1-996b-29a7017b7851");
        }
    }
}
