using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.BookingOnline.Data.Migrations
{
    public partial class initDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "App_Menu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TranslateKey = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Sequence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SequenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Sequence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Setting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_UploadFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<int>(type: "int", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_UploadFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Protected = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrolledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C_Hotline",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_Hotline", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "C_OrgType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_OrgType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MB_Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LowerFullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MobilePhone = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    IsMember = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AvatarFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MB_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_SequenceLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    App_Sequence_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateId = table.Column<DateTime>(type: "date", nullable: true),
                    MonthValue = table.Column<int>(type: "int", nullable: true),
                    YearValue = table.Column<int>(type: "int", nullable: true),
                    SeqValue = table.Column<int>(type: "int", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_SequenceLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_SequenceLine_App_Sequence_App_Sequence_Id",
                        column: x => x.App_Sequence_Id,
                        principalTable: "App_Sequence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "App_Role_Menu_Ref",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AspRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Role_Menu_Ref", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_Role_Menu_Ref_App_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "App_Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_App_Role_Menu_Ref_AspNetRoles_AspRoleId",
                        column: x => x.AspRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "C_Org",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    C_OrgType_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSummary = table.Column<bool>(type: "bit", nullable: false),
                    OrderValue = table.Column<int>(type: "int", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_Org", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_Org_C_OrgType_C_OrgType_Id",
                        column: x => x.C_OrgType_Id,
                        principalTable: "C_OrgType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "App_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_User_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "C_Booking_OtherType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_Booking_OtherType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_Booking_OtherType_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "C_CancelReason",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_CancelReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_CancelReason_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "C_Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OrderValue = table.Column<int>(type: "int", nullable: true),
                    PhoneSupport = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EmailSupport = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TimeSupport = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NoteSupport = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_Course_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "C_Holiday",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateId = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_Holiday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_Holiday_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "C_LockReason",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_LockReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_LockReason_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "C_OrgInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    InvoiceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InvoiceAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InvoiceBankAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceBankName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ShortAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_OrgInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_OrgInfo_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "C_PaymentType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_PaymentType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_PaymentType_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GF_CourseTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DOW = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_CourseTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_CourseTemplate_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GF_Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Message_Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Message_Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Message_Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Sent_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sent_User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Img_Url = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_Notification_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "M_Promotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PromotionCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Img_Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOW = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApplyTime_From = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ApplyTime_To = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Promotion_Type = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Priotity = table.Column<int>(type: "int", nullable: false),
                    IsHotPromotion = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Promotion_Formula = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Promotion_Value = table.Column<int>(type: "int", nullable: true),
                    PublicPrice_Percent = table.Column<int>(type: "int", nullable: true),
                    PromotionPrice_Percent = table.Column<int>(type: "int", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Promotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_M_Promotion_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MB_CustomerGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OrderValue = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MB_CustomerGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MB_CustomerGroup_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MB_MemberRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Request_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Request_Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Responsed_Date = table.Column<DateTime>(type: "date", nullable: true),
                    Responsed_User = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Responsed_Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MB_MemberRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MB_MemberRequest_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "C_ContactSupport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_C_ContactSupport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_C_ContactSupport_C_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "C_Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GF_TeeSheetLock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    C_LockReason_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    LockStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LockType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_TeeSheetLock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_TeeSheetLock_C_LockReason_C_LockReason_Id",
                        column: x => x.C_LockReason_Id,
                        principalTable: "C_LockReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_TeeSheetLock_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GF_CourseTemplateLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    C_Course_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GF_CourseTemplate_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTee = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Interval = table.Column<int>(type: "int", nullable: true),
                    TurnLength = table.Column<int>(type: "int", nullable: true),
                    Hole = table.Column<int>(type: "int", nullable: true),
                    FlightSeq = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxTurnLength = table.Column<int>(type: "int", nullable: true),
                    Part = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_CourseTemplateLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_CourseTemplateLine_C_Course_C_Course_Id",
                        column: x => x.C_Course_Id,
                        principalTable: "C_Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GF_CourseTemplateLine_GF_CourseTemplate_GF_CourseTemplate_Id",
                        column: x => x.GF_CourseTemplate_Id,
                        principalTable: "GF_CourseTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GF_BookingSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    C_Course_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    M_Promotion_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BookingCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Device_Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateId = table.Column<DateTime>(type: "date", nullable: true),
                    Start_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tee_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_BookingSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_BookingSession_C_Course_C_Course_Id",
                        column: x => x.C_Course_Id,
                        principalTable: "C_Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_BookingSession_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_BookingSession_M_Promotion_M_Promotion_Id",
                        column: x => x.M_Promotion_Id,
                        principalTable: "M_Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "M_Promotion_Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    M_Promotion_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    C_Course_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Promotion_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_M_Promotion_Course_C_Course_C_Course_Id",
                        column: x => x.C_Course_Id,
                        principalTable: "C_Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_M_Promotion_Course_M_Promotion_M_Promotion_Id",
                        column: x => x.M_Promotion_Id,
                        principalTable: "M_Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "M_Promotion_CustomerGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    M_Promotion_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MB_CustomerGroup_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Promotion_CustomerGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_M_Promotion_CustomerGroup_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_M_Promotion_CustomerGroup_M_Promotion_M_Promotion_Id",
                        column: x => x.M_Promotion_Id,
                        principalTable: "M_Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_M_Promotion_CustomerGroup_MB_CustomerGroup_MB_CustomerGroup_Id",
                        column: x => x.MB_CustomerGroup_Id,
                        principalTable: "MB_CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MB_CustomerGroup_Mapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MB_CustomerGroup_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Golf_Code = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MB_CustomerGroup_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MB_CustomerGroup_Mapping_MB_CustomerGroup_MB_CustomerGroup_Id",
                        column: x => x.MB_CustomerGroup_Id,
                        principalTable: "MB_CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MB_MemberCard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    MB_Customer_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Golf_MemberCardId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Golf_IDNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Golf_CardNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Golf_FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Golf_Mobilephone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Golf_Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Golf_DOB = table.Column<DateTime>(type: "date", nullable: true),
                    Golf_Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Golf_Effective_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Golf_Expire_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Golf_CardStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Golf_IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Golf_IsLock = table.Column<bool>(type: "bit", nullable: false),
                    MB_CustomerGroup_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MB_MemberCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MB_MemberCard_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MB_MemberCard_MB_Customer_MB_Customer_Id",
                        column: x => x.MB_Customer_Id,
                        principalTable: "MB_Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MB_MemberCard_MB_CustomerGroup_MB_CustomerGroup_Id",
                        column: x => x.MB_CustomerGroup_Id,
                        principalTable: "MB_CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GF_TeeSheetLockLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GF_TeeSheetLock_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    C_Course_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOW = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StartTee = table.Column<int>(type: "int", nullable: true),
                    FlightSeq = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    StartTimeValue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTimeValue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_TeeSheetLockLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_TeeSheetLockLine_C_Course_C_Course_Id",
                        column: x => x.C_Course_Id,
                        principalTable: "C_Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_TeeSheetLockLine_GF_TeeSheetLock_GF_TeeSheetLock_Id",
                        column: x => x.GF_TeeSheetLock_Id,
                        principalTable: "GF_TeeSheetLock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingSessionLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingSessionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BookingCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Device_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateId = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Start_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tee_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingSessionLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingSessionLine_GF_BookingSession_BookingSessionId",
                        column: x => x.BookingSessionId,
                        principalTable: "GF_BookingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GF_Booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    M_Promotion_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    C_Course_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GF_Booking_Session_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cancel_Reason_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BookingCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Device_Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateId = table.Column<DateTime>(type: "date", nullable: true),
                    Start_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tee_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Confirm_User = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Confirm_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Confirm_Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Confirm_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Payment_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cancel_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cancel_User = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cancel_Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Is_NoShow = table.Column<bool>(type: "bit", nullable: false),
                    UpdateNoShow_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateNoShow_UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BookingSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_Booking_C_CancelReason_Cancel_Reason_Id",
                        column: x => x.Cancel_Reason_Id,
                        principalTable: "C_CancelReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_Booking_C_Course_C_Course_Id",
                        column: x => x.C_Course_Id,
                        principalTable: "C_Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_Booking_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_Booking_GF_BookingSession_BookingSessionId",
                        column: x => x.BookingSessionId,
                        principalTable: "GF_BookingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_Booking_M_Promotion_M_Promotion_Id",
                        column: x => x.M_Promotion_Id,
                        principalTable: "M_Promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MB_MemberCard_Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    MC_MemberCard_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MB_CustomerGroup_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    C_Course_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MB_MemberCard_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MB_MemberCard_Course_C_Course_C_Course_Id",
                        column: x => x.C_Course_Id,
                        principalTable: "C_Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MB_MemberCard_Course_MB_CustomerGroup_MB_CustomerGroup_Id",
                        column: x => x.MB_CustomerGroup_Id,
                        principalTable: "MB_CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MB_MemberCard_Course_MB_MemberCard_MC_MemberCard_Id",
                        column: x => x.MC_MemberCard_Id,
                        principalTable: "MB_MemberCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GF_Booking_SpecialRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GF_Booking_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    C_Booking_OtherType_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_Booking_SpecialRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_Booking_SpecialRequest_C_Booking_OtherType_C_Booking_OtherType_Id",
                        column: x => x.C_Booking_OtherType_Id,
                        principalTable: "C_Booking_OtherType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_Booking_SpecialRequest_C_Org_C_Org_Id",
                        column: x => x.C_Org_Id,
                        principalTable: "C_Org",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_Booking_SpecialRequest_GF_Booking_GF_Booking_Id",
                        column: x => x.GF_Booking_Id,
                        principalTable: "GF_Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GF_BookingLine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    C_Org_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GF_Booking_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MB_CustomerGroup_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CardNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateId = table.Column<DateTime>(type: "date", nullable: true),
                    StartTee = table.Column<int>(type: "int", nullable: true),
                    Tee_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Hole = table.Column<int>(type: "int", nullable: true),
                    FlightSeq = table.Column<int>(type: "int", nullable: true),
                    Caddie_No = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlayerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MobilePhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BookingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Public_Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Promotion_Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Net_Ammount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Discount_Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Discount_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Total_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Deposit_Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Golf_Price_Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Golf_Promotion_Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Golf_Promotion_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Is_NoShow = table.Column<bool>(type: "bit", nullable: false),
                    UpdateNoShow_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateNoShow_UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Part = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TurnLength = table.Column<int>(type: "int", nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedUser = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GF_BookingLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GF_BookingLine_GF_Booking_GF_Booking_Id",
                        column: x => x.GF_Booking_Id,
                        principalTable: "GF_Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GF_BookingLine_MB_CustomerGroup_MB_CustomerGroup_Id",
                        column: x => x.MB_CustomerGroup_Id,
                        principalTable: "MB_CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "App_Menu",
                columns: new[] { "Id", "CreatedUser", "DisplayOrder", "Icon", "IsActive", "Level", "Name", "ParentId", "TranslateKey", "UpdatedDate", "UpdatedUser", "Url" },
                values: new object[] { new Guid("31f946a1-56cc-465d-a10d-20aae1d4f34e"), "admin", -1, null, true, -1, "ROOT", null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "App_Sequence",
                columns: new[] { "Id", "CreatedDate", "CreatedUser", "DocumentType", "IsActive", "MaxLength", "Prefix", "SequenceType", "UpdatedDate", "UpdatedUser" },
                values: new object[] { new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"), new DateTime(2021, 12, 21, 10, 46, 12, 243, DateTimeKind.Local).AddTicks(9686), "admin", "CUSTOMERCODE", true, 6, "GA", null, null, null });

            migrationBuilder.InsertData(
                table: "App_User",
                columns: new[] { "Id", "C_Org_Id", "CreatedUser", "Email", "FullName", "IsActive", "UpdatedDate", "UpdatedUser", "UserId", "UserName" },
                values: new object[] { new Guid("e45a2214-25aa-4f42-8d91-5aee48ab1c20"), null, null, "admin@brggroup.vn", "Admin", true, null, null, null, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedUser", "Description", "DisplayName", "IsActive", "Name", "NormalizedName", "Protected", "UpdatedDate", "UpdatedUser" },
                values: new object[,]
                {
                    { "672db3b8-c436-49bd-8172-bdb6ad6d6148", "2c081fa1-9666-460d-85f4-a4c61a4788a6", null, null, "Admin", true, "admin", "ADMIN", true, null, null },
                    { "db29c853-03ea-4328-9553-83676192aeed", "fd5b7ef0-a30e-4f07-9659-7e2170836105", null, null, "Nhân viên", true, "employee", "EMPLOYEE", true, null, null },
                    { "3e1ce2a6-e835-41ff-ab54-11dc1e60e839", "b76b94e1-20d0-469f-8b78-1bcd06a3e76d", null, null, "Khách hàng", true, "customer", "CUSTOMER", true, null, null }
                });

            migrationBuilder.InsertData(
                table: "App_SequenceLine",
                columns: new[] { "Id", "App_Sequence_Id", "CreatedDate", "CreatedUser", "DateId", "IsActive", "MonthValue", "SeqValue", "UpdatedDate", "UpdatedUser", "YearValue" },
                values: new object[] { new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"), new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"), new DateTime(2021, 12, 21, 10, 46, 12, 251, DateTimeKind.Local).AddTicks(5900), "admin", null, true, null, 0, null, null, 2021 });

            migrationBuilder.CreateIndex(
                name: "IX_App_Role_Menu_Ref_AspRoleId",
                table: "App_Role_Menu_Ref",
                column: "AspRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Role_Menu_Ref_MenuId",
                table: "App_Role_Menu_Ref",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_App_SequenceLine_App_Sequence_Id",
                table: "App_SequenceLine",
                column: "App_Sequence_Id");

            migrationBuilder.CreateIndex(
                name: "IX_App_User_C_Org_Id",
                table: "App_User",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSessionLine_BookingSessionId",
                table: "BookingSessionLine",
                column: "BookingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_C_Booking_OtherType_C_Org_Id",
                table: "C_Booking_OtherType",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_C_CancelReason_C_Org_Id",
                table: "C_CancelReason",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_C_ContactSupport_CourseId",
                table: "C_ContactSupport",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_C_Course_C_Org_Id",
                table: "C_Course",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_C_Holiday_C_Org_Id",
                table: "C_Holiday",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_C_LockReason_C_Org_Id",
                table: "C_LockReason",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_C_Org_C_OrgType_Id",
                table: "C_Org",
                column: "C_OrgType_Id");

            migrationBuilder.CreateIndex(
                name: "IX_C_OrgInfo_C_Org_Id",
                table: "C_OrgInfo",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_C_PaymentType_C_Org_Id",
                table: "C_PaymentType",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_BookingSessionId",
                table: "GF_Booking",
                column: "BookingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_C_Course_Id",
                table: "GF_Booking",
                column: "C_Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_C_Org_Id",
                table: "GF_Booking",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_Cancel_Reason_Id",
                table: "GF_Booking",
                column: "Cancel_Reason_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_M_Promotion_Id",
                table: "GF_Booking",
                column: "M_Promotion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_SpecialRequest_C_Booking_OtherType_Id",
                table: "GF_Booking_SpecialRequest",
                column: "C_Booking_OtherType_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_SpecialRequest_C_Org_Id",
                table: "GF_Booking_SpecialRequest",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Booking_SpecialRequest_GF_Booking_Id",
                table: "GF_Booking_SpecialRequest",
                column: "GF_Booking_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_BookingLine_GF_Booking_Id",
                table: "GF_BookingLine",
                column: "GF_Booking_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_BookingLine_MB_CustomerGroup_Id",
                table: "GF_BookingLine",
                column: "MB_CustomerGroup_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_BookingSession_C_Course_Id",
                table: "GF_BookingSession",
                column: "C_Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_BookingSession_C_Org_Id",
                table: "GF_BookingSession",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_BookingSession_M_Promotion_Id",
                table: "GF_BookingSession",
                column: "M_Promotion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_CourseTemplate_C_Org_Id",
                table: "GF_CourseTemplate",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_CourseTemplateLine_C_Course_Id",
                table: "GF_CourseTemplateLine",
                column: "C_Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_CourseTemplateLine_GF_CourseTemplate_Id",
                table: "GF_CourseTemplateLine",
                column: "GF_CourseTemplate_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_Notification_C_Org_Id",
                table: "GF_Notification",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_TeeSheetLock_C_LockReason_Id",
                table: "GF_TeeSheetLock",
                column: "C_LockReason_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_TeeSheetLock_C_Org_Id",
                table: "GF_TeeSheetLock",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_TeeSheetLockLine_C_Course_Id",
                table: "GF_TeeSheetLockLine",
                column: "C_Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GF_TeeSheetLockLine_GF_TeeSheetLock_Id",
                table: "GF_TeeSheetLockLine",
                column: "GF_TeeSheetLock_Id");

            migrationBuilder.CreateIndex(
                name: "IX_M_Promotion_C_Org_Id",
                table: "M_Promotion",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_M_Promotion_Course_C_Course_Id",
                table: "M_Promotion_Course",
                column: "C_Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_M_Promotion_Course_M_Promotion_Id",
                table: "M_Promotion_Course",
                column: "M_Promotion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_M_Promotion_CustomerGroup_C_Org_Id",
                table: "M_Promotion_CustomerGroup",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_M_Promotion_CustomerGroup_M_Promotion_Id",
                table: "M_Promotion_CustomerGroup",
                column: "M_Promotion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_M_Promotion_CustomerGroup_MB_CustomerGroup_Id",
                table: "M_Promotion_CustomerGroup",
                column: "MB_CustomerGroup_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_CustomerGroup_C_Org_Id",
                table: "MB_CustomerGroup",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_CustomerGroup_Mapping_MB_CustomerGroup_Id",
                table: "MB_CustomerGroup_Mapping",
                column: "MB_CustomerGroup_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_MemberCard_C_Org_Id",
                table: "MB_MemberCard",
                column: "C_Org_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_MemberCard_MB_Customer_Id",
                table: "MB_MemberCard",
                column: "MB_Customer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_MemberCard_MB_CustomerGroup_Id",
                table: "MB_MemberCard",
                column: "MB_CustomerGroup_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_MemberCard_Course_C_Course_Id",
                table: "MB_MemberCard_Course",
                column: "C_Course_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_MemberCard_Course_MB_CustomerGroup_Id",
                table: "MB_MemberCard_Course",
                column: "MB_CustomerGroup_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_MemberCard_Course_MC_MemberCard_Id",
                table: "MB_MemberCard_Course",
                column: "MC_MemberCard_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MB_MemberRequest_C_Org_Id",
                table: "MB_MemberRequest",
                column: "C_Org_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_Role_Menu_Ref");

            migrationBuilder.DropTable(
                name: "App_SequenceLine");

            migrationBuilder.DropTable(
                name: "App_Setting");

            migrationBuilder.DropTable(
                name: "App_UploadFile");

            migrationBuilder.DropTable(
                name: "App_User");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BookingSessionLine");

            migrationBuilder.DropTable(
                name: "C_ContactSupport");

            migrationBuilder.DropTable(
                name: "C_Holiday");

            migrationBuilder.DropTable(
                name: "C_Hotline");

            migrationBuilder.DropTable(
                name: "C_OrgInfo");

            migrationBuilder.DropTable(
                name: "C_PaymentType");

            migrationBuilder.DropTable(
                name: "GF_Booking_SpecialRequest");

            migrationBuilder.DropTable(
                name: "GF_BookingLine");

            migrationBuilder.DropTable(
                name: "GF_CourseTemplateLine");

            migrationBuilder.DropTable(
                name: "GF_Notification");

            migrationBuilder.DropTable(
                name: "GF_TeeSheetLockLine");

            migrationBuilder.DropTable(
                name: "M_Promotion_Course");

            migrationBuilder.DropTable(
                name: "M_Promotion_CustomerGroup");

            migrationBuilder.DropTable(
                name: "MB_CustomerGroup_Mapping");

            migrationBuilder.DropTable(
                name: "MB_MemberCard_Course");

            migrationBuilder.DropTable(
                name: "MB_MemberRequest");

            migrationBuilder.DropTable(
                name: "App_Menu");

            migrationBuilder.DropTable(
                name: "App_Sequence");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "C_Booking_OtherType");

            migrationBuilder.DropTable(
                name: "GF_Booking");

            migrationBuilder.DropTable(
                name: "GF_CourseTemplate");

            migrationBuilder.DropTable(
                name: "GF_TeeSheetLock");

            migrationBuilder.DropTable(
                name: "MB_MemberCard");

            migrationBuilder.DropTable(
                name: "C_CancelReason");

            migrationBuilder.DropTable(
                name: "GF_BookingSession");

            migrationBuilder.DropTable(
                name: "C_LockReason");

            migrationBuilder.DropTable(
                name: "MB_Customer");

            migrationBuilder.DropTable(
                name: "MB_CustomerGroup");

            migrationBuilder.DropTable(
                name: "C_Course");

            migrationBuilder.DropTable(
                name: "M_Promotion");

            migrationBuilder.DropTable(
                name: "C_Org");

            migrationBuilder.DropTable(
                name: "C_OrgType");
        }
    }
}
