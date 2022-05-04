using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AmeriForce.Data.Migrations
{
    public partial class ImportFromSalesforceReplacementAProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientContactRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientID = table.Column<string>(nullable: true),
                    ContactId = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedById = table.Column<string>(nullable: true),
                    SystemModstamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientContactRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CompanyId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StageName = table.Column<string>(nullable: true),
                    StageSortOrder = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: true),
                    ExpectedRevenue = table.Column<int>(nullable: true),
                    CloseDate = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    LeadSource = table.Column<string>(nullable: true),
                    IsClosed = table.Column<int>(nullable: true),
                    ForecastCategory = table.Column<string>(nullable: true),
                    ForecastCategoryName = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<string>(nullable: true),
                    SystemModstamp = table.Column<DateTime>(nullable: true),
                    LastActivityDate = table.Column<DateTime>(nullable: true),
                    LastStageChangeDate = table.Column<DateTime>(nullable: true),
                    FiscalYear = table.Column<int>(nullable: true),
                    FiscalQuarter = table.Column<int>(nullable: true),
                    ContactId = table.Column<string>(nullable: true),
                    LastAmountChangedHistoryId = table.Column<string>(nullable: true),
                    LastCloseDateChangedHistoryId = table.Column<string>(nullable: true),
                    Legal_Docs_Issued = table.Column<DateTime>(nullable: true),
                    Legal_Docs_Received = table.Column<DateTime>(nullable: true),
                    Legal_Docs_Time_Out = table.Column<DateTime>(nullable: true),
                    External_Opportunity_ID = table.Column<string>(nullable: true),
                    AD_Credit_Review = table.Column<DateTime>(nullable: true),
                    Delayed_Opportunity = table.Column<int>(nullable: true),
                    All_Post_Funding_Tasks_Completed = table.Column<DateTime>(nullable: true),
                    Approval_Decision = table.Column<string>(nullable: true),
                    Approval_Decision_Time_1 = table.Column<int>(nullable: true),
                    Bankruptcy_Search = table.Column<DateTime>(nullable: true),
                    BDO_Write_Up_Received = table.Column<DateTime>(nullable: true),
                    Client_Profile_Out = table.Column<DateTime>(nullable: true),
                    Client_Profile_In = table.Column<DateTime>(nullable: true),
                    Corporate_Verification = table.Column<DateTime>(nullable: true),
                    Criminal_Background_Search = table.Column<DateTime>(nullable: true),
                    D_B_Prospect = table.Column<DateTime>(nullable: true),
                    Due_Diligence_Deposit_Received = table.Column<DateTime>(nullable: true),
                    File_Received_by_UW = table.Column<DateTime>(nullable: true),
                    File_to_Client_Services = table.Column<DateTime>(nullable: true),
                    File_to_Mgmt_for_Review = table.Column<DateTime>(nullable: true),
                    First_Response = table.Column<DateTime>(nullable: true),
                    FTL_Search_State_County = table.Column<DateTime>(nullable: true),
                    Initial_Funding = table.Column<DateTime>(nullable: true),
                    Ins_Verification_Transp_Staff_only = table.Column<DateTime>(nullable: true),
                    Invoices_Rcd_Ready_to_Move_Fwd = table.Column<DateTime>(nullable: true),
                    Judgment_Search = table.Column<DateTime>(nullable: true),
                    Litigation_Search = table.Column<DateTime>(nullable: true),
                    Lost_to_Competitor = table.Column<string>(nullable: true),
                    Mgmt_Completes_Post_Funding_File_Review = table.Column<DateTime>(nullable: true),
                    Pre_Approval_Biz_Hours = table.Column<int>(nullable: true),
                    Reason_Lost = table.Column<string>(nullable: true),
                    Referral_Date = table.Column<DateTime>(nullable: true),
                    Referring_Company = table.Column<string>(nullable: true),
                    Referring_Contact = table.Column<string>(nullable: true),
                    Searches_Requested = table.Column<DateTime>(nullable: true),
                    Searches_Returned = table.Column<DateTime>(nullable: true),
                    STL_Search = table.Column<DateTime>(nullable: true),
                    Tax_Guard_Report = table.Column<DateTime>(nullable: true),
                    Term_Sheet_Issued = table.Column<DateTime>(nullable: true),
                    Term_Sheet_Received = table.Column<DateTime>(nullable: true),
                    UCC_Search = table.Column<DateTime>(nullable: true),
                    UW_Hands_over_for_First_Review = table.Column<DateTime>(nullable: true),
                    NextActivityID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientStages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    DisplayValue = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SICCode = table.Column<string>(nullable: true),
                    CharterState = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    MailingAddress = table.Column<string>(nullable: true),
                    MailingCity = table.Column<string>(nullable: true),
                    MailingState = table.Column<string>(nullable: true),
                    MailingPostalCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    Salutation = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: true),
                    OtherStreet = table.Column<string>(nullable: true),
                    OtherCity = table.Column<string>(nullable: true),
                    OtherState = table.Column<string>(nullable: true),
                    OtherPostalCode = table.Column<string>(nullable: true),
                    OtherCountry = table.Column<string>(nullable: true),
                    MailingStreet = table.Column<string>(nullable: true),
                    MailingCity = table.Column<string>(nullable: true),
                    MailingState = table.Column<string>(nullable: true),
                    MailingPostalCode = table.Column<string>(nullable: true),
                    MailingCountry = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    HomePhone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    AssistantName = table.Column<string>(nullable: true),
                    Birthdate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    HasOptedOutOfEmail = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedById = table.Column<string>(nullable: true),
                    LastActivityDate = table.Column<DateTime>(nullable: true),
                    EmailBouncedReason = table.Column<string>(nullable: true),
                    EmailBouncedDate = table.Column<DateTime>(nullable: true),
                    Alt_Email = table.Column<string>(nullable: true),
                    Alt_Contact = table.Column<string>(nullable: true),
                    Children = table.Column<string>(nullable: true),
                    Direct_Line = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Initial_Meeting_Details = table.Column<string>(nullable: true),
                    LinkedIn_Profile = table.Column<string>(nullable: true),
                    Mailing_Lists = table.Column<string>(nullable: true),
                    Opt_Out = table.Column<string>(nullable: true),
                    Opt_Out_Date = table.Column<DateTime>(nullable: true),
                    Preferred_Name = table.Column<string>(nullable: true),
                    Reassigned_Date = table.Column<DateTime>(nullable: true),
                    Referral_Date = table.Column<DateTime>(nullable: true),
                    Referral_Partner_Agmnt_Date = table.Column<DateTime>(nullable: true),
                    Referral_Partner_Agmnt_Details = table.Column<string>(nullable: true),
                    Referring_Company = table.Column<string>(nullable: true),
                    Referring_Contact = table.Column<string>(nullable: true),
                    Relationship_Status = table.Column<string>(nullable: true),
                    Standard_Pay_Terms = table.Column<string>(nullable: true),
                    Rating_Sort = table.Column<string>(nullable: true),
                    Tax_ID = table.Column<string>(nullable: true),
                    Term_Of_Agreement = table.Column<string>(nullable: true),
                    Twitter_Profile = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Update_Needed = table.Column<string>(nullable: true),
                    Update_Needed_Date = table.Column<DateTime>(nullable: true),
                    Alt_Phone_3 = table.Column<string>(nullable: true),
                    OwnershipPercentage = table.Column<string>(nullable: true),
                    Guarantor = table.Column<bool>(nullable: true),
                    NextActivityID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CRMTasks",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    WhoId = table.Column<string>(nullable: true),
                    WhatId = table.Column<string>(nullable: true),
                    WhoCount = table.Column<int>(nullable: true),
                    WhatCount = table.Column<int>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    ActivityDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Priority = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    IsClosed = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<string>(nullable: true),
                    SystemModstamp = table.Column<DateTime>(nullable: true),
                    IsArchived = table.Column<int>(nullable: true),
                    EmailMessageId = table.Column<string>(nullable: true),
                    ActivityOriginType = table.Column<int>(nullable: true),
                    ReminderDateTime = table.Column<DateTime>(nullable: true),
                    IsReminderSet = table.Column<int>(nullable: true),
                    RecurrenceActivityId = table.Column<string>(nullable: true),
                    IsRecurrence = table.Column<int>(nullable: true),
                    RecurrenceStartDateOnly = table.Column<DateTime>(nullable: true),
                    RecurrenceEndDateOnly = table.Column<DateTime>(nullable: true),
                    RecurrenceTimeZoneSidKey = table.Column<string>(nullable: true),
                    RecurrenceType = table.Column<string>(nullable: true),
                    RecurrenceInterval = table.Column<int>(nullable: true),
                    RecurrenceDayOfWeekMask = table.Column<int>(nullable: true),
                    RecurrenceDayOfMonth = table.Column<int>(nullable: true),
                    RecurrenceInstance = table.Column<string>(nullable: true),
                    RecurrenceMonthOfYear = table.Column<string>(nullable: true),
                    CompletedDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRMTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<string>(nullable: true),
                    SystemModstamp = table.Column<DateTime>(nullable: true),
                    TextBody = table.Column<string>(nullable: true),
                    HtmlBody = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    FromName = table.Column<string>(nullable: true),
                    FromAddress = table.Column<string>(nullable: true),
                    ToAddress = table.Column<string>(nullable: true),
                    CcAddress = table.Column<string>(nullable: true),
                    BccAddress = table.Column<string>(nullable: true),
                    HasAttachment = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    MessageDate = table.Column<DateTime>(nullable: true),
                    MessageSize = table.Column<int>(nullable: true),
                    MessageIdentifier = table.Column<string>(nullable: true),
                    ThreadIdentifier = table.Column<string>(nullable: true),
                    IsTracked = table.Column<int>(nullable: true),
                    FirstOpenedDate = table.Column<DateTime>(nullable: true),
                    LastOpenedDate = table.Column<DateTime>(nullable: true),
                    RelatedTo = table.Column<string>(nullable: true),
                    TemporaryID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<string>(nullable: true),
                    FacilityName = table.Column<string>(nullable: true),
                    FacilityType = table.Column<string>(nullable: true),
                    FacilityCategory = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_ClientStatus",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientStatus = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_ClientStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_ReferralType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferralType = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_ReferralType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_State",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_State", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_TaskType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TaskType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_TemplateType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TemplateType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NewInitialDeals",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Salutation = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    Suffix = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    LeadSource = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Industry = table.Column<string>(nullable: true),
                    AnnualRevenue = table.Column<int>(nullable: true),
                    NumberOfEmployees = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    IsConverted = table.Column<int>(nullable: false),
                    ConvertedDate = table.Column<DateTime>(nullable: true),
                    ConvertedAccountId = table.Column<string>(nullable: true),
                    ConvertedContactId = table.Column<string>(nullable: true),
                    ConvertedOpportunityId = table.Column<string>(nullable: true),
                    IsUnreadByOwner = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<string>(nullable: true),
                    SystemModstamp = table.Column<DateTime>(nullable: true),
                    LastActivityDate = table.Column<DateTime>(nullable: true),
                    LastTransferDate = table.Column<DateTime>(nullable: true),
                    EmailBouncedReason = table.Column<string>(nullable: true),
                    EmailBouncedDate = table.Column<DateTime>(nullable: true),
                    Business_Description = table.Column<string>(nullable: true),
                    Charter_State = table.Column<string>(nullable: true),
                    Client_Profile_Out = table.Column<DateTime>(nullable: true),
                    Client_Profile_In = table.Column<DateTime>(nullable: true),
                    Direct_Line = table.Column<string>(nullable: true),
                    DUNS_Acct = table.Column<int>(nullable: true),
                    EXPERIAN_Acct = table.Column<int>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Initial_Meeting_Details = table.Column<string>(nullable: true),
                    Opt_Out = table.Column<string>(nullable: true),
                    Opt_Out_Date = table.Column<DateTime>(nullable: true),
                    Pre_Approval_Biz_Hours = table.Column<int>(nullable: true),
                    Preferred_Name = table.Column<string>(nullable: true),
                    Referral_DAte = table.Column<DateTime>(nullable: true),
                    Referring_Company = table.Column<string>(nullable: true),
                    Referring_Contact = table.Column<string>(nullable: true),
                    Relationship_Status = table.Column<string>(nullable: true),
                    SIC = table.Column<string>(nullable: true),
                    SIC_Description = table.Column<string>(nullable: true),
                    Update_Needed = table.Column<string>(nullable: true),
                    Update_Needed_Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewInitialDeals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SICCodes",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SICCodes", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientContactRoles");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ClientStages");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "CRMTasks");

            migrationBuilder.DropTable(
                name: "EmailMessages");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "LOV_ClientStatus");

            migrationBuilder.DropTable(
                name: "LOV_ReferralType");

            migrationBuilder.DropTable(
                name: "LOV_State");

            migrationBuilder.DropTable(
                name: "LOV_TaskType");

            migrationBuilder.DropTable(
                name: "LOV_TemplateType");

            migrationBuilder.DropTable(
                name: "NewInitialDeals");

            migrationBuilder.DropTable(
                name: "SICCodes");
        }
    }
}
