using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bidesk.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Bideskmig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Bidesk");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false),
                    TableName = table.Column<string>(type: "text", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: false),
                    TableID = table.Column<long>(type: "bigint", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AuditLogs_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditLogs_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditLogs_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditLogs_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BideskParameters",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParamName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ParamValue = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BideskParameters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BideskParameters_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BideskParameters_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BideskParameters_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BideskParameters_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketCategories",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketCategories_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketCategories_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketCategories_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketCategories_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketGroups",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketGroups_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketGroups_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketGroups_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketGroups_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketPriorities",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    ResolveHours = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPriorities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketPriorities_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketPriorities_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketPriorities_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketPriorities_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false),
                    PausesSla = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatuses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketStatuses_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketStatuses_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketStatuses_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketStatuses_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketSubCategories",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryID = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSubCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketSubCategories_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketSubCategories_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketSubCategories_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketSubCategories_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketSubCategories_TicketCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "Bidesk",
                        principalTable: "TicketCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnicianGroups",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupID = table.Column<long>(type: "bigint", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TechnicianGroups_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TechnicianGroups_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicianGroups_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TechnicianGroups_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicianGroups_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicianGroups_TicketGroups_GroupID",
                        column: x => x.GroupID,
                        principalSchema: "Bidesk",
                        principalTable: "TicketGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RequesterUserID = table.Column<long>(type: "bigint", nullable: false),
                    StatusID = table.Column<long>(type: "bigint", nullable: false),
                    PriorityID = table.Column<long>(type: "bigint", nullable: false),
                    GroupID = table.Column<long>(type: "bigint", nullable: false),
                    TechnicianUserID = table.Column<long>(type: "bigint", nullable: true),
                    CategoryID = table.Column<long>(type: "bigint", nullable: true),
                    SubCategoryID = table.Column<long>(type: "bigint", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SlaPausedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OverdueNotified = table.Column<bool>(type: "boolean", nullable: false),
                    FirstResponseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ResolvedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Resolution = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_RequesterUserID",
                        column: x => x.RequesterUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_TechnicianUserID",
                        column: x => x.TechnicianUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "Bidesk",
                        principalTable: "TicketCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketGroups_GroupID",
                        column: x => x.GroupID,
                        principalSchema: "Bidesk",
                        principalTable: "TicketGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketPriorities_PriorityID",
                        column: x => x.PriorityID,
                        principalSchema: "Bidesk",
                        principalTable: "TicketPriorities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketStatuses_StatusID",
                        column: x => x.StatusID,
                        principalSchema: "Bidesk",
                        principalTable: "TicketStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketSubCategories_SubCategoryID",
                        column: x => x.SubCategoryID,
                        principalSchema: "Bidesk",
                        principalTable: "TicketSubCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketAttachments",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketID = table.Column<long>(type: "bigint", nullable: false),
                    ConversationID = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Keyword = table.Column<Guid>(type: "uuid", nullable: true),
                    AttachmentPath = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AttachmentID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAttachments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketAttachments_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketAttachments_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketAttachments_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketAttachments_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketAttachments_Tickets_TicketID",
                        column: x => x.TicketID,
                        principalSchema: "Bidesk",
                        principalTable: "Tickets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketConversations",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketID = table.Column<long>(type: "bigint", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    IsInternal = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketConversations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketConversations_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketConversations_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketConversations_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketConversations_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketConversations_Tickets_TicketID",
                        column: x => x.TicketID,
                        principalSchema: "Bidesk",
                        principalTable: "Tickets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketNotifyUsers",
                schema: "Bidesk",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketID = table.Column<long>(type: "bigint", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    CanceledDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CanceledUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketNotifyUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TicketNotifyUsers_AspNetUsers_CanceledUserId",
                        column: x => x.CanceledUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketNotifyUsers_AspNetUsers_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketNotifyUsers_AspNetUsers_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketNotifyUsers_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketNotifyUsers_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalSchema: "public",
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketNotifyUsers_Tickets_TicketID",
                        column: x => x.TicketID,
                        principalSchema: "Bidesk",
                        principalTable: "Tickets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CanceledUserId",
                schema: "Bidesk",
                table: "AuditLogs",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CompanyID",
                schema: "Bidesk",
                table: "AuditLogs",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedUserId",
                schema: "Bidesk",
                table: "AuditLogs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UpdatedUserId",
                schema: "Bidesk",
                table: "AuditLogs",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BideskParameters_CanceledUserId",
                schema: "Bidesk",
                table: "BideskParameters",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BideskParameters_CompanyID",
                schema: "Bidesk",
                table: "BideskParameters",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_BideskParameters_CreatedUserId",
                schema: "Bidesk",
                table: "BideskParameters",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BideskParameters_UpdatedUserId",
                schema: "Bidesk",
                table: "BideskParameters",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianGroup_Group_User",
                schema: "Bidesk",
                table: "TechnicianGroups",
                columns: new[] { "CompanyID", "GroupID", "UserID" },
                unique: true,
                filter: "\"IsCanceled\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianGroups_CanceledUserId",
                schema: "Bidesk",
                table: "TechnicianGroups",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianGroups_CreatedUserId",
                schema: "Bidesk",
                table: "TechnicianGroups",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianGroups_GroupID",
                schema: "Bidesk",
                table: "TechnicianGroups",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianGroups_UpdatedUserId",
                schema: "Bidesk",
                table: "TechnicianGroups",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianGroups_UserID",
                schema: "Bidesk",
                table: "TechnicianGroups",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_CanceledUserId",
                schema: "Bidesk",
                table: "TicketAttachments",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_CompanyID_IsCanceled_TicketID",
                schema: "Bidesk",
                table: "TicketAttachments",
                columns: new[] { "CompanyID", "IsCanceled", "TicketID" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_CreatedUserId",
                schema: "Bidesk",
                table: "TicketAttachments",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_TicketID",
                schema: "Bidesk",
                table: "TicketAttachments",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_UpdatedUserId",
                schema: "Bidesk",
                table: "TicketAttachments",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategories_CanceledUserId",
                schema: "Bidesk",
                table: "TicketCategories",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategories_CompanyID",
                schema: "Bidesk",
                table: "TicketCategories",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategories_CreatedUserId",
                schema: "Bidesk",
                table: "TicketCategories",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategories_UpdatedUserId",
                schema: "Bidesk",
                table: "TicketCategories",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConversations_CanceledUserId",
                schema: "Bidesk",
                table: "TicketConversations",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConversations_CompanyID_IsCanceled_TicketID",
                schema: "Bidesk",
                table: "TicketConversations",
                columns: new[] { "CompanyID", "IsCanceled", "TicketID" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketConversations_CreatedUserId",
                schema: "Bidesk",
                table: "TicketConversations",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConversations_TicketID",
                schema: "Bidesk",
                table: "TicketConversations",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketConversations_UpdatedUserId",
                schema: "Bidesk",
                table: "TicketConversations",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketGroups_CanceledUserId",
                schema: "Bidesk",
                table: "TicketGroups",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketGroups_CompanyID",
                schema: "Bidesk",
                table: "TicketGroups",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketGroups_CreatedUserId",
                schema: "Bidesk",
                table: "TicketGroups",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketGroups_UpdatedUserId",
                schema: "Bidesk",
                table: "TicketGroups",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketNotifyUsers_CanceledUserId",
                schema: "Bidesk",
                table: "TicketNotifyUsers",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketNotifyUsers_CompanyID",
                schema: "Bidesk",
                table: "TicketNotifyUsers",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketNotifyUsers_CreatedUserId",
                schema: "Bidesk",
                table: "TicketNotifyUsers",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketNotifyUsers_TicketID",
                schema: "Bidesk",
                table: "TicketNotifyUsers",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketNotifyUsers_UpdatedUserId",
                schema: "Bidesk",
                table: "TicketNotifyUsers",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketNotifyUsers_UserID",
                schema: "Bidesk",
                table: "TicketNotifyUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPriorities_CanceledUserId",
                schema: "Bidesk",
                table: "TicketPriorities",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPriorities_CompanyID",
                schema: "Bidesk",
                table: "TicketPriorities",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPriorities_CreatedUserId",
                schema: "Bidesk",
                table: "TicketPriorities",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPriorities_UpdatedUserId",
                schema: "Bidesk",
                table: "TicketPriorities",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CanceledUserId",
                schema: "Bidesk",
                table: "Tickets",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CategoryID",
                schema: "Bidesk",
                table: "Tickets",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CompanyID_IsCanceled_GroupID",
                schema: "Bidesk",
                table: "Tickets",
                columns: new[] { "CompanyID", "IsCanceled", "GroupID" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CompanyID_IsCanceled_RequesterUserID",
                schema: "Bidesk",
                table: "Tickets",
                columns: new[] { "CompanyID", "IsCanceled", "RequesterUserID" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CompanyID_IsCanceled_StatusID",
                schema: "Bidesk",
                table: "Tickets",
                columns: new[] { "CompanyID", "IsCanceled", "StatusID" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CompanyID_IsCanceled_TechnicianUserID",
                schema: "Bidesk",
                table: "Tickets",
                columns: new[] { "CompanyID", "IsCanceled", "TechnicianUserID" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatedUserId",
                schema: "Bidesk",
                table: "Tickets",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_GroupID",
                schema: "Bidesk",
                table: "Tickets",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PriorityID",
                schema: "Bidesk",
                table: "Tickets",
                column: "PriorityID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RequesterUserID",
                schema: "Bidesk",
                table: "Tickets",
                column: "RequesterUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StatusID",
                schema: "Bidesk",
                table: "Tickets",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SubCategoryID",
                schema: "Bidesk",
                table: "Tickets",
                column: "SubCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TechnicianUserID",
                schema: "Bidesk",
                table: "Tickets",
                column: "TechnicianUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UpdatedUserId",
                schema: "Bidesk",
                table: "Tickets",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatuses_CanceledUserId",
                schema: "Bidesk",
                table: "TicketStatuses",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatuses_CompanyID",
                schema: "Bidesk",
                table: "TicketStatuses",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatuses_CreatedUserId",
                schema: "Bidesk",
                table: "TicketStatuses",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatuses_UpdatedUserId",
                schema: "Bidesk",
                table: "TicketStatuses",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSubCategories_CanceledUserId",
                schema: "Bidesk",
                table: "TicketSubCategories",
                column: "CanceledUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSubCategories_CategoryID",
                schema: "Bidesk",
                table: "TicketSubCategories",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSubCategories_CompanyID",
                schema: "Bidesk",
                table: "TicketSubCategories",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSubCategories_CreatedUserId",
                schema: "Bidesk",
                table: "TicketSubCategories",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSubCategories_UpdatedUserId",
                schema: "Bidesk",
                table: "TicketSubCategories",
                column: "UpdatedUserId");

            // Baslangic tanimlari: her sirket icin, olusturan olarak ilk admin kullanici.
            // CreatedUserId / CompanyID public semasina FK oldugu icin HasData yerine SQL.
            const string seedUser = "(SELECT MIN(u.\"Id\") FROM public.\"AspNetUsers\" u WHERE u.\"IsAdmin\")";

            migrationBuilder.Sql($@"
INSERT INTO ""Bidesk"".""TicketStatuses""
    (""Name"", ""Severity"", ""SortOrder"", ""IsDefault"", ""IsResolved"", ""IsClosed"", ""PausesSla"", ""IsActive"", ""CreatedDate"", ""CreatedUserId"", ""IsCanceled"", ""CompanyID"")
SELECT v.name, v.sev, v.ord, v.def, v.res, v.cls, v.pause, true, now(), {seedUser}, false, c.""ID""
FROM public.""Companies"" c
CROSS JOIN (VALUES
    ('Talep',             'info',      1, true,  false, false, false),
    ('Beklemede',         'warn',      2, false, false, false, true),
    ('Bilgi İstendi',     'warn',      3, false, false, false, true),
    ('Danışman Desteği',  'contrast',  4, false, false, false, false),
    ('Geçici Çözüm',      'secondary', 5, false, false, false, false),
    ('Serviste',          'warn',      6, false, false, false, true),
    ('Çözüldü',           'success',   7, false, true,  false, true),
    ('Tamamlandı',        'success',   8, false, true,  true,  true)
) AS v(name, sev, ord, def, res, cls, pause);

INSERT INTO ""Bidesk"".""TicketPriorities""
    (""Name"", ""Severity"", ""SortOrder"", ""IsDefault"", ""ResolveHours"", ""IsActive"", ""CreatedDate"", ""CreatedUserId"", ""IsCanceled"", ""CompanyID"")
SELECT v.name, v.sev, v.ord, v.def, v.hours, true, now(), {seedUser}, false, c.""ID""
FROM public.""Companies"" c
CROSS JOIN (VALUES
    ('Belirtilmemiş', 'secondary', 1, true,  72),
    ('Düşük',         'info',      2, false, 120),
    ('Orta',          'warn',      3, false, 48),
    ('Yüksek',        'danger',    4, false, 8)
) AS v(name, sev, ord, def, hours);

INSERT INTO ""Bidesk"".""TicketGroups""
    (""Name"", ""SortOrder"", ""IsActive"", ""CreatedDate"", ""CreatedUserId"", ""IsCanceled"", ""CompanyID"")
SELECT v.name, v.ord, true, now(), {seedUser}, false, c.""ID""
FROM public.""Companies"" c
CROSS JOIN (VALUES
    ('Belirtilmemiş', 1),
    ('Bilgi Güvenliği', 2),
    ('Donanım Sorunları', 3),
    ('Yazılım Sorunları', 4)
) AS v(name, ord);

INSERT INTO ""Bidesk"".""BideskParameters""
    (""ParamName"", ""ParamValue"", ""CreatedDate"", ""CreatedUserId"", ""IsCanceled"", ""CompanyID"")
SELECT 'AutoCloseDays', '3', now(), {seedUser}, false, c.""ID""
FROM public.""Companies"" c;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "BideskParameters",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TechnicianGroups",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TicketAttachments",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TicketConversations",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TicketNotifyUsers",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "Tickets",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TicketGroups",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TicketPriorities",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TicketStatuses",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TicketSubCategories",
                schema: "Bidesk");

            migrationBuilder.DropTable(
                name: "TicketCategories",
                schema: "Bidesk");
        }
    }
}
