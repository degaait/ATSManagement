using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATSManagementExternal.Migrations
{
    /// <inheritdoc />
    public partial class addbookauthorviewmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestViewModels",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTypeNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityNameWithColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityNameWithColorAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherServiceType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tbl_ADREventTypes",
                columns: table => new
                {
                    TypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TypeNames = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ADREventTypes", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_AssignementTypes",
                columns: table => new
                {
                    AssigneeTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AssigneeType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AssigneeTypeAmharic = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tlb_AssignementTypes", x => x.AssigneeTypeID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CivilJusticeCaseType",
                columns: table => new
                {
                    CaseTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    CaseTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CivilJusticeCaseType", x => x.CaseTypeID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CompanyEmails",
                columns: table => new
                {
                    EmailID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    EmailAdress = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CompanyEmails", x => x.EmailID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ContactInformations",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactDetaiMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileUplaod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ContactInformations", x => x.ContactID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Country",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CourtAppointment",
                columns: table => new
                {
                    ChatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ChatContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentID = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    IsDephead = table.Column<bool>(type: "bit", nullable: true),
                    IsExpert = table.Column<bool>(type: "bit", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CourtAppointment", x => x.ChatID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_DebatePerformances",
                columns: table => new
                {
                    PerformanceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PreformanceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceNameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DebatePerformances", x => x.PerformanceID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_DecisionStatus",
                columns: table => new
                {
                    DesStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    StatusName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StatusDescription = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    StatusWithColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusNameAmharic = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    StatusWithColorAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DecisionStatus", x => x.DesStatusId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Department",
                columns: table => new
                {
                    DepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    DepName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DepCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DepNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Department", x => x.DepId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ExternalMainMenus",
                columns: table => new
                {
                    MenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MenuName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MenuDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Class_svg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ExternalMainMenus", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ExternalRequestStatus",
                columns: table => new
                {
                    ExternalRequestStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    StatusName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StatusWithColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusNameAmharic = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StatusWithColorAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ExternalRequestStatus", x => x.ExternalRequestStatusID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Inistitutions",
                columns: table => new
                {
                    InistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    NameAmharic = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Inistitutions", x => x.InistId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_InspectionLaws",
                columns: table => new
                {
                    LawId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    LawDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceArticle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_InspectionLaws", x => x.LawId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_InspectionStatus",
                columns: table => new
                {
                    ProId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ProstatusTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgressOrder = table.Column<int>(type: "int", nullable: true),
                    StatusWithColor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_InspectionStatus", x => x.ProId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Languages",
                columns: table => new
                {
                    LangID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tbl_LegalAdviceServantTypes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ServantTypeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LegalAdviceServantTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_LegalDraftingDocType",
                columns: table => new
                {
                    DocID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    DocName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DocDesciption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocmentOrder = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LegalDraftingDocType", x => x.DocID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_LegalDraftingQuestionType",
                columns: table => new
                {
                    QuestTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    QuestTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    QuestTypeDescription = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LegalDraftingQuestionType", x => x.QuestTypeID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_MainMenu",
                columns: table => new
                {
                    MenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MenuName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MenuDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Class_svg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuNameAmharic = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_MainMenu", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Months",
                columns: table => new
                {
                    MonthID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MonthName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Months", x => x.MonthID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Priority",
                columns: table => new
                {
                    PriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PriorityName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PriorityNameWithColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityNameWithColorAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Priority", x => x.PriorityId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_PriorityQuestions",
                columns: table => new
                {
                    PriorityQueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    QuestionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionsNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_PriorityQuestions", x => x.PriorityQueId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RecomendationStatus",
                columns: table => new
                {
                    RecostatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Status = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StatusColour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusColourAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RecomendationStatus", x => x.RecostatusID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ReponseStatus",
                columns: table => new
                {
                    ResponseStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusWithColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusWithColourAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ReponseStatus", x => x.ResponseStatusId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ReportServiceTypes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ReportSeriviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ReportServiceTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RequestAssignementTypes",
                columns: table => new
                {
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RequestAssignementTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RequestTypes",
                columns: table => new
                {
                    TypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TypeNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RequestTypes", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Roles",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Status",
                columns: table => new
                {
                    StatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Status = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    StatusWithColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusWithColorAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Status_1", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_TermsAndConditions",
                columns: table => new
                {
                    TermsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_TermsAndConditions", x => x.TermsID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_TopStatus",
                columns: table => new
                {
                    TopStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    StatusNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusHtml = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_TopStatus", x => x.TopStatusID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Years",
                columns: table => new
                {
                    YearID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Year = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Years", x => x.YearID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SubDebatePerformances",
                columns: table => new
                {
                    SubPerformanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    SubPerformanceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SubDebatePerformances", x => x.SubPerformanceId);
                    table.ForeignKey(
                        name: "FK_tbl_SubDebatePerformances_tbl_DebatePerformances",
                        column: x => x.PerformanceID,
                        principalTable: "tbl_DebatePerformances",
                        principalColumn: "PerformanceID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ServiceTypes",
                columns: table => new
                {
                    ServiceTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ServiceTypeName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    DepID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceTypeNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceOrderOrder = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ServiceTypes", x => x.ServiceTypeID);
                    table.ForeignKey(
                        name: "FK_tbl_ServiceTypes_tbl_Department",
                        column: x => x.DepID,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ExternalSubmenu",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Submenu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmenuAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ExternalSubmenu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_ExternalSubmenu_tbl_Department",
                        column: x => x.DepId,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId");
                    table.ForeignKey(
                        name: "FK_tbl_ExternalSubmenu_tbl_ExternalMainMenus",
                        column: x => x.MenuID,
                        principalTable: "tbl_ExternalMainMenus",
                        principalColumn: "MenuID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ExternalUser",
                columns: table => new
                {
                    ExterUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FirstName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    InistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    AcceptedTerms = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ExternalUser", x => x.ExterUserID);
                    table.ForeignKey(
                        name: "FK_tbl_ExternalUser_tbl_Inistitutions",
                        column: x => x.InistId,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Submenu",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Submenu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmenuAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    forDeputy = table.Column<bool>(type: "bit", nullable: true),
                    forDepHead = table.Column<bool>(type: "bit", nullable: true),
                    forDefaulUser = table.Column<bool>(type: "bit", nullable: true),
                    forTeamLeader = table.Column<bool>(type: "bit", nullable: true),
                    forSuperAdmin = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Submenu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_Submenu_tbl_Department",
                        column: x => x.DepId,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId");
                    table.ForeignKey(
                        name: "FK_tbl_Submenu_tbl_MainMenu",
                        column: x => x.MenuID,
                        principalTable: "tbl_MainMenu",
                        principalColumn: "MenuID");
                    table.ForeignKey(
                        name: "FK_tbl_Submenu_tbl_Roles",
                        column: x => x.RoleID,
                        principalTable: "tbl_Roles",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_DebatePerformanceEventTypes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    WorkPerformanceEventType = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    SubPerformanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkPerformanceEventTypeEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DebatePerformanceEventTypes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_DebatePerformanceEventTypes_tbl_SubDebatePerformances",
                        column: x => x.SubPerformanceId,
                        principalTable: "tbl_SubDebatePerformances",
                        principalColumn: "SubPerformanceId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Events",
                columns: table => new
                {
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    EventName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    EventDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubPerformanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_tbl_Events_tbl_SubDebatePerformances",
                        column: x => x.SubPerformanceId,
                        principalTable: "tbl_SubDebatePerformances",
                        principalColumn: "SubPerformanceId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Appointments",
                columns: table => new
                {
                    AppointmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AppointmentDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InistID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DescusionFinalComeup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowedAppointDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Appointments", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_tbl_Appointments_tbl_ExternalUser",
                        column: x => x.RequestedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_Appointments_tbl_Inistitutions",
                        column: x => x.InistID,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ExternalRequests",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IntID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExterUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalRequestStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ExternalRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_tbl_ExternalRequests_tbl_Department",
                        column: x => x.DepID,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId");
                    table.ForeignKey(
                        name: "FK_tbl_ExternalRequests_tbl_ExternalRequestStatus",
                        column: x => x.ExternalRequestStatusID,
                        principalTable: "tbl_ExternalRequestStatus",
                        principalColumn: "ExternalRequestStatusID");
                    table.ForeignKey(
                        name: "FK_tbl_ExternalRequests_tbl_ExternalUser",
                        column: x => x.ExterUserID,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_ExternalRequests_tbl_Inistitutions",
                        column: x => x.IntID,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Activities",
                columns: table => new
                {
                    ActivityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ActivityDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TimeTakenTocomplete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Activities", x => x.ActivityID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Adjornments",
                columns: table => new
                {
                    AdjoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdjorneyDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    WhatIsDone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExpertHanlingCase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plaintiff_Defendant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheCourtCaseHanled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Adjornments", x => x.AdjoryId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_AdjournmentChats",
                columns: table => new
                {
                    ChatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ChatContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDephead = table.Column<bool>(type: "bit", nullable: true),
                    IsExpert = table.Column<bool>(type: "bit", nullable: true),
                    SendBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdjoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AdjournmentChats", x => x.ChatID);
                    table.ForeignKey(
                        name: "FK_tbl_AdjournmentChats_tbl_Adjornments",
                        column: x => x.AdjoryId,
                        principalTable: "tbl_Adjornments",
                        principalColumn: "AdjoryId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ADRActivitiesReport",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Womens = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Childrens = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WomeElders = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    HIVPositives = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Mens = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    OtherServantType = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    OutofResponsibilty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Property = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AsserinaSerategna = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LelochGudayAyine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YediridirGenzebMeten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YeteWosenewuGenzebMeten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MonthID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ADRActivitiesReport", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_ADRActivitiesReport_tbl_ADREventTypes",
                        column: x => x.TypeID,
                        principalTable: "tbl_ADREventTypes",
                        principalColumn: "TypeID");
                    table.ForeignKey(
                        name: "FK_tbl_ADRActivitiesReport_tbl_Months",
                        column: x => x.MonthID,
                        principalTable: "tbl_Months",
                        principalColumn: "MonthID");
                    table.ForeignKey(
                        name: "FK_tbl_ADRActivitiesReport_tbl_Years",
                        column: x => x.YearID,
                        principalTable: "tbl_Years",
                        principalColumn: "YearID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_AppointmentChats",
                columns: table => new
                {
                    ChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsEnternal = table.Column<bool>(type: "bit", nullable: true),
                    IsInternal = table.Column<bool>(type: "bit", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExterUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AppointmentChats", x => x.ChatId);
                    table.ForeignKey(
                        name: "FK_tbl_AppointmentChats_tbl_Appointments",
                        column: x => x.AppointmentID,
                        principalTable: "tbl_Appointments",
                        principalColumn: "AppointmentID");
                    table.ForeignKey(
                        name: "FK_tbl_AppointmentChats_tbl_ExternalUser",
                        column: x => x.ExterUserID,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_AppointmentParticipants",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AppointmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AppointmentParticipants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_AppointmentParticipants_tbl_AppointmentParticipants",
                        column: x => x.AppointmentID,
                        principalTable: "tbl_Appointments",
                        principalColumn: "AppointmentID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_AssignedYearlyPlans",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AssignedTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "date", nullable: true),
                    DueDate = table.Column<DateTime>(type: "date", nullable: true),
                    EvaluationCheckLists = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngagementLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeetingLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgressStatus = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    StatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinalReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUprovedByDeputy = table.Column<bool>(type: "bit", nullable: true),
                    IsUpprovedbyTeam = table.Column<bool>(type: "bit", nullable: true),
                    IsUpprovedbyDepartment = table.Column<bool>(type: "bit", nullable: true),
                    SpecificPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TORAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvaluationCheckListsAttachmet = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AssignedYearlyPlans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_AssignedYearlyPlans_tbl_Status",
                        column: x => x.StatusID,
                        principalTable: "tbl_Status",
                        principalColumn: "StatusID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Assignees",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Assignees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CivilJustice",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CaseTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AssingmentRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalRequestStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeputyUprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopStatus = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinalReport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CivilJustice", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_CivilJusticeCaseType",
                        column: x => x.CaseTypeID,
                        principalTable: "tbl_CivilJusticeCaseType",
                        principalColumn: "CaseTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_DecisionStatus",
                        column: x => x.DepartmentUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_DecisionStatus1",
                        column: x => x.TeamUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_DecisionStatus2",
                        column: x => x.DeputyUprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_DecisionStatus3",
                        column: x => x.UserUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_Department",
                        column: x => x.DepId,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_ExternalRequestStatus",
                        column: x => x.ExternalRequestStatusID,
                        principalTable: "tbl_ExternalRequestStatus",
                        principalColumn: "ExternalRequestStatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_ExternalUser",
                        column: x => x.RequestedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_Inistitutions",
                        column: x => x.InistId,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJustice_tbl_Priority",
                        column: x => x.PriorityId,
                        principalTable: "tbl_Priority",
                        principalColumn: "PriorityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CivilJusticeChats",
                columns: table => new
                {
                    ChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInternal = table.Column<bool>(type: "bit", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDephead = table.Column<bool>(type: "bit", nullable: true),
                    IsExpert = table.Column<bool>(type: "bit", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CivilJusticeChats", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CivilJusticeRequestActivity",
                columns: table => new
                {
                    ActivityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ActivityDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CivilJusticeRequestActivity", x => x.ActivityID);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJusticeRequestActivity_tbl_CivilJustice",
                        column: x => x.RequestID,
                        principalTable: "tbl_CivilJustice",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CivilJusticeRequestReplys",
                columns: table => new
                {
                    ReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ReplayDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InternalReplayedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReplyDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExternalReplayedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CivilJusticeRequestReplys", x => x.ReplyId);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJusticeRequestReplys_tbl_CivilJustice",
                        column: x => x.RequestId,
                        principalTable: "tbl_CivilJustice",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_CivilJusticeRequestReplys_tbl_ExternalUser",
                        column: x => x.ExternalReplayedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_DebateWorkPerformanceReports",
                columns: table => new
                {
                    WorkPerformID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Womens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Childrens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WomenElders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HIVPositives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherServants = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalServant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutofContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Property = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkDebate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherCaseTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JudgementMoneyAmmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JudgementVerifiedAmmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    YearID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MonthID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubPerformanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DebateWorkPerformanceReports", x => x.WorkPerformID);
                    table.ForeignKey(
                        name: "FK_tbl_DebateWorkPerformanceReports_tbl_DebateWorkPerformanceReports",
                        column: x => x.ID,
                        principalTable: "tbl_DebatePerformanceEventTypes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tbl_DebateWorkPerformanceReports_tbl_Months",
                        column: x => x.MonthID,
                        principalTable: "tbl_Months",
                        principalColumn: "MonthID");
                    table.ForeignKey(
                        name: "FK_tbl_DebateWorkPerformanceReports_tbl_SubDebatePerformances",
                        column: x => x.SubPerformanceId,
                        principalTable: "tbl_SubDebatePerformances",
                        principalColumn: "SubPerformanceId");
                    table.ForeignKey(
                        name: "FK_tbl_DebateWorkPerformanceReports_tbl_Years",
                        column: x => x.YearID,
                        principalTable: "tbl_Years",
                        principalColumn: "YearID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_DesicionRemark",
                columns: table => new
                {
                    DesicionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesicionRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DesicionRemark", x => x.DesicionId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_DocumentHistories",
                columns: table => new
                {
                    HistoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Round = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalReplyID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalRepliedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExactFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DocumentHistories", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_tbl_DocumentHistories_tbl_ExternalUser",
                        column: x => x.ExternalRepliedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_DraftContractExaminationReport",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    QuestionSubmitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullMoneyAmmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Investigation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MonthID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmittedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DraftContractExaminationReport", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_DraftContractExaminationReport_tbl_Months",
                        column: x => x.MonthID,
                        principalTable: "tbl_Months",
                        principalColumn: "MonthID");
                    table.ForeignKey(
                        name: "FK_tbl_DraftContractExaminationReport_tbl_Years",
                        column: x => x.YearID,
                        principalTable: "tbl_Years",
                        principalColumn: "YearID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Followups",
                columns: table => new
                {
                    FollowUpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InistID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    fromExternal = table.Column<bool>(type: "bit", nullable: true),
                    fromInternal = table.Column<bool>(type: "bit", nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Followups", x => x.FollowUpId);
                    table.ForeignKey(
                        name: "FK_tbl_Followups_tbl_ExternalUser",
                        column: x => x.ExternalUserId,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_Followups_tbl_Inistitutions",
                        column: x => x.InistID,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_InpectionActivites",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TimeTakenTocomplete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_InpectionActivites", x => x.ActivityId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Inspection_Institutions",
                columns: table => new
                {
                    SubMissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestStatus = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    RecomendationDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecomendationFeedBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExpectedResponseDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    InstitutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResponseStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReComendationFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReturnedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Inspection_Institutions", x => x.SubMissionId);
                    table.ForeignKey(
                        name: "FK_tbl_Inspection_Institutions_tbl_ExternalUser",
                        column: x => x.ReturnedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_Inspection_Institutions_tbl_Inistitutions",
                        column: x => x.InstitutionId,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                    table.ForeignKey(
                        name: "FK_tbl_Inspection_Institutions_tbl_ReponseStatus",
                        column: x => x.ResponseStatusId,
                        principalTable: "tbl_ReponseStatus",
                        principalColumn: "ResponseStatusId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_InspectionPlans",
                columns: table => new
                {
                    InspectionPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PlanTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    YearID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssigningRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssigneeTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAssignedToUser = table.Column<bool>(type: "bit", nullable: true),
                    IsUprovedByDeputy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsUpprovedbyTeam = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsUpprovedbyDepartment = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinalReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalStatus = table.Column<bool>(type: "bit", nullable: true),
                    SendingRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    returningRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSenttoInst = table.Column<bool>(type: "bit", nullable: true),
                    IsReturned = table.Column<bool>(type: "bit", nullable: true),
                    SentReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAssignedTeam = table.Column<bool>(type: "bit", nullable: true),
                    returnDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    sentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsUserUproved = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OfficialLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExactFileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_InspectionPlans", x => x.InspectionPlanId);
                    table.ForeignKey(
                        name: "FK_tbl_InspectionPlans_tbl_AssignementTypes",
                        column: x => x.AssigneeTypeID,
                        principalTable: "tbl_AssignementTypes",
                        principalColumn: "AssigneeTypeID");
                    table.ForeignKey(
                        name: "FK_tbl_InspectionPlans_tbl_DecisionStatus",
                        column: x => x.IsUpprovedbyDepartment,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_InspectionPlans_tbl_DecisionStatus1",
                        column: x => x.IsUpprovedbyTeam,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_InspectionPlans_tbl_DecisionStatus2",
                        column: x => x.IsUprovedByDeputy,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_InspectionPlans_tbl_DecisionStatus3",
                        column: x => x.IsUserUproved,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_Progress_tbl_InspectionPlans",
                        column: x => x.ProId,
                        principalTable: "tbl_InspectionStatus",
                        principalColumn: "ProId");
                    table.ForeignKey(
                        name: "FK_tbl_Years_tbl_InspectionPlans",
                        column: x => x.YearID,
                        principalTable: "tbl_Years",
                        principalColumn: "YearID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_PlanCatagory",
                columns: table => new
                {
                    PlanCatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CatDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DoesHaveSpecificPlan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_PlanCatagory", x => x.PlanCatId);
                    table.ForeignKey(
                        name: "FK_tbl_PlanCatagory_tbl_InspectionPlans",
                        column: x => x.InspectionPlanId,
                        principalTable: "tbl_InspectionPlans",
                        principalColumn: "InspectionPlanId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_InspectionReplyes",
                columns: table => new
                {
                    ReplyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecId = table.Column<int>(type: "int", nullable: true),
                    RecoDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: true),
                    IsInternal = table.Column<bool>(type: "bit", nullable: true),
                    InternalUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    Attachement = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_InspectionReplyes", x => x.ReplyId);
                    table.ForeignKey(
                        name: "FK_tbl_InspectionReplyes_tbl_ExternalUser",
                        column: x => x.ExternalUser,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_InspectionReportFiles",
                columns: table => new
                {
                    RepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportFiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ForDeputy = table.Column<bool>(type: "bit", nullable: true),
                    ForExpert = table.Column<bool>(type: "bit", nullable: true),
                    forDepartment = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_InspectionReportFiles", x => x.RepId);
                    table.ForeignKey(
                        name: "FK_tbl_InspectionReportFiles_tbl_AssignedYearlyPlans",
                        column: x => x.ID,
                        principalTable: "tbl_AssignedYearlyPlans",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_InstotutionMonitoringReports",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MoniteredOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractMoneyAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADRNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADRMoneyAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebateMoneyAmmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Investigation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MonthID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_InstotutionMonitoringReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_InstotutionMonitoringReports_tbl_Months",
                        column: x => x.MonthID,
                        principalTable: "tbl_Months",
                        principalColumn: "MonthID");
                    table.ForeignKey(
                        name: "FK_tbl_InstotutionMonitoringReports_tbl_Years",
                        column: x => x.YearID,
                        principalTable: "tbl_Years",
                        principalColumn: "YearID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_InternalUsers",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FirstName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MidleName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsTeamLeader = table.Column<bool>(type: "bit", nullable: false),
                    DepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    IsDeputy = table.Column<bool>(type: "bit", nullable: false),
                    IsDepartmentHead = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDefaultUser = table.Column<bool>(type: "bit", nullable: true),
                    IsSecretary = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_InternalUsers", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_tbl_InternalUsers_tbl_Department",
                        column: x => x.DepId,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_LegalAdviceReports",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Investigation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Year = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Total = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LegalAdviceReports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_tbl_LegalAdviceReports_tbl_LegalAdviceReports",
                        column: x => x.ReportedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalAdviceReports_tbl_LegalAdviceServantTypes",
                        column: x => x.ID,
                        principalTable: "tbl_LegalAdviceServantTypes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalAdviceReports_tbl_Months",
                        column: x => x.Month,
                        principalTable: "tbl_Months",
                        principalColumn: "MonthID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalAdviceReports_tbl_Years",
                        column: x => x.Year,
                        principalTable: "tbl_Years",
                        principalColumn: "YearID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_LegalStudiesDrafting",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CaseTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AssingmentRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalRequestStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TopStatus = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeputyUprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinalReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LegalStudiesDrafting", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_DecisionStatus",
                        column: x => x.TeamUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_DecisionStatus1",
                        column: x => x.UserUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_DecisionStatus2",
                        column: x => x.DeputyUprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_DecisionStatus3",
                        column: x => x.DepartmentUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_Department",
                        column: x => x.DepId,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_ExternalRequestStatus",
                        column: x => x.ExternalRequestStatusID,
                        principalTable: "tbl_ExternalRequestStatus",
                        principalColumn: "ExternalRequestStatusID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_ExternalUser",
                        column: x => x.RequestedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_Inistitutions",
                        column: x => x.InistId,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_InternalUsers",
                        column: x => x.AssignedTo,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_InternalUsers1",
                        column: x => x.AssignedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_InternalUsers2",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_LegalDraftingDocType",
                        column: x => x.DocID,
                        principalTable: "tbl_LegalDraftingDocType",
                        principalColumn: "DocID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_LegalDraftingQuestionType",
                        column: x => x.QuestTypeID,
                        principalTable: "tbl_LegalDraftingQuestionType",
                        principalColumn: "QuestTypeID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesDrafting_tbl_Priority",
                        column: x => x.PriorityId,
                        principalTable: "tbl_Priority",
                        principalColumn: "PriorityId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsChecked = table.Column<bool>(type: "bit", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExterUserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FromExternal = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Notifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_tbl_Notifications_tbl_ExternalUser",
                        column: x => x.ExterUserID,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_Notifications_tbl_InternalUsers",
                        column: x => x.UserID,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_Notifications_tbl_InternalUsers1",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Recomendation",
                columns: table => new
                {
                    RecoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Recomendation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecostatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    YearID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatinDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LawId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecomendationTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Recomendation", x => x.RecoId);
                    table.ForeignKey(
                        name: "FK_tbl_Recomendation_tbl_Inistitutions",
                        column: x => x.InistId,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                    table.ForeignKey(
                        name: "FK_tbl_Recomendation_tbl_InspectionLaws",
                        column: x => x.LawId,
                        principalTable: "tbl_InspectionLaws",
                        principalColumn: "LawId");
                    table.ForeignKey(
                        name: "FK_tbl_Recomendation_tbl_InternalUsers",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_Recomendation_tbl_Recomendation2",
                        column: x => x.RecostatusID,
                        principalTable: "tbl_RecomendationStatus",
                        principalColumn: "RecostatusID");
                    table.ForeignKey(
                        name: "FK_tbl_Recomendation_tbl_Years",
                        column: x => x.YearID,
                        principalTable: "tbl_Years",
                        principalColumn: "YearID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Requests",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAssignedTodepartment = table.Column<bool>(type: "bit", nullable: true),
                    CaseTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AssingmentRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalRequestStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TopStatusID = table.Column<int>(type: "int", nullable: true),
                    PriorityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeputyUprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentUpprovalStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinalReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestRound = table.Column<int>(type: "int", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LetterofUpproval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSenttoInst = table.Column<bool>(type: "bit", nullable: true),
                    IsReturned = table.Column<bool>(type: "bit", nullable: true),
                    returningRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sentDate = table.Column<DateTime>(type: "date", nullable: true),
                    SendingRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoneyAmount = table.Column<int>(type: "int", nullable: true),
                    MoneyCurrency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ADRType = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Claimant = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ActingAs = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Respondent = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Reasult = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ResultDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CourtCenter = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    DateOfAdjournment = table.Column<DateTime>(type: "date", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LitigationType = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Jursidiction = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Bench = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Plaintiful = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Defendent = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    DateofJudgement = table.Column<DateTime>(type: "date", nullable: true),
                    DeputyRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    teamDesicionRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    departmentDesicionRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deputyDesicionRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalReportSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractNeServiceType = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ContractNeStatus = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ContractNeResult = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    LegalAdviceStatus = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    LegalAdviceResult = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    InternationalCaseStatus = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    InternationalCaseResult = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    AdrStatus = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    AdrResult = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ListigationStatus = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ListigationResult = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OtherServiceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherDocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Requests", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_CivilJusticeCaseType",
                        column: x => x.CaseTypeID,
                        principalTable: "tbl_CivilJusticeCaseType",
                        principalColumn: "CaseTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_DecisionStatus",
                        column: x => x.UserUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_DecisionStatus1",
                        column: x => x.TeamUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_DecisionStatus2",
                        column: x => x.DepartmentUpprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_DecisionStatus3",
                        column: x => x.DeputyUprovalStatus,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_ExternalRequestStatus",
                        column: x => x.ExternalRequestStatusID,
                        principalTable: "tbl_ExternalRequestStatus",
                        principalColumn: "ExternalRequestStatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_ExternalUser",
                        column: x => x.RequestedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_Inistitutions",
                        column: x => x.InistId,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_InternalUsers",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_InternalUsers1",
                        column: x => x.AssignedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_LegalDraftingDocType",
                        column: x => x.DocTypeId,
                        principalTable: "tbl_LegalDraftingDocType",
                        principalColumn: "DocID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_LegalDraftingQuestionType",
                        column: x => x.QuestTypeID,
                        principalTable: "tbl_LegalDraftingQuestionType",
                        principalColumn: "QuestTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_Priority",
                        column: x => x.PriorityId,
                        principalTable: "tbl_Priority",
                        principalColumn: "PriorityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_RequestAssignementTypes",
                        column: x => x.TypeId,
                        principalTable: "tbl_RequestAssignementTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_Requests",
                        column: x => x.TopStatusID,
                        principalTable: "tbl_TopStatus",
                        principalColumn: "TopStatusID");
                    table.ForeignKey(
                        name: "FK_tbl_Requests_tbl_ServiceTypes",
                        column: x => x.ServiceTypeID,
                        principalTable: "tbl_ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Teams",
                columns: table => new
                {
                    TeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DepID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamLeaderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamNameAmharic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Teams", x => x.TeamID);
                    table.ForeignKey(
                        name: "FK_tbl_Teams_tbl_Department",
                        column: x => x.DepID,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId");
                    table.ForeignKey(
                        name: "FK_tbl_Teams_tbl_InternalUsers",
                        column: x => x.TeamLeaderID,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_LegalStudiesActivity",
                columns: table => new
                {
                    ActivityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ActivityDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LegalStudiesActivity", x => x.ActivityID);
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesActivity_tbl_InternalUsers",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesActivity_tbl_LegalStudiesDrafting",
                        column: x => x.RequestID,
                        principalTable: "tbl_LegalStudiesDrafting",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_LegalStudiesReplays",
                columns: table => new
                {
                    ReplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ReplayDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InternalReplayedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReplyDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExternalReplayedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LegalStudiesReplays", x => x.ReplyId);
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesReplays_tbl_ExternalUser",
                        column: x => x.ExternalReplayedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesReplays_tbl_InternalUsers",
                        column: x => x.ExternalReplayedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesReplays_tbl_LegalStudiesDrafting",
                        column: x => x.RequestId,
                        principalTable: "tbl_LegalStudiesDrafting",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_LegalStudiesChats",
                columns: table => new
                {
                    ChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInternal = table.Column<bool>(type: "bit", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Datetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDephead = table.Column<bool>(type: "bit", nullable: true),
                    IsExpert = table.Column<bool>(type: "bit", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_LegalStudiesChats", x => x.ChatId);
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesChats_tbl_InternalUsers",
                        column: x => x.UserID,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesChats_tbl_InternalUsers1",
                        column: x => x.SendBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesChats_tbl_InternalUsers2",
                        column: x => x.SendTo,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_LegalStudiesChats_tbl_Requests",
                        column: x => x.RequestID,
                        principalTable: "tbl_Requests",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Replays",
                columns: table => new
                {
                    ReplyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ReplayDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InternalReplayedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReplyDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExternalReplayedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: true),
                    IsInternal = table.Column<bool>(type: "bit", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Replays", x => x.ReplyID);
                    table.ForeignKey(
                        name: "FK_tbl_Replays_tbl_ExternalUser",
                        column: x => x.ExternalReplayedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_Replays_tbl_InternalUsers",
                        column: x => x.InternalReplayedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_Replays_tbl_Requests",
                        column: x => x.RequestId,
                        principalTable: "tbl_Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RequestAssignees",
                columns: table => new
                {
                    AssigneeRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RequestAssignees", x => x.AssigneeRequestId);
                    table.ForeignKey(
                        name: "FK_tbl_RequestAssignees_tbl_InternalUsers",
                        column: x => x.UserID,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_RequestAssignees_tbl_Requests",
                        column: x => x.RequestID,
                        principalTable: "tbl_Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RequestPriorityQuestionsRelations",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PriorityQueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RequestPriorityQuestionsRelations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_RequestPriorityQuestionsRelations_tbl_PriorityQuestions",
                        column: x => x.PriorityQueId,
                        principalTable: "tbl_PriorityQuestions",
                        principalColumn: "PriorityQueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_RequestPriorityQuestionsRelations_tbl_Requests",
                        column: x => x.RequestID,
                        principalTable: "tbl_Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Rounds",
                columns: table => new
                {
                    RoundID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestIID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoundNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Rounds", x => x.RoundID);
                    table.ForeignKey(
                        name: "FK_tbl_Rounds_tbl_Requests",
                        column: x => x.RequestIID,
                        principalTable: "tbl_Requests",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Witness_Evidences",
                columns: table => new
                {
                    WitnessID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    WitnessesName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvidenceFiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvidenceVideos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Witness_Evidences", x => x.WitnessID);
                    table.ForeignKey(
                        name: "FK_tbl_Witness_Evidences_tbl_InternalUsers",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_Witness_Evidences_tbl_Requests",
                        column: x => x.RequestID,
                        principalTable: "tbl_Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBL_RequestDepartmentRelations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssigneeTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAssingedToUser = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_RequestDepartmentRelations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TBL_RequestDepartmentRelations_tbl_AssignementTypes",
                        column: x => x.AssigneeTypeID,
                        principalTable: "tbl_AssignementTypes",
                        principalColumn: "AssigneeTypeID");
                    table.ForeignKey(
                        name: "FK_TBL_RequestDepartmentRelations_tbl_Department",
                        column: x => x.DepID,
                        principalTable: "tbl_Department",
                        principalColumn: "DepId");
                    table.ForeignKey(
                        name: "FK_TBL_RequestDepartmentRelations_tbl_Requests",
                        column: x => x.RequestID,
                        principalTable: "tbl_Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBL_RequestDepartmentRelations_tbl_Teams",
                        column: x => x.TeamID,
                        principalTable: "tbl_Teams",
                        principalColumn: "TeamID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_SpecificPlans",
                columns: table => new
                {
                    SpecificPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InspectionPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlanCatId = table.Column<int>(type: "int", nullable: true),
                    IsAssignedToUser = table.Column<bool>(type: "bit", nullable: true),
                    IsAssignedToTeam = table.Column<bool>(type: "bit", nullable: true),
                    AssigningRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssigneeTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinalReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUprovedByDeputy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsUpprovedbyTeam = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsUpprovedbyDepartment = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinalStatus = table.Column<bool>(type: "bit", nullable: true),
                    IsUserUproved = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SpecificPlans", x => x.SpecificPlanId);
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_AssignementTypes",
                        column: x => x.AssigneeTypeID,
                        principalTable: "tbl_AssignementTypes",
                        principalColumn: "AssigneeTypeID");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_DecisionStatus",
                        column: x => x.IsUpprovedbyDepartment,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_DecisionStatus1",
                        column: x => x.IsUpprovedbyTeam,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_DecisionStatus2",
                        column: x => x.IsUprovedByDeputy,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_DecisionStatus3",
                        column: x => x.IsUserUproved,
                        principalTable: "tbl_DecisionStatus",
                        principalColumn: "DesStatusId");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_Inistitutions",
                        column: x => x.InistId,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_InspectionPlans",
                        column: x => x.InspectionPlanId,
                        principalTable: "tbl_InspectionPlans",
                        principalColumn: "InspectionPlanId");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_InspectionStatus",
                        column: x => x.ProId,
                        principalTable: "tbl_InspectionStatus",
                        principalColumn: "ProId");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_InternalUsers",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_PlanCatagory",
                        column: x => x.PlanCatId,
                        principalTable: "tbl_PlanCatagory",
                        principalColumn: "PlanCatId");
                    table.ForeignKey(
                        name: "FK_tbl_SpecificPlans_tbl_Teams",
                        column: x => x.TeamID,
                        principalTable: "tbl_Teams",
                        principalColumn: "TeamID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_Plan_Inistitution",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    InistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SpecificPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Plan_Inistitution", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_Plan_Inistitution_tbl_Inistitutions",
                        column: x => x.InistId,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                    table.ForeignKey(
                        name: "FK_tbl_Plan_Inistitution_tbl_SpecificPlans",
                        column: x => x.SpecificPlanId,
                        principalTable: "tbl_SpecificPlans",
                        principalColumn: "SpecificPlanId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ReplyResponseWithExpert",
                columns: table => new
                {
                    RepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportFiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ReplyResponseWithExpert", x => x.RepId);
                    table.ForeignKey(
                        name: "FK_tbl_ReplyResponseWithExpert_tbl_AssignedYearlyPlans",
                        column: x => x.ID,
                        principalTable: "tbl_AssignedYearlyPlans",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tbl_ReplyResponseWithExpert_tbl_InternalUsers",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_ReplyResponseWithExpert_tbl_SpecificPlans",
                        column: x => x.SpecificPlanId,
                        principalTable: "tbl_SpecificPlans",
                        principalColumn: "SpecificPlanId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_ReplyResponseWithStateMinster",
                columns: table => new
                {
                    RepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportFiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ReplyResponseWithStateMinster", x => x.RepId);
                    table.ForeignKey(
                        name: "FK_tbl_ReplyResponseWithStateMinster_tbl_AssignedYearlyPlans",
                        column: x => x.ID,
                        principalTable: "tbl_AssignedYearlyPlans",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tbl_ReplyResponseWithStateMinster_tbl_InternalUsers",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_ReplyResponseWithStateMinster_tbl_SpecificPlans",
                        column: x => x.SpecificPlanId,
                        principalTable: "tbl_SpecificPlans",
                        principalColumn: "SpecificPlanId");
                });

            migrationBuilder.CreateTable(
                name: "tbl_SentInspections",
                columns: table => new
                {
                    RecID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SentReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficialLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SendingRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExpectedReplyDate = table.Column<DateTime>(type: "date", nullable: true),
                    ResponseDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepliedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RespondedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SentBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InspectionPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsChatCloset = table.Column<bool>(type: "bit", nullable: true),
                    SpecificPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SentInspections", x => x.RecID);
                    table.ForeignKey(
                        name: "FK_tbl_SentInspections_tbl_ExternalUser",
                        column: x => x.RepliedBy,
                        principalTable: "tbl_ExternalUser",
                        principalColumn: "ExterUserID");
                    table.ForeignKey(
                        name: "FK_tbl_SentInspections_tbl_Inistitutions",
                        column: x => x.InstID,
                        principalTable: "tbl_Inistitutions",
                        principalColumn: "InistId");
                    table.ForeignKey(
                        name: "FK_tbl_SentInspections_tbl_InspectionPlans",
                        column: x => x.InspectionPlanId,
                        principalTable: "tbl_InspectionPlans",
                        principalColumn: "InspectionPlanId");
                    table.ForeignKey(
                        name: "FK_tbl_SentInspections_tbl_InternalUsers",
                        column: x => x.SentBy,
                        principalTable: "tbl_InternalUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_tbl_SentInspections_tbl_SpecificPlans",
                        column: x => x.SpecificPlanId,
                        principalTable: "tbl_SpecificPlans",
                        principalColumn: "SpecificPlanId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Activities_CreatedBy",
                table: "tbl_Activities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Activities_RequestID",
                table: "tbl_Activities",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Adjornments_CreatedBy",
                table: "tbl_Adjornments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Adjornments_RequestID",
                table: "tbl_Adjornments",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AdjournmentChats_AdjoryId",
                table: "tbl_AdjournmentChats",
                column: "AdjoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AdjournmentChats_SendBy",
                table: "tbl_AdjournmentChats",
                column: "SendBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AdjournmentChats_SendTo",
                table: "tbl_AdjournmentChats",
                column: "SendTo");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AdjournmentChats_UserID",
                table: "tbl_AdjournmentChats",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ADRActivitiesReport_CreatedBy",
                table: "tbl_ADRActivitiesReport",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ADRActivitiesReport_MonthID",
                table: "tbl_ADRActivitiesReport",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ADRActivitiesReport_TypeID",
                table: "tbl_ADRActivitiesReport",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ADRActivitiesReport_YearID",
                table: "tbl_ADRActivitiesReport",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AppointmentChats_AppointmentID",
                table: "tbl_AppointmentChats",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AppointmentChats_ExterUserID",
                table: "tbl_AppointmentChats",
                column: "ExterUserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AppointmentChats_UserID",
                table: "tbl_AppointmentChats",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AppointmentParticipants_AppointmentID",
                table: "tbl_AppointmentParticipants",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AppointmentParticipants_UserID",
                table: "tbl_AppointmentParticipants",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Appointments_InistID",
                table: "tbl_Appointments",
                column: "InistID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Appointments_RequestedBy",
                table: "tbl_Appointments",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AssignedYearlyPlans_AssignedBy",
                table: "tbl_AssignedYearlyPlans",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AssignedYearlyPlans_AssignedTo",
                table: "tbl_AssignedYearlyPlans",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AssignedYearlyPlans_PlanId",
                table: "tbl_AssignedYearlyPlans",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AssignedYearlyPlans_SpecificPlanId",
                table: "tbl_AssignedYearlyPlans",
                column: "SpecificPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_AssignedYearlyPlans_StatusID",
                table: "tbl_AssignedYearlyPlans",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Assignees_UserID",
                table: "tbl_Assignees",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_AssignedBy",
                table: "tbl_CivilJustice",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_AssignedTo",
                table: "tbl_CivilJustice",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_CaseTypeID",
                table: "tbl_CivilJustice",
                column: "CaseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_CreatedBy",
                table: "tbl_CivilJustice",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_DepartmentUpprovalStatus",
                table: "tbl_CivilJustice",
                column: "DepartmentUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_DepId",
                table: "tbl_CivilJustice",
                column: "DepId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_DeputyUprovalStatus",
                table: "tbl_CivilJustice",
                column: "DeputyUprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_ExternalRequestStatusID",
                table: "tbl_CivilJustice",
                column: "ExternalRequestStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_InistId",
                table: "tbl_CivilJustice",
                column: "InistId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_PriorityId",
                table: "tbl_CivilJustice",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_RequestedBy",
                table: "tbl_CivilJustice",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_TeamUpprovalStatus",
                table: "tbl_CivilJustice",
                column: "TeamUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJustice_UserUpprovalStatus",
                table: "tbl_CivilJustice",
                column: "UserUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeChats_RequestID",
                table: "tbl_CivilJusticeChats",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeChats_SendBy",
                table: "tbl_CivilJusticeChats",
                column: "SendBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeChats_SendTo",
                table: "tbl_CivilJusticeChats",
                column: "SendTo");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeChats_UserID",
                table: "tbl_CivilJusticeChats",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeRequestActivity_CreatedBy",
                table: "tbl_CivilJusticeRequestActivity",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeRequestActivity_RequestID",
                table: "tbl_CivilJusticeRequestActivity",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeRequestReplys_ExternalReplayedBy",
                table: "tbl_CivilJusticeRequestReplys",
                column: "ExternalReplayedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeRequestReplys_InternalReplayedBy",
                table: "tbl_CivilJusticeRequestReplys",
                column: "InternalReplayedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_CivilJusticeRequestReplys_RequestId",
                table: "tbl_CivilJusticeRequestReplys",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DebatePerformanceEventTypes_SubPerformanceId",
                table: "tbl_DebatePerformanceEventTypes",
                column: "SubPerformanceId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DebateWorkPerformanceReports_CreatedBy",
                table: "tbl_DebateWorkPerformanceReports",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DebateWorkPerformanceReports_ID",
                table: "tbl_DebateWorkPerformanceReports",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DebateWorkPerformanceReports_MonthID",
                table: "tbl_DebateWorkPerformanceReports",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DebateWorkPerformanceReports_SubPerformanceId",
                table: "tbl_DebateWorkPerformanceReports",
                column: "SubPerformanceId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DebateWorkPerformanceReports_YearID",
                table: "tbl_DebateWorkPerformanceReports",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DesicionRemark_CreatedBy",
                table: "tbl_DesicionRemark",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DesicionRemark_RequestID",
                table: "tbl_DesicionRemark",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DocumentHistories_ExternalRepliedBy",
                table: "tbl_DocumentHistories",
                column: "ExternalRepliedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DocumentHistories_InternalReplyID",
                table: "tbl_DocumentHistories",
                column: "InternalReplyID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DocumentHistories_RequestID",
                table: "tbl_DocumentHistories",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DraftContractExaminationReport_MonthID",
                table: "tbl_DraftContractExaminationReport",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DraftContractExaminationReport_SubmittedBy",
                table: "tbl_DraftContractExaminationReport",
                column: "SubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DraftContractExaminationReport_YearID",
                table: "tbl_DraftContractExaminationReport",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Events_SubPerformanceId",
                table: "tbl_Events",
                column: "SubPerformanceId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ExternalRequests_DepID",
                table: "tbl_ExternalRequests",
                column: "DepID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ExternalRequests_ExternalRequestStatusID",
                table: "tbl_ExternalRequests",
                column: "ExternalRequestStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ExternalRequests_ExterUserID",
                table: "tbl_ExternalRequests",
                column: "ExterUserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ExternalRequests_IntID",
                table: "tbl_ExternalRequests",
                column: "IntID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ExternalSubmenu_DepId",
                table: "tbl_ExternalSubmenu",
                column: "DepId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ExternalSubmenu_MenuID",
                table: "tbl_ExternalSubmenu",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ExternalUser_InistId",
                table: "tbl_ExternalUser",
                column: "InistId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Followups_ExternalUserId",
                table: "tbl_Followups",
                column: "ExternalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Followups_InistID",
                table: "tbl_Followups",
                column: "InistID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Followups_RequestID",
                table: "tbl_Followups",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Followups_UserId",
                table: "tbl_Followups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InpectionActivites_CreatedBy",
                table: "tbl_InpectionActivites",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InpectionActivites_SpecificPlanId",
                table: "tbl_InpectionActivites",
                column: "SpecificPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Inspection_Institutions_InstitutionId",
                table: "tbl_Inspection_Institutions",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Inspection_Institutions_ResponseStatusId",
                table: "tbl_Inspection_Institutions",
                column: "ResponseStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Inspection_Institutions_ReturnedBy",
                table: "tbl_Inspection_Institutions",
                column: "ReturnedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Inspection_Institutions_SubmittedBy",
                table: "tbl_Inspection_Institutions",
                column: "SubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_AssigneeTypeID",
                table: "tbl_InspectionPlans",
                column: "AssigneeTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_IsUpprovedbyDepartment",
                table: "tbl_InspectionPlans",
                column: "IsUpprovedbyDepartment");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_IsUpprovedbyTeam",
                table: "tbl_InspectionPlans",
                column: "IsUpprovedbyTeam");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_IsUprovedByDeputy",
                table: "tbl_InspectionPlans",
                column: "IsUprovedByDeputy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_IsUserUproved",
                table: "tbl_InspectionPlans",
                column: "IsUserUproved");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_ProId",
                table: "tbl_InspectionPlans",
                column: "ProId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_TeamID",
                table: "tbl_InspectionPlans",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_UserID",
                table: "tbl_InspectionPlans",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionPlans_YearID",
                table: "tbl_InspectionPlans",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionReplyes_ExternalUser",
                table: "tbl_InspectionReplyes",
                column: "ExternalUser");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionReplyes_InternalUser",
                table: "tbl_InspectionReplyes",
                column: "InternalUser");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionReplyes_RecId",
                table: "tbl_InspectionReplyes",
                column: "RecId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionReportFiles_CreatedBy",
                table: "tbl_InspectionReportFiles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionReportFiles_ID",
                table: "tbl_InspectionReportFiles",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InspectionReportFiles_SpecificPlanId",
                table: "tbl_InspectionReportFiles",
                column: "SpecificPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InstotutionMonitoringReports_CreatedBy",
                table: "tbl_InstotutionMonitoringReports",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InstotutionMonitoringReports_MonthID",
                table: "tbl_InstotutionMonitoringReports",
                column: "MonthID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InstotutionMonitoringReports_YearID",
                table: "tbl_InstotutionMonitoringReports",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InternalUsers_DepId",
                table: "tbl_InternalUsers",
                column: "DepId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_InternalUsers_TeamID",
                table: "tbl_InternalUsers",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalAdviceReports_ID",
                table: "tbl_LegalAdviceReports",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalAdviceReports_Month",
                table: "tbl_LegalAdviceReports",
                column: "Month");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalAdviceReports_ReportedBy",
                table: "tbl_LegalAdviceReports",
                column: "ReportedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalAdviceReports_Year",
                table: "tbl_LegalAdviceReports",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesActivity_CreatedBy",
                table: "tbl_LegalStudiesActivity",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesActivity_RequestID",
                table: "tbl_LegalStudiesActivity",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesChats_RequestID",
                table: "tbl_LegalStudiesChats",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesChats_SendBy",
                table: "tbl_LegalStudiesChats",
                column: "SendBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesChats_SendTo",
                table: "tbl_LegalStudiesChats",
                column: "SendTo");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesChats_UserID",
                table: "tbl_LegalStudiesChats",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_AssignedBy",
                table: "tbl_LegalStudiesDrafting",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_AssignedTo",
                table: "tbl_LegalStudiesDrafting",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_CreatedBy",
                table: "tbl_LegalStudiesDrafting",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_DepartmentUpprovalStatus",
                table: "tbl_LegalStudiesDrafting",
                column: "DepartmentUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_DepId",
                table: "tbl_LegalStudiesDrafting",
                column: "DepId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_DeputyUprovalStatus",
                table: "tbl_LegalStudiesDrafting",
                column: "DeputyUprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_DocID",
                table: "tbl_LegalStudiesDrafting",
                column: "DocID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_ExternalRequestStatusID",
                table: "tbl_LegalStudiesDrafting",
                column: "ExternalRequestStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_InistId",
                table: "tbl_LegalStudiesDrafting",
                column: "InistId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_PriorityId",
                table: "tbl_LegalStudiesDrafting",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_QuestTypeID",
                table: "tbl_LegalStudiesDrafting",
                column: "QuestTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_RequestedBy",
                table: "tbl_LegalStudiesDrafting",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_TeamUpprovalStatus",
                table: "tbl_LegalStudiesDrafting",
                column: "TeamUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesDrafting_UserUpprovalStatus",
                table: "tbl_LegalStudiesDrafting",
                column: "UserUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesReplays_ExternalReplayedBy",
                table: "tbl_LegalStudiesReplays",
                column: "ExternalReplayedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_LegalStudiesReplays_RequestId",
                table: "tbl_LegalStudiesReplays",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Notifications_CreatedBy",
                table: "tbl_Notifications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Notifications_ExterUserID",
                table: "tbl_Notifications",
                column: "ExterUserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Notifications_UserID",
                table: "tbl_Notifications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Plan_Inistitution_InistId",
                table: "tbl_Plan_Inistitution",
                column: "InistId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Plan_Inistitution_SpecificPlanId",
                table: "tbl_Plan_Inistitution",
                column: "SpecificPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_PlanCatagory_InspectionPlanId",
                table: "tbl_PlanCatagory",
                column: "InspectionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Recomendation_CreatedBy",
                table: "tbl_Recomendation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Recomendation_InistId",
                table: "tbl_Recomendation",
                column: "InistId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Recomendation_LawId",
                table: "tbl_Recomendation",
                column: "LawId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Recomendation_RecostatusID",
                table: "tbl_Recomendation",
                column: "RecostatusID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Recomendation_YearID",
                table: "tbl_Recomendation",
                column: "YearID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Replays_ExternalReplayedBy",
                table: "tbl_Replays",
                column: "ExternalReplayedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Replays_InternalReplayedBy",
                table: "tbl_Replays",
                column: "InternalReplayedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Replays_RequestId",
                table: "tbl_Replays",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ReplyResponseWithExpert_CreatedBy",
                table: "tbl_ReplyResponseWithExpert",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ReplyResponseWithExpert_ID",
                table: "tbl_ReplyResponseWithExpert",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ReplyResponseWithExpert_SpecificPlanId",
                table: "tbl_ReplyResponseWithExpert",
                column: "SpecificPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ReplyResponseWithStateMinster_CreatedBy",
                table: "tbl_ReplyResponseWithStateMinster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ReplyResponseWithStateMinster_ID",
                table: "tbl_ReplyResponseWithStateMinster",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ReplyResponseWithStateMinster_SpecificPlanId",
                table: "tbl_ReplyResponseWithStateMinster",
                column: "SpecificPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RequestAssignees_RequestID",
                table: "tbl_RequestAssignees",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RequestAssignees_UserID",
                table: "tbl_RequestAssignees",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TBL_RequestDepartmentRelations_AssigneeTypeID",
                table: "TBL_RequestDepartmentRelations",
                column: "AssigneeTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TBL_RequestDepartmentRelations_DepID",
                table: "TBL_RequestDepartmentRelations",
                column: "DepID");

            migrationBuilder.CreateIndex(
                name: "IX_TBL_RequestDepartmentRelations_RequestID",
                table: "TBL_RequestDepartmentRelations",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_TBL_RequestDepartmentRelations_TeamID",
                table: "TBL_RequestDepartmentRelations",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RequestPriorityQuestionsRelations_PriorityQueId",
                table: "tbl_RequestPriorityQuestionsRelations",
                column: "PriorityQueId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_RequestPriorityQuestionsRelations_RequestID",
                table: "tbl_RequestPriorityQuestionsRelations",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_AssignedBy",
                table: "tbl_Requests",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_CaseTypeID",
                table: "tbl_Requests",
                column: "CaseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_CreatedBy",
                table: "tbl_Requests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_DepartmentUpprovalStatus",
                table: "tbl_Requests",
                column: "DepartmentUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_DeputyUprovalStatus",
                table: "tbl_Requests",
                column: "DeputyUprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_DocTypeId",
                table: "tbl_Requests",
                column: "DocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_ExternalRequestStatusID",
                table: "tbl_Requests",
                column: "ExternalRequestStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_InistId",
                table: "tbl_Requests",
                column: "InistId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_PriorityId",
                table: "tbl_Requests",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_QuestTypeID",
                table: "tbl_Requests",
                column: "QuestTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_RequestedBy",
                table: "tbl_Requests",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_ServiceTypeID",
                table: "tbl_Requests",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_TeamUpprovalStatus",
                table: "tbl_Requests",
                column: "TeamUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_TopStatusID",
                table: "tbl_Requests",
                column: "TopStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_TypeId",
                table: "tbl_Requests",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Requests_UserUpprovalStatus",
                table: "tbl_Requests",
                column: "UserUpprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Rounds_RequestIID",
                table: "tbl_Rounds",
                column: "RequestIID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SentInspections_InspectionPlanId",
                table: "tbl_SentInspections",
                column: "InspectionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SentInspections_InstID",
                table: "tbl_SentInspections",
                column: "InstID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SentInspections_RepliedBy",
                table: "tbl_SentInspections",
                column: "RepliedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SentInspections_SentBy",
                table: "tbl_SentInspections",
                column: "SentBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SentInspections_SpecificPlanId",
                table: "tbl_SentInspections",
                column: "SpecificPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ServiceTypes_DepID",
                table: "tbl_ServiceTypes",
                column: "DepID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_AssigneeTypeID",
                table: "tbl_SpecificPlans",
                column: "AssigneeTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_CreatedBy",
                table: "tbl_SpecificPlans",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_InistId",
                table: "tbl_SpecificPlans",
                column: "InistId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_InspectionPlanId",
                table: "tbl_SpecificPlans",
                column: "InspectionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_IsUpprovedbyDepartment",
                table: "tbl_SpecificPlans",
                column: "IsUpprovedbyDepartment");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_IsUpprovedbyTeam",
                table: "tbl_SpecificPlans",
                column: "IsUpprovedbyTeam");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_IsUprovedByDeputy",
                table: "tbl_SpecificPlans",
                column: "IsUprovedByDeputy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_IsUserUproved",
                table: "tbl_SpecificPlans",
                column: "IsUserUproved");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_PlanCatId",
                table: "tbl_SpecificPlans",
                column: "PlanCatId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_ProId",
                table: "tbl_SpecificPlans",
                column: "ProId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SpecificPlans_TeamID",
                table: "tbl_SpecificPlans",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SubDebatePerformances_PerformanceID",
                table: "tbl_SubDebatePerformances",
                column: "PerformanceID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Submenu_DepId",
                table: "tbl_Submenu",
                column: "DepId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Submenu_MenuID",
                table: "tbl_Submenu",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Submenu_RoleID",
                table: "tbl_Submenu",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Teams_DepID",
                table: "tbl_Teams",
                column: "DepID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Teams_TeamLeaderID",
                table: "tbl_Teams",
                column: "TeamLeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Witness_Evidences_CreatedBy",
                table: "tbl_Witness_Evidences",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Witness_Evidences_RequestID",
                table: "tbl_Witness_Evidences",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Activities_tbl_InternalUsers",
                table: "tbl_Activities",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Activities_tbl_Requests",
                table: "tbl_Activities",
                column: "RequestID",
                principalTable: "tbl_Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Adjornments_tbl_InternalUsers",
                table: "tbl_Adjornments",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Adjornments_tbl_Requests",
                table: "tbl_Adjornments",
                column: "RequestID",
                principalTable: "tbl_Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AdjournmentChats_tbl_InternalUsers",
                table: "tbl_AdjournmentChats",
                column: "UserID",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AdjournmentChats_tbl_InternalUsers1",
                table: "tbl_AdjournmentChats",
                column: "SendBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AdjournmentChats_tbl_InternalUsers2",
                table: "tbl_AdjournmentChats",
                column: "SendTo",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_ADRActivitiesReport_tbl_InternalUsers",
                table: "tbl_ADRActivitiesReport",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AppointmentChats_tbl_InternalUsers",
                table: "tbl_AppointmentChats",
                column: "UserID",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AppointmentParticipants_tbl_InternalUsers",
                table: "tbl_AppointmentParticipants",
                column: "UserID",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AssignedYearlyPlans_tbl_AssignedBy",
                table: "tbl_AssignedYearlyPlans",
                column: "AssignedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AssignedYearlyPlans_tbl_InternalUsers",
                table: "tbl_AssignedYearlyPlans",
                column: "AssignedTo",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AssignedYearlyPlans_tbl_InspectionPlans",
                table: "tbl_AssignedYearlyPlans",
                column: "PlanId",
                principalTable: "tbl_InspectionPlans",
                principalColumn: "InspectionPlanId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_AssignedYearlyPlans_tbl_SpecificPlans",
                table: "tbl_AssignedYearlyPlans",
                column: "SpecificPlanId",
                principalTable: "tbl_SpecificPlans",
                principalColumn: "SpecificPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Assignees_tbl_InternalUsers",
                table: "tbl_Assignees",
                column: "UserID",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJustice_tbl_InternalUsers",
                table: "tbl_CivilJustice",
                column: "AssignedTo",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJustice_tbl_InternalUsers1",
                table: "tbl_CivilJustice",
                column: "AssignedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJustice_tbl_InternalUsers2",
                table: "tbl_CivilJustice",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJusticeChats_tbl_InternalUsers",
                table: "tbl_CivilJusticeChats",
                column: "UserID",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJusticeChats_tbl_InternalUsers1",
                table: "tbl_CivilJusticeChats",
                column: "SendBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJusticeChats_tbl_InternalUsers2",
                table: "tbl_CivilJusticeChats",
                column: "SendTo",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJusticeChats_tbl_Requests",
                table: "tbl_CivilJusticeChats",
                column: "RequestID",
                principalTable: "tbl_Requests",
                principalColumn: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJusticeRequestActivity_tbl_InternalUsers",
                table: "tbl_CivilJusticeRequestActivity",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_CivilJusticeRequestReplys_tbl_InternalUsers",
                table: "tbl_CivilJusticeRequestReplys",
                column: "InternalReplayedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_DebateWorkPerformanceReports_tbl_InternalUsers",
                table: "tbl_DebateWorkPerformanceReports",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_DesicionRemark_tbl_InternalUsers",
                table: "tbl_DesicionRemark",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_DesicionRemark_tbl_Requests",
                table: "tbl_DesicionRemark",
                column: "RequestID",
                principalTable: "tbl_Requests",
                principalColumn: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_DocumentHistories_tbl_InternalUsers",
                table: "tbl_DocumentHistories",
                column: "InternalReplyID",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_DocumentHistories_tbl_Requests",
                table: "tbl_DocumentHistories",
                column: "RequestID",
                principalTable: "tbl_Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_DraftContractExaminationReport_tbl_InternalUsers",
                table: "tbl_DraftContractExaminationReport",
                column: "SubmittedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Followups_tbl_InternalUsers",
                table: "tbl_Followups",
                column: "UserId",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Followups_tbl_Requests",
                table: "tbl_Followups",
                column: "RequestID",
                principalTable: "tbl_Requests",
                principalColumn: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InpectionActivites_tbl_InternalUsers",
                table: "tbl_InpectionActivites",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InpectionActivites_tbl_SpecificPlans",
                table: "tbl_InpectionActivites",
                column: "SpecificPlanId",
                principalTable: "tbl_SpecificPlans",
                principalColumn: "SpecificPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Inspection_Institutions_tbl_InternalUsers",
                table: "tbl_Inspection_Institutions",
                column: "SubmittedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InspectionPlans_tbl_InternalUsers",
                table: "tbl_InspectionPlans",
                column: "UserID",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InspectionPlans_tbl_Teams",
                table: "tbl_InspectionPlans",
                column: "TeamID",
                principalTable: "tbl_Teams",
                principalColumn: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InspectionReplyes_tbl_InternalUsers1",
                table: "tbl_InspectionReplyes",
                column: "InternalUser",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InspectionReplyes_tbl_SentInspections",
                table: "tbl_InspectionReplyes",
                column: "RecId",
                principalTable: "tbl_SentInspections",
                principalColumn: "RecID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InspectionReportFiles_tbl_InternalUsers",
                table: "tbl_InspectionReportFiles",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InspectionReportFiles_tbl_SpecificPlans",
                table: "tbl_InspectionReportFiles",
                column: "SpecificPlanId",
                principalTable: "tbl_SpecificPlans",
                principalColumn: "SpecificPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InstotutionMonitoringReports_tbl_InternalUsers",
                table: "tbl_InstotutionMonitoringReports",
                column: "CreatedBy",
                principalTable: "tbl_InternalUsers",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_InternalUsers_tbl_Teams",
                table: "tbl_InternalUsers",
                column: "TeamID",
                principalTable: "tbl_Teams",
                principalColumn: "TeamID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Teams_tbl_InternalUsers",
                table: "tbl_Teams");

            migrationBuilder.DropTable(
                name: "RequestViewModels");

            migrationBuilder.DropTable(
                name: "tbl_Activities");

            migrationBuilder.DropTable(
                name: "tbl_AdjournmentChats");

            migrationBuilder.DropTable(
                name: "tbl_ADRActivitiesReport");

            migrationBuilder.DropTable(
                name: "tbl_AppointmentChats");

            migrationBuilder.DropTable(
                name: "tbl_AppointmentParticipants");

            migrationBuilder.DropTable(
                name: "tbl_Assignees");

            migrationBuilder.DropTable(
                name: "tbl_CivilJusticeChats");

            migrationBuilder.DropTable(
                name: "tbl_CivilJusticeRequestActivity");

            migrationBuilder.DropTable(
                name: "tbl_CivilJusticeRequestReplys");

            migrationBuilder.DropTable(
                name: "tbl_CompanyEmails");

            migrationBuilder.DropTable(
                name: "tbl_ContactInformations");

            migrationBuilder.DropTable(
                name: "tbl_Country");

            migrationBuilder.DropTable(
                name: "tbl_CourtAppointment");

            migrationBuilder.DropTable(
                name: "tbl_DebateWorkPerformanceReports");

            migrationBuilder.DropTable(
                name: "tbl_DesicionRemark");

            migrationBuilder.DropTable(
                name: "tbl_DocumentHistories");

            migrationBuilder.DropTable(
                name: "tbl_DraftContractExaminationReport");

            migrationBuilder.DropTable(
                name: "tbl_Events");

            migrationBuilder.DropTable(
                name: "tbl_ExternalRequests");

            migrationBuilder.DropTable(
                name: "tbl_ExternalSubmenu");

            migrationBuilder.DropTable(
                name: "tbl_Followups");

            migrationBuilder.DropTable(
                name: "tbl_InpectionActivites");

            migrationBuilder.DropTable(
                name: "tbl_Inspection_Institutions");

            migrationBuilder.DropTable(
                name: "tbl_InspectionReplyes");

            migrationBuilder.DropTable(
                name: "tbl_InspectionReportFiles");

            migrationBuilder.DropTable(
                name: "tbl_InstotutionMonitoringReports");

            migrationBuilder.DropTable(
                name: "tbl_Languages");

            migrationBuilder.DropTable(
                name: "tbl_LegalAdviceReports");

            migrationBuilder.DropTable(
                name: "tbl_LegalStudiesActivity");

            migrationBuilder.DropTable(
                name: "tbl_LegalStudiesChats");

            migrationBuilder.DropTable(
                name: "tbl_LegalStudiesReplays");

            migrationBuilder.DropTable(
                name: "tbl_Notifications");

            migrationBuilder.DropTable(
                name: "tbl_Plan_Inistitution");

            migrationBuilder.DropTable(
                name: "tbl_Recomendation");

            migrationBuilder.DropTable(
                name: "tbl_Replays");

            migrationBuilder.DropTable(
                name: "tbl_ReplyResponseWithExpert");

            migrationBuilder.DropTable(
                name: "tbl_ReplyResponseWithStateMinster");

            migrationBuilder.DropTable(
                name: "tbl_ReportServiceTypes");

            migrationBuilder.DropTable(
                name: "tbl_RequestAssignees");

            migrationBuilder.DropTable(
                name: "TBL_RequestDepartmentRelations");

            migrationBuilder.DropTable(
                name: "tbl_RequestPriorityQuestionsRelations");

            migrationBuilder.DropTable(
                name: "tbl_RequestTypes");

            migrationBuilder.DropTable(
                name: "tbl_Rounds");

            migrationBuilder.DropTable(
                name: "tbl_Submenu");

            migrationBuilder.DropTable(
                name: "tbl_TermsAndConditions");

            migrationBuilder.DropTable(
                name: "tbl_Witness_Evidences");

            migrationBuilder.DropTable(
                name: "tbl_Adjornments");

            migrationBuilder.DropTable(
                name: "tbl_ADREventTypes");

            migrationBuilder.DropTable(
                name: "tbl_Appointments");

            migrationBuilder.DropTable(
                name: "tbl_CivilJustice");

            migrationBuilder.DropTable(
                name: "tbl_DebatePerformanceEventTypes");

            migrationBuilder.DropTable(
                name: "tbl_ExternalMainMenus");

            migrationBuilder.DropTable(
                name: "tbl_ReponseStatus");

            migrationBuilder.DropTable(
                name: "tbl_SentInspections");

            migrationBuilder.DropTable(
                name: "tbl_LegalAdviceServantTypes");

            migrationBuilder.DropTable(
                name: "tbl_Months");

            migrationBuilder.DropTable(
                name: "tbl_LegalStudiesDrafting");

            migrationBuilder.DropTable(
                name: "tbl_InspectionLaws");

            migrationBuilder.DropTable(
                name: "tbl_RecomendationStatus");

            migrationBuilder.DropTable(
                name: "tbl_AssignedYearlyPlans");

            migrationBuilder.DropTable(
                name: "tbl_PriorityQuestions");

            migrationBuilder.DropTable(
                name: "tbl_MainMenu");

            migrationBuilder.DropTable(
                name: "tbl_Roles");

            migrationBuilder.DropTable(
                name: "tbl_Requests");

            migrationBuilder.DropTable(
                name: "tbl_SubDebatePerformances");

            migrationBuilder.DropTable(
                name: "tbl_SpecificPlans");

            migrationBuilder.DropTable(
                name: "tbl_Status");

            migrationBuilder.DropTable(
                name: "tbl_CivilJusticeCaseType");

            migrationBuilder.DropTable(
                name: "tbl_ExternalRequestStatus");

            migrationBuilder.DropTable(
                name: "tbl_ExternalUser");

            migrationBuilder.DropTable(
                name: "tbl_LegalDraftingDocType");

            migrationBuilder.DropTable(
                name: "tbl_LegalDraftingQuestionType");

            migrationBuilder.DropTable(
                name: "tbl_Priority");

            migrationBuilder.DropTable(
                name: "tbl_RequestAssignementTypes");

            migrationBuilder.DropTable(
                name: "tbl_TopStatus");

            migrationBuilder.DropTable(
                name: "tbl_ServiceTypes");

            migrationBuilder.DropTable(
                name: "tbl_DebatePerformances");

            migrationBuilder.DropTable(
                name: "tbl_PlanCatagory");

            migrationBuilder.DropTable(
                name: "tbl_Inistitutions");

            migrationBuilder.DropTable(
                name: "tbl_InspectionPlans");

            migrationBuilder.DropTable(
                name: "tbl_AssignementTypes");

            migrationBuilder.DropTable(
                name: "tbl_DecisionStatus");

            migrationBuilder.DropTable(
                name: "tbl_InspectionStatus");

            migrationBuilder.DropTable(
                name: "tbl_Years");

            migrationBuilder.DropTable(
                name: "tbl_InternalUsers");

            migrationBuilder.DropTable(
                name: "tbl_Teams");

            migrationBuilder.DropTable(
                name: "tbl_Department");
        }
    }
}
