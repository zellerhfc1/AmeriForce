using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmeriForce.Data;
using AmeriForce.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AmeriForce.Models.Email;
using Microsoft.Extensions.Configuration;
using AmeriForce.Models.Clients;

namespace AmeriForce.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private GuidHelper _guidHelper = new GuidHelper();
        private CompanyHelper _companyHelper;
        private LOVHelper _lovHelper;
        private UserHelper _userHelper;
        private UploadHelper _uploadHelper;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailConfigurationViewModel _emailConfig = new EmailConfigurationViewModel();

        public ClientsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            configuration.GetSection("EmailConfiguration").Bind(_emailConfig);

            _lovHelper = new LOVHelper(_context);
            _companyHelper = new CompanyHelper(_context);
            _userHelper = new UserHelper(_context);
            _uploadHelper = new UploadHelper(_context, _webHostEnvironment);
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            var clientCreateModel = new ClientCreateViewModel();

            clientCreateModel.BaseList = _lovHelper.GetBaseList();
            clientCreateModel.CompanyList = _lovHelper.GetCompanyDropdownList();
            clientCreateModel.OwnerList = _lovHelper.GetOwnerDropdown();
            clientCreateModel.ClientStageList = _lovHelper.GetClientStages();
            clientCreateModel.BDOList = _lovHelper.GetBDOList();
            clientCreateModel.StateList = _lovHelper.GetStateList();
            clientCreateModel.TaskList = _lovHelper.GetTaskTypes();
            clientCreateModel.ActiveUserList = _lovHelper.GetActiveUsers();
            clientCreateModel.ReferralTypeList = _lovHelper.GetReferralTypes();

            return View(clientCreateModel);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,Name,Description,StageName,StageSortOrder,Amount,ExpectedRevenue,CloseDate,Type,LeadSource,IsClosed,ForecastCategory,ForecastCategoryName,OwnerId,CreatedDate,CreatedById,LastModifiedDate,LastModifiedById,SystemModstamp,LastActivityDate,LastStageChangeDate,FiscalYear,FiscalQuarter,ContactId,LastAmountChangedHistoryId,LastCloseDateChangedHistoryId,Legal_Docs_Issued,Legal_Docs_Received,Legal_Docs_Time_Out,External_Opportunity_ID,AD_Credit_Review,Delayed_Opportunity,All_Post_Funding_Tasks_Completed,Approval_Decision,Approval_Decision_Time_1,Bankruptcy_Search,BDO_Write_Up_Received,Client_Profile_Out,Client_Profile_In,Corporate_Verification,Criminal_Background_Search,D_B_Prospect,Due_Diligence_Deposit_Received,File_Received_by_UW,File_to_Client_Services,File_to_Mgmt_for_Review,First_Response,FTL_Search_State_County,Initial_Funding,Ins_Verification_Transp_Staff_only,Invoices_Rcd_Ready_to_Move_Fwd,Judgment_Search,Litigation_Search,Lost_to_Competitor,Mgmt_Completes_Post_Funding_File_Review,Pre_Approval_Biz_Hours,Reason_Lost,Referral_Date,Referring_Company,Referring_Contact,Searches_Requested,Searches_Returned,STL_Search,Tax_Guard_Report,Term_Sheet_Issued,Term_Sheet_Received,UCC_Search,UW_Hands_over_for_First_Review,NextActivityID")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,CompanyId,Name,Description,StageName,StageSortOrder,Amount,ExpectedRevenue,CloseDate,Type,LeadSource,IsClosed,ForecastCategory,ForecastCategoryName,OwnerId,CreatedDate,CreatedById,LastModifiedDate,LastModifiedById,SystemModstamp,LastActivityDate,LastStageChangeDate,FiscalYear,FiscalQuarter,ContactId,LastAmountChangedHistoryId,LastCloseDateChangedHistoryId,Legal_Docs_Issued,Legal_Docs_Received,Legal_Docs_Time_Out,External_Opportunity_ID,AD_Credit_Review,Delayed_Opportunity,All_Post_Funding_Tasks_Completed,Approval_Decision,Approval_Decision_Time_1,Bankruptcy_Search,BDO_Write_Up_Received,Client_Profile_Out,Client_Profile_In,Corporate_Verification,Criminal_Background_Search,D_B_Prospect,Due_Diligence_Deposit_Received,File_Received_by_UW,File_to_Client_Services,File_to_Mgmt_for_Review,First_Response,FTL_Search_State_County,Initial_Funding,Ins_Verification_Transp_Staff_only,Invoices_Rcd_Ready_to_Move_Fwd,Judgment_Search,Litigation_Search,Lost_to_Competitor,Mgmt_Completes_Post_Funding_File_Review,Pre_Approval_Biz_Hours,Reason_Lost,Referral_Date,Referring_Company,Referring_Contact,Searches_Requested,Searches_Returned,STL_Search,Tax_Guard_Report,Term_Sheet_Issued,Term_Sheet_Received,UCC_Search,UW_Hands_over_for_First_Review,NextActivityID")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
