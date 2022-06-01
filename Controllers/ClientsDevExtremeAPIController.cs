using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AmeriForce.Data;
using AmeriForce.Models.Clients.Reports;
using AmeriForce.Helpers;

namespace AmeriForce.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ClientsDevExtremeAPIController : Controller
    {
        private ApplicationDbContext _context;
        private UserHelper _userHelper;

        public ClientsDevExtremeAPIController(ApplicationDbContext context) {
            _context = context;
            _userHelper = new UserHelper(_context);
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var clients = _context.Clients.Select(i => new {
                i.Id,
                i.CompanyId,
                i.Name,
                i.Description,
                i.StageName,
                i.StageSortOrder,
                i.Amount,
                i.ExpectedRevenue,
                i.CloseDate,
                i.Type,
                i.LeadSource,
                i.IsClosed,
                i.ForecastCategory,
                i.ForecastCategoryName,
                i.OwnerId,
                i.CreatedDate,
                i.CreatedById,
                i.LastModifiedDate,
                i.LastModifiedById,
                i.SystemModstamp,
                i.LastActivityDate,
                i.LastStageChangeDate,
                i.FiscalYear,
                i.FiscalQuarter,
                i.ContactId,
                i.LastAmountChangedHistoryId,
                i.LastCloseDateChangedHistoryId,
                i.Legal_Docs_Issued,
                i.Legal_Docs_Received,
                i.Legal_Docs_Time_Out,
                i.External_Opportunity_ID,
                i.AD_Credit_Review,
                i.Delayed_Opportunity,
                i.All_Post_Funding_Tasks_Completed,
                i.Approval_Decision,
                i.Approval_Decision_Time_1,
                i.Bankruptcy_Search,
                i.BDO_Write_Up_Received,
                i.Client_Profile_Out,
                i.Client_Profile_In,
                i.Corporate_Verification,
                i.Criminal_Background_Search,
                i.D_B_Prospect,
                i.Due_Diligence_Deposit_Received,
                i.File_Received_by_UW,
                i.File_to_Client_Services,
                i.File_to_Mgmt_for_Review,
                i.First_Response,
                i.FTL_Search_State_County,
                i.Initial_Funding,
                i.Ins_Verification_Transp_Staff_only,
                i.Invoices_Rcd_Ready_to_Move_Fwd,
                i.Judgment_Search,
                i.Litigation_Search,
                i.Lost_to_Competitor,
                i.Mgmt_Completes_Post_Funding_File_Review,
                i.Pre_Approval_Biz_Hours,
                i.Reason_Lost,
                i.Referral_Date,
                i.Referring_Company,
                i.Referring_Contact,
                i.Searches_Requested,
                i.Searches_Returned,
                i.STL_Search,
                i.Tax_Guard_Report,
                i.Term_Sheet_Issued,
                i.Term_Sheet_Received,
                i.UCC_Search,
                i.UW_Hands_over_for_First_Review,
                i.NextActivityID
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(clients, loadOptions));
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Client();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Clients.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(string key, string values) {
            var model = await _context.Clients.FirstOrDefaultAsync(item => item.Id == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(string key) {
            var model = await _context.Clients.FirstOrDefaultAsync(item => item.Id == key);

            _context.Clients.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Client model, IDictionary values) {
            string ID = nameof(Client.Id);
            string COMPANY_ID = nameof(Client.CompanyId);
            string NAME = nameof(Client.Name);
            string DESCRIPTION = nameof(Client.Description);
            string STAGE_NAME = nameof(Client.StageName);
            string STAGE_SORT_ORDER = nameof(Client.StageSortOrder);
            string AMOUNT = nameof(Client.Amount);
            string EXPECTED_REVENUE = nameof(Client.ExpectedRevenue);
            string CLOSE_DATE = nameof(Client.CloseDate);
            string TYPE = nameof(Client.Type);
            string LEAD_SOURCE = nameof(Client.LeadSource);
            string IS_CLOSED = nameof(Client.IsClosed);
            string FORECAST_CATEGORY = nameof(Client.ForecastCategory);
            string FORECAST_CATEGORY_NAME = nameof(Client.ForecastCategoryName);
            string OWNER_ID = nameof(Client.OwnerId);
            string CREATED_DATE = nameof(Client.CreatedDate);
            string CREATED_BY_ID = nameof(Client.CreatedById);
            string LAST_MODIFIED_DATE = nameof(Client.LastModifiedDate);
            string LAST_MODIFIED_BY_ID = nameof(Client.LastModifiedById);
            string SYSTEM_MODSTAMP = nameof(Client.SystemModstamp);
            string LAST_ACTIVITY_DATE = nameof(Client.LastActivityDate);
            string LAST_STAGE_CHANGE_DATE = nameof(Client.LastStageChangeDate);
            string FISCAL_YEAR = nameof(Client.FiscalYear);
            string FISCAL_QUARTER = nameof(Client.FiscalQuarter);
            string CONTACT_ID = nameof(Client.ContactId);
            string LAST_AMOUNT_CHANGED_HISTORY_ID = nameof(Client.LastAmountChangedHistoryId);
            string LAST_CLOSE_DATE_CHANGED_HISTORY_ID = nameof(Client.LastCloseDateChangedHistoryId);
            string LEGAL_DOCS_ISSUED = nameof(Client.Legal_Docs_Issued);
            string LEGAL_DOCS_RECEIVED = nameof(Client.Legal_Docs_Received);
            string LEGAL_DOCS_TIME_OUT = nameof(Client.Legal_Docs_Time_Out);
            string EXTERNAL_OPPORTUNITY_ID = nameof(Client.External_Opportunity_ID);
            string AD_CREDIT_REVIEW = nameof(Client.AD_Credit_Review);
            string DELAYED_OPPORTUNITY = nameof(Client.Delayed_Opportunity);
            string ALL_POST_FUNDING_TASKS_COMPLETED = nameof(Client.All_Post_Funding_Tasks_Completed);
            string APPROVAL_DECISION = nameof(Client.Approval_Decision);
            string APPROVAL_DECISION_TIME_1 = nameof(Client.Approval_Decision_Time_1);
            string BANKRUPTCY_SEARCH = nameof(Client.Bankruptcy_Search);
            string BDO_WRITE_UP_RECEIVED = nameof(Client.BDO_Write_Up_Received);
            string CLIENT_PROFILE_OUT = nameof(Client.Client_Profile_Out);
            string CLIENT_PROFILE_IN = nameof(Client.Client_Profile_In);
            string CORPORATE_VERIFICATION = nameof(Client.Corporate_Verification);
            string CRIMINAL_BACKGROUND_SEARCH = nameof(Client.Criminal_Background_Search);
            string D_B_PROSPECT = nameof(Client.D_B_Prospect);
            string DUE_DILIGENCE_DEPOSIT_RECEIVED = nameof(Client.Due_Diligence_Deposit_Received);
            string FILE_RECEIVED_BY_UW = nameof(Client.File_Received_by_UW);
            string FILE_TO_CLIENT_SERVICES = nameof(Client.File_to_Client_Services);
            string FILE_TO_MGMT_FOR_REVIEW = nameof(Client.File_to_Mgmt_for_Review);
            string FIRST_RESPONSE = nameof(Client.First_Response);
            string FTL_SEARCH_STATE_COUNTY = nameof(Client.FTL_Search_State_County);
            string INITIAL_FUNDING = nameof(Client.Initial_Funding);
            string INS_VERIFICATION_TRANSP_STAFF_ONLY = nameof(Client.Ins_Verification_Transp_Staff_only);
            string INVOICES_RCD_READY_TO_MOVE_FWD = nameof(Client.Invoices_Rcd_Ready_to_Move_Fwd);
            string JUDGMENT_SEARCH = nameof(Client.Judgment_Search);
            string LITIGATION_SEARCH = nameof(Client.Litigation_Search);
            string LOST_TO_COMPETITOR = nameof(Client.Lost_to_Competitor);
            string MGMT_COMPLETES_POST_FUNDING_FILE_REVIEW = nameof(Client.Mgmt_Completes_Post_Funding_File_Review);
            string PRE_APPROVAL_BIZ_HOURS = nameof(Client.Pre_Approval_Biz_Hours);
            string REASON_LOST = nameof(Client.Reason_Lost);
            string REFERRAL_DATE = nameof(Client.Referral_Date);
            string REFERRING_COMPANY = nameof(Client.Referring_Company);
            string REFERRING_CONTACT = nameof(Client.Referring_Contact);
            string SEARCHES_REQUESTED = nameof(Client.Searches_Requested);
            string SEARCHES_RETURNED = nameof(Client.Searches_Returned);
            string STL_SEARCH = nameof(Client.STL_Search);
            string TAX_GUARD_REPORT = nameof(Client.Tax_Guard_Report);
            string TERM_SHEET_ISSUED = nameof(Client.Term_Sheet_Issued);
            string TERM_SHEET_RECEIVED = nameof(Client.Term_Sheet_Received);
            string UCC_SEARCH = nameof(Client.UCC_Search);
            string UW_HANDS_OVER_FOR_FIRST_REVIEW = nameof(Client.UW_Hands_over_for_First_Review);
            string NEXT_ACTIVITY_ID = nameof(Client.NextActivityID);

            if(values.Contains(ID)) {
                model.Id = Convert.ToString(values[ID]);
            }

            if(values.Contains(COMPANY_ID)) {
                model.CompanyId = Convert.ToString(values[COMPANY_ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(STAGE_NAME)) {
                model.StageName = Convert.ToString(values[STAGE_NAME]);
            }

            if(values.Contains(STAGE_SORT_ORDER)) {
                model.StageSortOrder = values[STAGE_SORT_ORDER] != null ? Convert.ToInt32(values[STAGE_SORT_ORDER]) : (int?)null;
            }

            if(values.Contains(AMOUNT)) {
                model.Amount = values[AMOUNT] != null ? Convert.ToInt32(values[AMOUNT]) : (int?)null;
            }

            if(values.Contains(EXPECTED_REVENUE)) {
                model.ExpectedRevenue = values[EXPECTED_REVENUE] != null ? Convert.ToInt32(values[EXPECTED_REVENUE]) : (int?)null;
            }

            if(values.Contains(CLOSE_DATE)) {
                model.CloseDate = values[CLOSE_DATE] != null ? Convert.ToDateTime(values[CLOSE_DATE]) : (DateTime?)null;
            }

            if(values.Contains(TYPE)) {
                model.Type = Convert.ToString(values[TYPE]);
            }

            if(values.Contains(LEAD_SOURCE)) {
                model.LeadSource = Convert.ToString(values[LEAD_SOURCE]);
            }

            if(values.Contains(IS_CLOSED)) {
                model.IsClosed = values[IS_CLOSED] != null ? Convert.ToInt32(values[IS_CLOSED]) : (int?)null;
            }

            if(values.Contains(FORECAST_CATEGORY)) {
                model.ForecastCategory = Convert.ToString(values[FORECAST_CATEGORY]);
            }

            if(values.Contains(FORECAST_CATEGORY_NAME)) {
                model.ForecastCategoryName = Convert.ToString(values[FORECAST_CATEGORY_NAME]);
            }

            if(values.Contains(OWNER_ID)) {
                model.OwnerId = Convert.ToString(values[OWNER_ID]);
            }

            if(values.Contains(CREATED_DATE)) {
                model.CreatedDate = values[CREATED_DATE] != null ? Convert.ToDateTime(values[CREATED_DATE]) : (DateTime?)null;
            }

            if(values.Contains(CREATED_BY_ID)) {
                model.CreatedById = Convert.ToString(values[CREATED_BY_ID]);
            }

            if(values.Contains(LAST_MODIFIED_DATE)) {
                model.LastModifiedDate = values[LAST_MODIFIED_DATE] != null ? Convert.ToDateTime(values[LAST_MODIFIED_DATE]) : (DateTime?)null;
            }

            if(values.Contains(LAST_MODIFIED_BY_ID)) {
                model.LastModifiedById = Convert.ToString(values[LAST_MODIFIED_BY_ID]);
            }

            if(values.Contains(SYSTEM_MODSTAMP)) {
                model.SystemModstamp = values[SYSTEM_MODSTAMP] != null ? Convert.ToDateTime(values[SYSTEM_MODSTAMP]) : (DateTime?)null;
            }

            if(values.Contains(LAST_ACTIVITY_DATE)) {
                model.LastActivityDate = values[LAST_ACTIVITY_DATE] != null ? Convert.ToDateTime(values[LAST_ACTIVITY_DATE]) : (DateTime?)null;
            }

            if(values.Contains(LAST_STAGE_CHANGE_DATE)) {
                model.LastStageChangeDate = values[LAST_STAGE_CHANGE_DATE] != null ? Convert.ToDateTime(values[LAST_STAGE_CHANGE_DATE]) : (DateTime?)null;
            }

            if(values.Contains(FISCAL_YEAR)) {
                model.FiscalYear = values[FISCAL_YEAR] != null ? Convert.ToInt32(values[FISCAL_YEAR]) : (int?)null;
            }

            if(values.Contains(FISCAL_QUARTER)) {
                model.FiscalQuarter = values[FISCAL_QUARTER] != null ? Convert.ToInt32(values[FISCAL_QUARTER]) : (int?)null;
            }

            if(values.Contains(CONTACT_ID)) {
                model.ContactId = Convert.ToString(values[CONTACT_ID]);
            }

            if(values.Contains(LAST_AMOUNT_CHANGED_HISTORY_ID)) {
                model.LastAmountChangedHistoryId = Convert.ToString(values[LAST_AMOUNT_CHANGED_HISTORY_ID]);
            }

            if(values.Contains(LAST_CLOSE_DATE_CHANGED_HISTORY_ID)) {
                model.LastCloseDateChangedHistoryId = Convert.ToString(values[LAST_CLOSE_DATE_CHANGED_HISTORY_ID]);
            }

            if(values.Contains(LEGAL_DOCS_ISSUED)) {
                model.Legal_Docs_Issued = values[LEGAL_DOCS_ISSUED] != null ? Convert.ToDateTime(values[LEGAL_DOCS_ISSUED]) : (DateTime?)null;
            }

            if(values.Contains(LEGAL_DOCS_RECEIVED)) {
                model.Legal_Docs_Received = values[LEGAL_DOCS_RECEIVED] != null ? Convert.ToDateTime(values[LEGAL_DOCS_RECEIVED]) : (DateTime?)null;
            }

            if(values.Contains(LEGAL_DOCS_TIME_OUT)) {
                model.Legal_Docs_Time_Out = values[LEGAL_DOCS_TIME_OUT] != null ? Convert.ToDateTime(values[LEGAL_DOCS_TIME_OUT]) : (DateTime?)null;
            }

            if(values.Contains(EXTERNAL_OPPORTUNITY_ID)) {
                model.External_Opportunity_ID = Convert.ToString(values[EXTERNAL_OPPORTUNITY_ID]);
            }

            if(values.Contains(AD_CREDIT_REVIEW)) {
                model.AD_Credit_Review = values[AD_CREDIT_REVIEW] != null ? Convert.ToDateTime(values[AD_CREDIT_REVIEW]) : (DateTime?)null;
            }

            if(values.Contains(DELAYED_OPPORTUNITY)) {
                model.Delayed_Opportunity = values[DELAYED_OPPORTUNITY] != null ? Convert.ToInt32(values[DELAYED_OPPORTUNITY]) : (int?)null;
            }

            if(values.Contains(ALL_POST_FUNDING_TASKS_COMPLETED)) {
                model.All_Post_Funding_Tasks_Completed = values[ALL_POST_FUNDING_TASKS_COMPLETED] != null ? Convert.ToDateTime(values[ALL_POST_FUNDING_TASKS_COMPLETED]) : (DateTime?)null;
            }

            if(values.Contains(APPROVAL_DECISION)) {
                model.Approval_Decision = Convert.ToString(values[APPROVAL_DECISION]);
            }

            if(values.Contains(APPROVAL_DECISION_TIME_1)) {
                model.Approval_Decision_Time_1 = values[APPROVAL_DECISION_TIME_1] != null ? Convert.ToInt32(values[APPROVAL_DECISION_TIME_1]) : (int?)null;
            }

            if(values.Contains(BANKRUPTCY_SEARCH)) {
                model.Bankruptcy_Search = values[BANKRUPTCY_SEARCH] != null ? Convert.ToDateTime(values[BANKRUPTCY_SEARCH]) : (DateTime?)null;
            }

            if(values.Contains(BDO_WRITE_UP_RECEIVED)) {
                model.BDO_Write_Up_Received = values[BDO_WRITE_UP_RECEIVED] != null ? Convert.ToDateTime(values[BDO_WRITE_UP_RECEIVED]) : (DateTime?)null;
            }

            if(values.Contains(CLIENT_PROFILE_OUT)) {
                model.Client_Profile_Out = values[CLIENT_PROFILE_OUT] != null ? Convert.ToDateTime(values[CLIENT_PROFILE_OUT]) : (DateTime?)null;
            }

            if(values.Contains(CLIENT_PROFILE_IN)) {
                model.Client_Profile_In = values[CLIENT_PROFILE_IN] != null ? Convert.ToDateTime(values[CLIENT_PROFILE_IN]) : (DateTime?)null;
            }

            if(values.Contains(CORPORATE_VERIFICATION)) {
                model.Corporate_Verification = values[CORPORATE_VERIFICATION] != null ? Convert.ToDateTime(values[CORPORATE_VERIFICATION]) : (DateTime?)null;
            }

            if(values.Contains(CRIMINAL_BACKGROUND_SEARCH)) {
                model.Criminal_Background_Search = values[CRIMINAL_BACKGROUND_SEARCH] != null ? Convert.ToDateTime(values[CRIMINAL_BACKGROUND_SEARCH]) : (DateTime?)null;
            }

            if(values.Contains(D_B_PROSPECT)) {
                model.D_B_Prospect = values[D_B_PROSPECT] != null ? Convert.ToDateTime(values[D_B_PROSPECT]) : (DateTime?)null;
            }

            if(values.Contains(DUE_DILIGENCE_DEPOSIT_RECEIVED)) {
                model.Due_Diligence_Deposit_Received = values[DUE_DILIGENCE_DEPOSIT_RECEIVED] != null ? Convert.ToDateTime(values[DUE_DILIGENCE_DEPOSIT_RECEIVED]) : (DateTime?)null;
            }

            if(values.Contains(FILE_RECEIVED_BY_UW)) {
                model.File_Received_by_UW = values[FILE_RECEIVED_BY_UW] != null ? Convert.ToDateTime(values[FILE_RECEIVED_BY_UW]) : (DateTime?)null;
            }

            if(values.Contains(FILE_TO_CLIENT_SERVICES)) {
                model.File_to_Client_Services = values[FILE_TO_CLIENT_SERVICES] != null ? Convert.ToDateTime(values[FILE_TO_CLIENT_SERVICES]) : (DateTime?)null;
            }

            if(values.Contains(FILE_TO_MGMT_FOR_REVIEW)) {
                model.File_to_Mgmt_for_Review = values[FILE_TO_MGMT_FOR_REVIEW] != null ? Convert.ToDateTime(values[FILE_TO_MGMT_FOR_REVIEW]) : (DateTime?)null;
            }

            if(values.Contains(FIRST_RESPONSE)) {
                model.First_Response = values[FIRST_RESPONSE] != null ? Convert.ToDateTime(values[FIRST_RESPONSE]) : (DateTime?)null;
            }

            if(values.Contains(FTL_SEARCH_STATE_COUNTY)) {
                model.FTL_Search_State_County = values[FTL_SEARCH_STATE_COUNTY] != null ? Convert.ToDateTime(values[FTL_SEARCH_STATE_COUNTY]) : (DateTime?)null;
            }

            if(values.Contains(INITIAL_FUNDING)) {
                model.Initial_Funding = values[INITIAL_FUNDING] != null ? Convert.ToDateTime(values[INITIAL_FUNDING]) : (DateTime?)null;
            }

            if(values.Contains(INS_VERIFICATION_TRANSP_STAFF_ONLY)) {
                model.Ins_Verification_Transp_Staff_only = values[INS_VERIFICATION_TRANSP_STAFF_ONLY] != null ? Convert.ToDateTime(values[INS_VERIFICATION_TRANSP_STAFF_ONLY]) : (DateTime?)null;
            }

            if(values.Contains(INVOICES_RCD_READY_TO_MOVE_FWD)) {
                model.Invoices_Rcd_Ready_to_Move_Fwd = values[INVOICES_RCD_READY_TO_MOVE_FWD] != null ? Convert.ToDateTime(values[INVOICES_RCD_READY_TO_MOVE_FWD]) : (DateTime?)null;
            }

            if(values.Contains(JUDGMENT_SEARCH)) {
                model.Judgment_Search = values[JUDGMENT_SEARCH] != null ? Convert.ToDateTime(values[JUDGMENT_SEARCH]) : (DateTime?)null;
            }

            if(values.Contains(LITIGATION_SEARCH)) {
                model.Litigation_Search = values[LITIGATION_SEARCH] != null ? Convert.ToDateTime(values[LITIGATION_SEARCH]) : (DateTime?)null;
            }

            if(values.Contains(LOST_TO_COMPETITOR)) {
                model.Lost_to_Competitor = Convert.ToString(values[LOST_TO_COMPETITOR]);
            }

            if(values.Contains(MGMT_COMPLETES_POST_FUNDING_FILE_REVIEW)) {
                model.Mgmt_Completes_Post_Funding_File_Review = values[MGMT_COMPLETES_POST_FUNDING_FILE_REVIEW] != null ? Convert.ToDateTime(values[MGMT_COMPLETES_POST_FUNDING_FILE_REVIEW]) : (DateTime?)null;
            }

            if(values.Contains(PRE_APPROVAL_BIZ_HOURS)) {
                model.Pre_Approval_Biz_Hours = Convert.ToDecimal(values[PRE_APPROVAL_BIZ_HOURS], CultureInfo.InvariantCulture);
            }

            if(values.Contains(REASON_LOST)) {
                model.Reason_Lost = Convert.ToString(values[REASON_LOST]);
            }

            if(values.Contains(REFERRAL_DATE)) {
                model.Referral_Date = values[REFERRAL_DATE] != null ? Convert.ToDateTime(values[REFERRAL_DATE]) : (DateTime?)null;
            }

            if(values.Contains(REFERRING_COMPANY)) {
                model.Referring_Company = Convert.ToString(values[REFERRING_COMPANY]);
            }

            if(values.Contains(REFERRING_CONTACT)) {
                model.Referring_Contact = Convert.ToString(values[REFERRING_CONTACT]);
            }

            if(values.Contains(SEARCHES_REQUESTED)) {
                model.Searches_Requested = values[SEARCHES_REQUESTED] != null ? Convert.ToDateTime(values[SEARCHES_REQUESTED]) : (DateTime?)null;
            }

            if(values.Contains(SEARCHES_RETURNED)) {
                model.Searches_Returned = values[SEARCHES_RETURNED] != null ? Convert.ToDateTime(values[SEARCHES_RETURNED]) : (DateTime?)null;
            }

            if(values.Contains(STL_SEARCH)) {
                model.STL_Search = values[STL_SEARCH] != null ? Convert.ToDateTime(values[STL_SEARCH]) : (DateTime?)null;
            }

            if(values.Contains(TAX_GUARD_REPORT)) {
                model.Tax_Guard_Report = values[TAX_GUARD_REPORT] != null ? Convert.ToDateTime(values[TAX_GUARD_REPORT]) : (DateTime?)null;
            }

            if(values.Contains(TERM_SHEET_ISSUED)) {
                model.Term_Sheet_Issued = values[TERM_SHEET_ISSUED] != null ? Convert.ToDateTime(values[TERM_SHEET_ISSUED]) : (DateTime?)null;
            }

            if(values.Contains(TERM_SHEET_RECEIVED)) {
                model.Term_Sheet_Received = values[TERM_SHEET_RECEIVED] != null ? Convert.ToDateTime(values[TERM_SHEET_RECEIVED]) : (DateTime?)null;
            }

            if(values.Contains(UCC_SEARCH)) {
                model.UCC_Search = values[UCC_SEARCH] != null ? Convert.ToDateTime(values[UCC_SEARCH]) : (DateTime?)null;
            }

            if(values.Contains(UW_HANDS_OVER_FOR_FIRST_REVIEW)) {
                model.UW_Hands_over_for_First_Review = values[UW_HANDS_OVER_FOR_FIRST_REVIEW] != null ? Convert.ToDateTime(values[UW_HANDS_OVER_FOR_FIRST_REVIEW]) : (DateTime?)null;
            }

            if(values.Contains(NEXT_ACTIVITY_ID)) {
                model.NextActivityID = Convert.ToString(values[NEXT_ACTIVITY_ID]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }


        #region Report APIs


        [HttpGet]
        public async Task<IActionResult> GetPipeline(DataSourceLoadOptions loadOptions)
        {

            var pipelineClients = new List<CurrentPipelineViewModel>();  

            var clients = _context.Clients.Where(c => c.StageName != "80-Hold" && c.StageName != "90-Lost" && c.StageName != "95-Declined By ASF" && c.StageName != "50-Funded" && c.StageName != "").ToList();
            foreach (var client in clients)
            {
                var pipelineClient = new CurrentPipelineViewModel
                {
                    Id = client.Id,
                    Name = (!String.IsNullOrEmpty(client.Name)) ? client.Name : "TBD",
                    Type = client.Type,
                    Amount = client.Amount,
                    Stage = client.StageName,
                    CreatedDate = client.CreatedDate,
                    CloseDate = client.CloseDate,
                    OwnerName = _userHelper.GetNameFromID(client.OwnerId),
                    Description = client.Description,
                    DaysSinceSubmission = (DateTime.Now - Convert.ToDateTime(client.CreatedDate)).Days.ToString(),
                    DaysSinceTSIssued = (client.Term_Sheet_Issued != null && client.Term_Sheet_Issued != Convert.ToDateTime("01/01/1900")) ? (DateTime.Now - Convert.ToDateTime(client.Term_Sheet_Issued)).Days.ToString() : "---",
                    DaysSinceTSReceived = (client.Term_Sheet_Received != null && client.Term_Sheet_Received != Convert.ToDateTime("01/01/1900")) ? (DateTime.Now - Convert.ToDateTime(client.Term_Sheet_Received)).Days.ToString() : "---",
                };
                pipelineClients.Add(pipelineClient);
            }

            var pipelineClientsQueryable = pipelineClients.Select(c => new
            {
                c.Name,
                c.OwnerName,
                c.CreatedDate
            }).AsQueryable();

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(DataSourceLoader.Load(pipelineClients, loadOptions));
        }


        [HttpGet]
        public async Task<IActionResult> GetTermSheetsPending(DataSourceLoadOptions loadOptions)
        {

            var pipelineClients = new List<CurrentPipelineViewModel>();

            //var clients = _context.Clients.Where(c => c.StageName == "20 - Pending TS, Dep, or Exp Docs").ToList();
            //c.StageName != "80-Hold" && c.StageName != "90-Lost" && c.StageName != "95-Declined By ASF" && c.StageName != "50-Funded" && c.StageName != "").ToList();


            var clients = from cl in _context.Clients
                           join com in _context.Companies on cl.CompanyId equals com.ID
                          where cl.StageName == "20 - Pending TS, Dep, or Exp Docs"
                          select new { cl, com };

            foreach (var client in clients)
            {
                var pipelineClient = new CurrentPipelineViewModel
                {
                    Name = (!String.IsNullOrEmpty(client.cl.Name)) ? client.cl.Name : "TBD",
                    Amount = client.cl.Amount,
                    OwnerName = _userHelper.GetNameFromID(client.cl.OwnerId),
                    Type = client.cl.Type,
                    LeadSource = client.cl.LeadSource,
                    CloseDate = client.cl.CloseDate,
                    Stage = client.cl.StageName,
                    FiscalPeriod = $"{client.cl.FiscalQuarter.ToString()} - {client.cl.FiscalYear.ToString()}",
                    Age = (DateTime.Now - Convert.ToDateTime(client.cl.CreatedDate)).Days.ToString(),
                    CreatedDate = client.cl.CreatedDate,
                    CompanyName = client.com.Name,
                    Id = client.cl.Id
                    
                    
                    //Description = client.cl.Description,
                    //DaysSinceSubmission = (DateTime.Now - Convert.ToDateTime(client.CreatedDate)).Days.ToString(),
                    //DaysSinceTSIssued = (client.Term_Sheet_Issued != null && client.Term_Sheet_Issued != Convert.ToDateTime("01/01/1900")) ? (DateTime.Now - Convert.ToDateTime(client.Term_Sheet_Issued)).Days.ToString() : "---",
                    //DaysSinceTSReceived = (client.Term_Sheet_Received != null && client.Term_Sheet_Received != Convert.ToDateTime("01/01/1900")) ? (DateTime.Now - Convert.ToDateTime(client.Term_Sheet_Received)).Days.ToString() : "---",
                    
                };
                pipelineClients.Add(pipelineClient);
            }

            var pipelineClientsQueryable = pipelineClients.Select(c => new
            {
                c.Name,
                c.OwnerName,
                c.CreatedDate
            }).AsQueryable();

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(DataSourceLoader.Load(pipelineClients, loadOptions));
        }



        #endregion

    }
}