using AmeriForce.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AmeriForce.Helpers
{
    public class LOVHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LOVHelper(ApplicationDbContext context)
        {
            _context = context;
        }
        public LOVHelper(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<SelectListItem> GetBaseList()
        {
            // GET BASE LIST
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            return sList;

        }

        public IEnumerable<SelectListItem> GetCompanyDropdownList()
        {
            // GET COMPANIES LIST
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            //using (var db = new ApplicationDbContext())
            //{

            var companies = _context.Companies.OrderBy(l => l.Name).ToList();

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            foreach (var company in companies)
            {
                item = new SelectListItem();
                item.Text = company.Name;
                item.Value = company.ID;
                sList.Add(item);
            }
            return sList;
            //}
        }


        public IEnumerable<SelectListItem> GetOwnerDropdown()
        {
            // GET COMPANIES LIST
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var owners = _context.Users.Where(u => u.IsActive == true && (u.Department == "Underwriting" || u.Department == "Business Development")).OrderBy(l => l.FirstName);

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            foreach (var owner in owners)
            {
                item = new SelectListItem();
                item.Text = $"{owner.FirstName} {owner.LastName}";
                item.Value = owner.Id;
                sList.Add(item);
            }
            return sList;

        }


        public IEnumerable<SelectListItem> GetClientStages()
        {
            // GET CLIENT STAGES LIST
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var clientStages = _context.ClientStages.OrderBy(c => c.Name).ToList();

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            foreach (var clientStage in clientStages)
            {
                item = new SelectListItem();
                item.Text = $"{clientStage.Name}";
                item.Value = $"{clientStage.Name}";
                sList.Add(item);
            }
            return sList;
        }


        public IEnumerable<SelectListItem> GetContactListByCompany(string companyID)
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var contacts = _context.Contacts.Where(c => c.AccountId == companyID).ToList();

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            foreach (var contact in contacts)
            {
                item = new SelectListItem();
                item.Text = $"{contact.FirstName} {contact.LastName}";
                item.Value = $"{contact.Id}";
                sList.Add(item);
            }
            return sList;
        }


        public IEnumerable<SelectListItem> GetContactEmailListByContactId(string contactID)
        {
            // GET CONTACTS LIST BY ContactID
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var contacts = _context.Contacts.Where(c => c.Id == contactID).ToList();

            //var defaultItem = new SelectListItem();
            //defaultItem.Text = "";
            //defaultItem.Value = "";
            //sList.Add(defaultItem);

            foreach (var contact in contacts)
            {
                item = new SelectListItem();
                item.Text = $"{contact.FirstName} {contact.LastName}";
                item.Value = $"{contact.Id}";
                sList.Add(item);
            }
            return sList;
        }


        public IEnumerable<SelectListItem> GetBDOList()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var users = _context.Users.Where(c => (c.Department == "Business Development" || c.Department=="Underwriting") && c.IsActive == true).OrderBy(c => c.FirstName);

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var user in users)
                {
                    item = new SelectListItem();
                    item.Text = $"{user.FirstName} {user.LastName}";
                    item.Value = $"{user.Id}";
                    sList.Add(item);
                }
                return sList;
        }

        public IEnumerable<SelectListItem> GetBDOListWithInactive()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var users = _context.Users.Where(c => (c.Department == "Business Development" || c.Department == "Underwriting")).OrderBy(c => c.FirstName);

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            foreach (var user in users)
            {
                item = new SelectListItem();
                item.Text = $"{user.FirstName} {user.LastName}";
                item.Value = $"{user.Id}";
                sList.Add(item);
            }
            return sList;
        }


        public IEnumerable<SelectListItem> GetStateList()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            //using (var db = new ApplicationDbContext())
            //{

                var states = _context.LOV_State.OrderBy(s => s.Abbreviation);

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var state in states)
                {
                    item = new SelectListItem();
                    item.Text = $"{state.Name}";
                    item.Value = $"{state.Abbreviation}";
                    sList.Add(item);
                }
                return sList;
            //}
        }


        public IEnumerable<SelectListItem> GetTaskTypes()
        {
            // GET TASK TYPES
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var tasks = _context.LOV_TaskType.OrderBy(t => t.TaskType);

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var task in tasks)
                {
                    item = new SelectListItem();
                    item.Text = $"{task.TaskType}";
                    item.Value = $"{task.TaskType}";
                    sList.Add(item);
                }
                return sList;
        }


        public IEnumerable<SelectListItem> GetActiveUsers()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var users = _context.Users.Where(c => c.IsActive == true).OrderBy(c => c.FirstName);

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var user in users)
                {
                    item = new SelectListItem();
                    item.Text = $"{user.FirstName} {user.LastName}";
                    item.Value = $"{user.Id}";
                    sList.Add(item);
                }
                return sList;
            
        }


        public IEnumerable<SelectListItem> GetActiveUsersEmailList()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var users = _context.Users.Where(c => c.IsActive == true).OrderBy(c => c.FirstName);

                foreach (var user in users)
                {
                    item = new SelectListItem();
                    item.Text = $"{user.FirstName} {user.LastName}";
                    item.Value = $"{user.Email}";
                    sList.Add(item);
                }
                return sList;
        }


        public IEnumerable<SelectListItem> GetReferralTypes()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var referralTypes = _context.LOV_ReferralType.OrderBy(c => c.ReferralType);

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var referral in referralTypes)
                {
                    item = new SelectListItem();
                    item.Text = $"{referral.ReferralType}";
                    item.Value = $"{referral.ReferralType}";
                    sList.Add(item);
                }
                return sList;
        }


        public IEnumerable<SelectListItem> GetCompanyTypes()
        {
            // GET COMPANY REFERRAL TYPES
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var referralTypes = _context.LOV_CompanyTypes.OrderBy(c => c.ReferralType);

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            foreach (var referral in referralTypes)
            {
                item = new SelectListItem();
                item.Text = $"{referral.ReferralType}";
                item.Value = $"{referral.ReferralType}";
                sList.Add(item);
            }
            return sList;
        }


        //public IEnumerable<SelectListItem> GetClientStatuses()
        //{
        //    // GET CONTACTS LIST BY COMPANY
        //    SelectListItem item;
        //    List<SelectListItem> sList = new List<SelectListItem>();

        //    using (var db = new ApplicationDbContext())
        //    {

        //        var clientStatuses = db.LOV_ClientStatuses.OrderBy(c => c.ClientStatus);

        //        var defaultItem = new SelectListItem();
        //        defaultItem.Text = "";
        //        defaultItem.Value = "";
        //        sList.Add(defaultItem);

        //        foreach (var clientStatus in clientStatuses)
        //        {
        //            item = new SelectListItem();
        //            item.Text = $"{clientStatus.ClientStatus}";
        //            item.Value = $"{clientStatus.ClientStatus}";
        //            sList.Add(item);
        //        }
        //        return sList;
        //    }
        //}


        public IEnumerable<SelectListItem> GetSICCodesList()
        {
            // GET SIC CODES
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            using (_context)
            {
                var sicCodes = _context.SICCodes.OrderBy(s => s.Value).ToList();

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var sicCode in sicCodes)
                {
                    item = new SelectListItem();
                    item.Text = $"{sicCode.Value}";
                    item.Value = $"{sicCode.Value}";
                    sList.Add(item);
                }
                return sList;
            }
        }


        public IEnumerable<SelectListItem> GetYesNoList()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            var yesItem = new SelectListItem();
            yesItem.Text = "Yes";
            yesItem.Value = "true";
            sList.Add(yesItem);

            var noItem = new SelectListItem();
            noItem.Text = "No";
            noItem.Value = "false";
            sList.Add(noItem);

            return sList;
        }


        public IEnumerable<SelectListItem> GetApprovalDecisionList()
        {
            // GET CONTACTS LIST BY COMPANY
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            var yesItem = new SelectListItem();
            yesItem.Text = "APPROVED";
            yesItem.Value = "APPROVED";
            sList.Add(yesItem);

            var noItem = new SelectListItem();
            noItem.Text = "DECLINED";
            noItem.Value = "DECLINED";
            sList.Add(noItem);

            return sList;
        }


        public IEnumerable<SelectListItem> GetMailMergeTemplateList()
        {
            // GET MailMerge Templates from DB
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();


                var mailMergeTemplates = _context.LOV_TemplateType.OrderBy(t => t.TemplateName).ToList();

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var mailMergeTemplate in mailMergeTemplates)
                {
                    item = new SelectListItem();
                    item.Text = $"{mailMergeTemplate.TemplateName}";
                    item.Value = $"{mailMergeTemplate.TemplateName}";
                    sList.Add(item);
                }
                return sList;
        }

        public IEnumerable<SelectListItem> GetMailMergeTemplateList(string category)
        {
            // GET MailMerge Templates from DB
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            //string webRootPath = _webHostEnvironment.WebRootPath;
            //var folderLocation = $"Templates/{category}/TestDoc.doc";
            //var fileLocation = $"Templates/{category}/{templateName}";
            //var folderPath = Path.Combine(webRootPath, folderLocation);
            //var filePath = Path.Combine(webRootPath, fileLocation);


            string webRootPath = _webHostEnvironment.WebRootPath;
            var fileLocation = $"Templates/{category}";

            var files = new DirectoryInfo(Path.Combine(webRootPath, fileLocation)).GetFiles().OrderBy(f => f.Name);

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);


            foreach (var f in files)
            {
                    item = new SelectListItem();
                    item.Text = $"{Path.GetFileName(f.ToString())}";
                    item.Value = $"{Path.GetFileName(f.ToString())}";
                    sList.Add(item);

                //var fileInfo = new FileInfo(f.ToString());
                //string scrubbedF = f.ToString().Replace("'", "\\'");

                //if (fileInfo.Extension.ToUpper().Contains("PDF"))
                //{
                //    fileTypeFontAwesome = "file-pdf";
                //    colorFontAwesome = "ff0000";
                //}

                //if (fileInfo.Extension.ToUpper().Contains("DOC"))
                //{
                //    fileTypeFontAwesome = "file-word";
                //    colorFontAwesome = "0078d7 ";
                //}

                //if (fileInfo.Extension.ToUpper().Contains("XLS") || fileInfo.Extension.ToUpper().Contains("CSV"))
                //{
                //    fileTypeFontAwesome = "file-excel";
                //    colorFontAwesome = "1D6F42";
                //}

                //if (fileInfo.Extension.ToUpper().Contains("PNG") || fileInfo.Extension.ToUpper().Contains("JPG") || fileInfo.Extension.ToUpper().Contains("JPEG")
                //            || fileInfo.Extension.ToUpper().Contains("GIF") || fileInfo.Extension.ToUpper().Contains("TIFF"))
                //{
                //    fileTypeFontAwesome = "file-image";
                //    colorFontAwesome = "999";
                //}

                ////returnString += "<a href='../../Images/DecisionLogicReports/" + Path.GetFileName(f) + "' target='_blank'><span class='glyphicon glyphicon-file' data-toggle='tooltip' title='" + Path.GetFileName(f) + "' style='color:#80a7d8;'></span></a>";
                ////returnString += String.Format("<a href='../../Uploads/CBR/{0}/{1}' target='_blank'><span class='glyphicon glyphicon-file' data-toggle='tooltip' title='{1}' style='color:#80a7d8;'></span></a><br>", id, Path.GetFileName(f));

                //returnString += $@"<tr><td style='width:5%;padding:3px;' class='leftTextAlign'>
                //                            <i class='fa fa-{fileTypeFontAwesome}' data-toggle='tooltip' title='{1}' style='color:#{colorFontAwesome};font-size:14px;padding:3px;'></i></td>
                //                                <td class='leftTextAlign' style='width:60%;padding:3px;'><a href='../../Documents/Contacts/{id}/{Path.GetFileName(f.ToString()).Replace("'", HttpUtility.UrlEncode("'"))}' target='_blank'><span style='font-size:12px;'>{Path.GetFileName(f.ToString())}</span>
                //                                </a></td>
                //                                <td style='width:35%;padding:3px;'>Uploaded: {fileInfo.LastWriteTime.ToString()}</td>
                //                                </tr>";

                ////fileInfo.LastWriteTime.ToString() + "</tr></table></div></div>", id, Path.GetFileName(f), "<b>" + Path.GetFileName(f) + "</b><br>Uploaded: " +
                ////fileInfo.LastWriteTime.ToString() + "<br>Last Opened: " + fileInfo.LastAccessTime.ToString(), fileTypeFontAwesome, colorFontAwesome);
            }

            return sList;
        }



        public IEnumerable<SelectListItem> GetRelationshipStatusList(string selectedOption = "")
        {
            // GET LIST OF POTENTIAL RELATIONSHIP STATUSES
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var relationshipStatues = _context.LOV_RelationshipStatuses.OrderBy(t => t.Status).ToList();

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var relationshipStatus in relationshipStatues)
                {
                    item = new SelectListItem();
                    item.Text = $"{relationshipStatus.Status}";
                    item.Value = $"{relationshipStatus.Status}";
                    if (item.Value == selectedOption)
                    {
                        item.Selected = true;
                    }
                    sList.Add(item);
                }
                return sList;
            
        }



        public IEnumerable<SelectListItem> GetTagGradeSortList()
        {
            // GET LIST OF TAG/GRADE/SORT STATUSES
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var tagGradeSorts = _context.LOV_TagGradeSorts.OrderBy(t => t.Grade).ToList();

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var tagGradeSort in tagGradeSorts)
                {
                    item = new SelectListItem();
                    item.Text = $"{tagGradeSort.Grade} - {tagGradeSort.Description}";
                    item.Value = $"{tagGradeSort.Grade}";
                    sList.Add(item);
                }
                return sList;
            
        }



        public IEnumerable<SelectListItem> GetUpdateNeededList()
        {
            // GET LIST OF UPDATE NEEDED ITEMS
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var updatesNeeded = _context.LOV_UpdateNeededs.OrderBy(t => t.UpdateField).ToList();

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var updateNeeded in updatesNeeded)
                {
                    item = new SelectListItem();
                    item.Text = $"{updateNeeded.UpdateField}";
                    item.Value = $"{updateNeeded.UpdateField}";
                    sList.Add(item);
                }
                return sList;
            
        }



        public IEnumerable<SelectListItem> GetMailingLists()
        {
            // GET LIST OF MAILING LISTS
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

                var mailingLists = _context.LOV_MailingLists.OrderBy(t => t.Name).ToList();

                var defaultItem = new SelectListItem();
                defaultItem.Text = "";
                defaultItem.Value = "";
                sList.Add(defaultItem);

                foreach (var mailingList in mailingLists)
                {
                    item = new SelectListItem();
                    item.Text = $"{mailingList.Name}";
                    item.Value = $"{mailingList.Name}";
                    sList.Add(item);
                }
                return sList;

        }


        public IEnumerable<SelectListItem> GetClientTypes()
        {
            // GET COMPANY REFERRAL TYPES
            SelectListItem item;
            List<SelectListItem> sList = new List<SelectListItem>();

            var clientTypes = _context.LOV_ClientTypes.OrderBy(c => c.Type);

            var defaultItem = new SelectListItem();
            defaultItem.Text = "";
            defaultItem.Value = "";
            sList.Add(defaultItem);

            foreach (var clientType in clientTypes)
            {
                item = new SelectListItem();
                item.Text = $"{clientType.Type}";
                item.Value = $"{clientType.Type}";
                sList.Add(item);
            }
            return sList;
        }



    }
}
