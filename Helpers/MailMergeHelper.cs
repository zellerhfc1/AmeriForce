using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Text;
using System.Reflection;
using System.IO;
using AmeriForce.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Packaging;
using Aspose.Words;
using System.Data;

namespace AmeriForce.Helpers
{

    public class MailMergeHelper
    {
        private readonly ApplicationDbContext _context;
        private string _entityID { get; set; }
        private string _entityType { get; set; }
        private string _documentName { get; set; }
        private string _contactName { get; set; }
        private UploadHelper _uploadHelper { get; set; }
        private IEnumerable<MergeEntry> _mergeEntries { get; set; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string webRootPath;

        public MailMergeHelper(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //public MailMergeHelper(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, string entityID, string entityType, string documentName, string contactName = null)
        //{
        //    _entityID = entityID;
        //    _entityType = entityType;
        //    _documentName = documentName;
        //    _contactName = contactName;
        //    _context = context;
        //    _webHostEnvironment = webHostEnvironment;
        //    webRootPath = _webHostEnvironment.WebRootPath;
        //}

        //public void WordDocumentMailMerge()
        //{
        //    Object oMissing = System.Reflection.Missing.Value;
        //    Object oTemplatePath = $"{ResolveApplicationDataPath()}";

        //    Application wordApp = new Application();
        //    Document wordDoc = new Document();

        //    wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
        //    var lookupHelper = new LookupHelper(_context);

        //    Client client;
        //    Contact contact;
        //    Company company;
        //    ApplicationUser owner;

        //        client = _context.Clients.Where(c => c.Id == _entityID).FirstOrDefault();
        //        var clientContactID = client.ContactId;

        //        if (_contactName == null)
        //        {
        //            contact = _context.Contacts.Where(c => c.Id == clientContactID).FirstOrDefault();
        //        }
        //        else
        //        {
        //            contact = _context.Contacts.Where(c => c.Id == _contactName).FirstOrDefault();
        //        }

        //        company = _context.Companies.Where(c => c.ID == client.CompanyId).FirstOrDefault();
        //        owner = _context.Users.Where(u => u.Id == client.OwnerId).FirstOrDefault();


        //    foreach (Field myMergeField in wordDoc.Fields)
        //    {
        //        Microsoft.Office.Interop.Word.Range rngFieldCode = myMergeField.Code;
        //        String fieldText = rngFieldCode.Text;

        //        // ONLY GETTING THE MAILMERGE FIELDS
        //        if (fieldText.StartsWith(" MERGEFIELD"))
        //        {
        //            // THE TEXT COMES IN THE FORMAT OF
        //            // MERGEFIELD  MyFieldName  \\* MERGEFORMAT
        //            // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"

        //            Int32 endMerge = fieldText.IndexOf("\\");
        //            Int32 fieldNameLength = fieldText.Length - endMerge;
        //            String fieldName = fieldText.Substring(11, endMerge - 11);

        //            // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .doc FILE
        //            fieldName = fieldName.Trim();

        //            // **** FIELD REPLACEMENT IMPLEMENTATION GOES HERE ****//
        //            // THE PROGRAMMER CAN HAVE HIS OWN IMPLEMENTATIONS HERE
        //            if (fieldName == "USER_FULLNAME") { myMergeField.Select(); wordApp.Selection.TypeText($"{owner.FirstName} {owner.LastName}"); }
        //            if (fieldName == "USER_STREET") { myMergeField.Select(); wordApp.Selection.TypeText("7225 Langtry St."); }// wordApp.Selection.TypeText(contact.MailingStreet); }
        //            if (fieldName == "USER_CITY") { myMergeField.Select(); wordApp.Selection.TypeText("Houston"); }
        //            if (fieldName == "USER_STATE") { myMergeField.Select(); wordApp.Selection.TypeText("TX"); }
        //            if (fieldName == "USER_POSTALCODE") { myMergeField.Select(); wordApp.Selection.TypeText("77040"); }
        //            if (fieldName == "USER_PHONE") { myMergeField.Select(); wordApp.Selection.TypeText("(713) 863-8300"); }
        //            if (fieldName == "USER_FAX") { myMergeField.Select(); wordApp.Selection.TypeText(" "); }

        //            if (contact != null)
        //            {
        //                if (fieldName == "CONTACT_FULLNAME") { myMergeField.Select(); wordApp.Selection.TypeText($"{contact.FirstName} {contact.LastName}"); }
        //                if (fieldName == "CONTACT_PHONE") { myMergeField.Select(); wordApp.Selection.TypeText(contact.Phone); }
        //                if (fieldName == "CONTACT_FAX") { myMergeField.Select(); wordApp.Selection.TypeText(contact.Fax); }
        //                if (fieldName == "CONTACT_TITLE") { myMergeField.Select(); wordApp.Selection.TypeText(contact.Title); }
        //                if (fieldName == "CONTACT_MAILINGSTREET") { myMergeField.Select(); wordApp.Selection.TypeText(contact.MailingStreet); }
        //                if (fieldName == "CONTACT_MAILINGCITY") { myMergeField.Select(); wordApp.Selection.TypeText(contact.MailingCity); }
        //                if (fieldName == "CONTACT_MAILINGSTATE") { myMergeField.Select(); wordApp.Selection.TypeText(contact.MailingState); }
        //                if (fieldName == "CONTACT_MAILINGPOSTALCODE") { myMergeField.Select(); wordApp.Selection.TypeText(contact.MailingPostalCode); }
        //                if (fieldName == "CONTACT_EMAIL") { myMergeField.Select(); wordApp.Selection.TypeText(contact.Email); }
        //                if (fieldName == "CONTACT_TAX_ID") { myMergeField.Select(); wordApp.Selection.TypeText(contact.Tax_ID); }
        //                if (fieldName == "CONTACT_FULLMAILINGADDRESS") { myMergeField.Select(); wordApp.Selection.TypeText($"{contact.MailingStreet} {contact.MailingCity}, {contact.MailingState}  {contact.MailingPostalCode}"); }
        //                if (fieldName == "CONTACT_FIRSTNAME") { myMergeField.Select(); wordApp.Selection.TypeText(contact.FirstName); }
        //                if (fieldName == "CONTACT_LASTNAME") { myMergeField.Select(); wordApp.Selection.TypeText(contact.LastName); }
        //                if (fieldName == "CONTACT_LASTNAME") { myMergeField.Select(); wordApp.Selection.TypeText(company.Name); }


        //                //CONTACT_EMAIL
        //                //CONTACT_TAX_ID
        //                //CONTACT_DIRECT_LINE
        //                //CONTACT_FULLMAILINGADDRESS
        //                //CONTACT_FIRSTNAME
        //                //CONTACT_LASTNAME
        //                //ACCOUNT_LEGAL_ENTITY_NAME           
        //            }
        //            if (fieldName == "OPPORTUNITY_LEGAL_ENTITY_NAME") { myMergeField.Select(); wordApp.Selection.TypeText(company.Name); }
        //            if (fieldName == "OPPORTUNITYOWNER_FULLNAME") { myMergeField.Select(); wordApp.Selection.TypeText(lookupHelper.GetUserNameFromID(client.OwnerId)); }
        //            if (fieldName == "OPPORTUNITYOWNER_TITLE") { myMergeField.Select(); wordApp.Selection.TypeText(owner.Title); }
        //            if (fieldName == "OPPORTUNITY_ACCOUNT") { myMergeField.Select(); wordApp.Selection.TypeText(client.CompanyId); }
        //            if (fieldName == "OPPORTUNITY_REFERRING_CONTACT") { myMergeField.Select(); wordApp.Selection.TypeText(client.Referring_Contact); }
        //            if (fieldName == "OPPORTUNITY_REFERRING_COMPANY") { myMergeField.Select(); wordApp.Selection.TypeText(client.Referring_Company); }

        //            if (fieldName == "ACCOUNT_BILLINGSTREET") { myMergeField.Select(); wordApp.Selection.TypeText(company.MailingAddress); }
        //            if (fieldName == "ACCOUNT_BILLINGCITY") { myMergeField.Select(); wordApp.Selection.TypeText(company.MailingCity); }
        //            if (fieldName == "ACCOUNT_BILLINGSTATE") { myMergeField.Select(); wordApp.Selection.TypeText(company.MailingState); }
        //            if (fieldName == "ACCOUNT_BILLINGPOSTALCODE") { myMergeField.Select(); wordApp.Selection.TypeText(company.MailingPostalCode); }
        //            if (fieldName == "ACCOUNT_NAME") { myMergeField.Select(); wordApp.Selection.TypeText(company.Name); }

        //            //OPPORTUNITY_LEGAL_ENTITY_NAME *
        //            //ACCOUNT_BILLINGSTREET  *
        //            //ACCOUNT_BILLINGCITY *
        //            //ACCOUNT_BILLINGSTATE ** 
        //            //ACCOUNT_BILLINGPOSTALCODE *
        //            //ACCOUNT_NAME *
        //            //USER_STREET
        //            //USER_CITY
        //            //USER_STATE
        //            //USER_POSTALCODE
        //            //USER_PHONE
        //            //USER_FAX
        //            //CONTACT_FULLNAME
        //            //CONTACT_PHONE
        //            //CONTACT_FAX
        //            //USER_FULLNAME
        //            //CONTACT_MAILINGSTREET
        //            //CONTACT_MAILINGCITY
        //            //CONTACT_MAILINGSTATE
        //            //CONTACT_MAILINGPOSTALCODE
        //            //CONTACT_TITLE
        //            //USER_FIRSTNAME
        //            //USER_LASTNAME
        //            //USER_EMAIL
        //            //OPPORTUNITY_ACCOUNT
        //            //OPPORTUNITY_REFERRING_CONTACT
        //            //OPPORTUNITY_REFERRING_COMPANY
        //            //OPPORTUNITYOWNER_FULLNAME
        //            //OPPORTUNITYOWNER_TITLE
        //            //CONTACTOWNER_FULLNAME
        //            //CONTACTOWNER_TITLE
        //            //CONTACT_EMAIL
        //            //CONTACT_TAX_ID
        //            //CONTACT_DIRECT_LINE
        //            //CONTACT_FULLMAILINGADDRESS
        //            //CONTACT_FIRSTNAME
        //            //CONTACT_LASTNAME
        //            //ACCOUNT_LEGAL_ENTITY_NAME
        //            //USER_TITLE

        //        }
        //    }

        //    string resultName = ResolveResultDataPath("/");
        //    var y = wordDoc.Revisions.Count;
        //    wordDoc.SaveAs(resultName);

        //    wordApp.Documents.Open(resultName);
        //    wordApp.Application.Quit();

        //    return;
        //}

        //public List<string> WordDocumentMailMerge_GetMergeFields()
        //{
        //    List<string> filePath = new List<string>();
        //    List<string> fieldNames = new List<string>();

        //    string virtualPath = $"Templates/{_entityType}/";
        //    var path = Path.Combine(webRootPath, virtualPath);

        //    var d = new DirectoryInfo($"{path}");

        //    foreach (FileInfo fi in d.GetFiles())
        //    {
        //        filePath.Add(fi.Name);

        //        Object oMissing = System.Reflection.Missing.Value;
        //        Object oTemplatePath = $"{ResolveApplicationDataPath(fi.Name)}";

        //        Application wordApp = new Application();
        //        Document wordDoc = new Document();

        //        wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
        //        if (wordDoc.WriteReserved)
        //        {
        //            wordDoc.Unprotect();
        //        }

        //        #region Field Level Code

        //        foreach (Field myMergeField in wordDoc.Fields)
        //        {
        //            Microsoft.Office.Interop.Word.Range rngFieldCode = myMergeField.Code;
        //            String fieldText = rngFieldCode.Text;

        //            // ONLY GETTING THE MAILMERGE FIELDS
        //            if (fieldText.StartsWith(" MERGEFIELD"))
        //            {
        //                Int32 endMerge = fieldText.IndexOf("\\");
        //                Int32 fieldNameLength = fieldText.Length - endMerge;
        //                String fieldName = fieldText.Substring(11, endMerge - 11);

        //                fieldName = fieldName.Trim();
        //                fieldNames.Add(fieldName);

        //            }
        //        }

        //        #endregion

        //        wordApp.Application.Quit();
        //    }

        //    return fieldNames;
        //}

        ////Get the path for Word template document
        //protected string ResolveApplicationDataPath(string fileName)
        //{
        //    string filePath = $"C:\\Users\\jzeller\\Documents\\visual studio 2015\\Projects\\SalesforceReplacementA\\SalesforceReplacementA.Client1\\Templates\\Clients\\{fileName}";
        //    string dataPath = new System.IO.DirectoryInfo(filePath).FullName;
        //    return string.Format("{0}", dataPath);
        //}

        ////Get the path for Word template document
        //protected string ResolveApplicationDataPath()
        //{

        //    string virtualPath = $"Templates/{_entityType}/{_documentName}";
        //    string filePath = Path.Combine(webRootPath, virtualPath);

        //    //string filePath = $"C:\\Users\\jzeller\\Documents\\visual studio 2015\\Projects\\SalesforceReplacementA\\SalesforceReplacementA.Client1\\Templates\\{_entityType}\\{_documentName}";

        //    //Data folder path is resolved from requested page physical path.
        //    //string dataPath = new System.IO.DirectoryInfo(@"C:\\Users\jzeller\\Documents\\visual studio 2015\\Projects\\DXWebApplication1\\Templates\\").FullName;
        //    string dataPath = new System.IO.DirectoryInfo(filePath).FullName;
        //    return string.Format("{0}", dataPath);
        //}

        ////Get the path for Word document result
        //protected string ResolveResultDataPath(string fileName)
        //{
        //    string filePath = $"C:\\Users\\jzeller\\Documents\\visual studio 2015\\Projects\\SalesforceReplacementA\\SalesforceReplacementA.Client1\\TemplateResults\\{_entityType}\\{_entityID}\\{_documentName}";
        //    string folderPath = $"~\\TemplateResults\\{_entityType}\\{_entityID}";

        //    _uploadHelper.CreatedocumentDirectory(folderPath);
        //    //Data folder path is resolved from requested page physical path.
        //    string dataPath = new System.IO.DirectoryInfo(filePath).FullName;
        //    return string.Format("{0}", dataPath);
        //}


        ////public List<MergeEntry> GetMergeEntryList()
        ////{
        ////    List<MergeEntry> mergeEntryList = new List<MergeEntry>();
        ////    string[] fieldNames = { "USER_STREET", "USER_CITY", "USER_STATE", "USER_POSTALCODE", "USER_PHONE", "USER_FAX",
        ////                        "CONTACT_FULLNAME", "OPPORTUNITY_LEGAL_ENTITY_NAME",
        ////                        "OPPORTUNITYOWNER_FULLNAME", "OPPORTUNITYOWNER_TITLE", "CONTACT_FULLNAME", "CONTACT_TITLE" };

        ////    //using (var db = new ApplicationDbContext())
        ////    //{
        ////    //var mergeClientEntry = db.Clients.Where(c => c.Id == _entityID).FirstOrDefault();
        ////    //var mergeContactEntry = db.Contacts.Where(c => c.Id == mergeClientEntry.ContactId).FirstOrDefault();

        ////    //if (mergeClientEntry != null && mergeContactEntry != null)
        ////    //{
        ////    foreach (var fieldName in fieldNames)
        ////    {
        ////        var appFieldName = GetFieldValue(fieldName);
        ////        FieldInfo fld = typeof(Contact).GetField(appFieldName);
        ////        var appFieldValue = fld.GetValue(null).ToString(); 

        ////        var entry = new MergeEntry
        ////        {
        ////            TemplateFieldName = fieldName,
        ////            AppFieldName = appFieldName, //typeof(Contact).GetProperty(GetFieldValue(fieldName)).ToString()
        ////            AppFieldValue = appFieldValue
        ////        }; 
        ////        mergeEntryList.Add(entry);
        ////    }
        ////    //}
        ////    //}

        ////    return mergeEntryList;
        ////}

        ////internal string GetFieldValue(string FieldName)
        ////{
        ////    switch (FieldName)
        ////    {
        ////        case "USER_STREET":
        ////            return "MailingStreet";
        ////        case "USER_STATE":
        ////            return "MailingState";
        ////        default:
        ////            return "";
        ////    }
        ////}

        //public List<string> WordDocumentMailMerge_Test()
        //{
        //    List<string> filePath = new List<string>();

        //    var d = new DirectoryInfo(@"P:\Salesforce\Salesforce Conversion\Templates\Uploaded Templates");
        //    foreach (FileInfo fi in d.GetFiles())
        //    {
        //        filePath.Add($"C:\\Users\\jzeller\\Documents\\visual studio 2015\\Projects\\SalesforceReplacementA\\SalesforceReplacementA.Client1\\Templates\\Clients\\{fi.Name}");
        //        //filePath += "<br>";
        //        //Data folder path is resolved from requested page physical path.
        //        //string dataPath = new System.IO.DirectoryInfo(@"C:\\Users\jzeller\\Documents\\visual studio 2015\\Projects\\DXWebApplication1\\Templates\\").FullName;
        //        ////string dataPath = new System.IO.DirectoryInfo(filePath).FullName;
        //        ////return string.Format("{0}", dataPath);
        //    }



        //    //Object oMissing = System.Reflection.Missing.Value;
        //    //Object oTemplatePath = $"{ResolveApplicationDataPath()}";

        //    //Application wordApp = new Application();
        //    //Document wordDoc = new Document();

        //    //wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
        //    //var lookupHelper = new LookupHelper();

        //    //Client client;
        //    //Contact contact;
        //    //Company company;
        //    //ApplicationUser owner;

        //    //#region Field Level Code
        //    ////using (var db = new ApplicationDbContext())
        //    ////{
        //    ////    client = db.Clients.Where(c => c.Id == _entityID).FirstOrDefault();
        //    ////    var clientContactID = client.ContactId;

        //    ////    if (_contactName == null)
        //    ////    {
        //    ////        contact = db.Contacts.Where(c => c.Id == clientContactID).FirstOrDefault();
        //    ////    }
        //    ////    else
        //    ////    {
        //    ////        contact = db.Contacts.Where(c => c.Id == _contactName).FirstOrDefault();
        //    ////    }

        //    ////    company = db.Companies.Where(c => c.ID == client.CompanyId).FirstOrDefault();
        //    ////    owner = db.Users.Where(u => u.Id == client.OwnerId).FirstOrDefault();
        //    ////}

        //    ////foreach (Field myMergeField in wordDoc.Fields)
        //    ////{
        //    ////    Range rngFieldCode = myMergeField.Code;
        //    ////    String fieldText = rngFieldCode.Text;

        //    ////    // ONLY GETTING THE MAILMERGE FIELDS
        //    ////    if (fieldText.StartsWith(" MERGEFIELD"))
        //    ////    {
        //    ////        // THE TEXT COMES IN THE FORMAT OF
        //    ////        // MERGEFIELD  MyFieldName  \\* MERGEFORMAT
        //    ////        // THIS HAS TO BE EDITED TO GET ONLY THE FIELDNAME "MyFieldName"

        //    ////        Int32 endMerge = fieldText.IndexOf("\\");
        //    ////        Int32 fieldNameLength = fieldText.Length - endMerge;
        //    ////        String fieldName = fieldText.Substring(11, endMerge - 11);

        //    ////        // GIVES THE FIELDNAMES AS THE USER HAD ENTERED IN .doc FILE
        //    ////        fieldName = fieldName.Trim();

        //    ////        // **** FIELD REPLACEMENT IMPLEMENTATION GOES HERE ****//
        //    ////        // THE PROGRAMMER CAN HAVE HIS OWN IMPLEMENTATIONS HERE

        //    ////        if (fieldName == "USER_STREET") { myMergeField.Select(); wordApp.Selection.TypeText(contact.MailingStreet); }
        //    ////        if (fieldName == "USER_CITY") { myMergeField.Select(); wordApp.Selection.TypeText(contact.MailingCity); }
        //    ////        if (fieldName == "USER_STATE") { myMergeField.Select(); wordApp.Selection.TypeText(contact.MailingState); }
        //    ////        if (fieldName == "USER_POSTALCODE") { myMergeField.Select(); wordApp.Selection.TypeText(contact.MailingPostalCode); }
        //    ////        if (fieldName == "USER_PHONE") { myMergeField.Select(); wordApp.Selection.TypeText(owner.Phone); }
        //    ////        if (fieldName == "USER_FAX") { myMergeField.Select(); wordApp.Selection.TypeText(owner.Fax); }

        //    ////        if (contact != null)
        //    ////        {
        //    ////            if (fieldName == "CONTACT_FULLNAME") { myMergeField.Select(); wordApp.Selection.TypeText($"{contact.FirstName} {contact.LastName}"); }
        //    ////            if (fieldName == "CONTACT_TITLE") { myMergeField.Select(); wordApp.Selection.TypeText(contact.Title); }
        //    ////        }
        //    ////        if (fieldName == "OPPORTUNITY_LEGAL_ENTITY_NAME") { myMergeField.Select(); wordApp.Selection.TypeText(client.Name); }
        //    ////        if (fieldName == "OPPORTUNITYOWNER_FULLNAME") { myMergeField.Select(); wordApp.Selection.TypeText(lookupHelper.GetUserNameFromID(client.OwnerId)); }
        //    ////        if (fieldName == "OPPORTUNITYOWNER_TITLE") { myMergeField.Select(); wordApp.Selection.TypeText(owner.Title); }

        //    ////    }
        //    ////}

        //    //#endregion

        //    //string resultName = ResolveResultDataPath("/") + ".docx";
        //    //var y = wordDoc.Revisions.Count;
        //    //wordDoc.SaveAs(resultName);

        //    //wordApp.Documents.Open(resultName);
        //    //wordApp.Application.Quit();

        //    return filePath;
        //}


        //public void MailMergeForASpecificTemplate(string templateName)
        //{
        //    var application = new Application();
        //    var templateDocument = new Document();
        //    templateName = "Form-Opp - Term Sheet - Standard.doc";

        //    if (!String.IsNullOrEmpty(templateName))
        //    {
        //        //_uploadHelper.CreatedocumentDirectory(templateName, "Contacts");
        //        string webRootPath = _webHostEnvironment.WebRootPath;
        //        var fileLocation = $"Templates/Contacts/{templateName}";

        //        templateDocument = application.Documents.Add(Template: $"{fileLocation}");

        //        templateDocument.SaveAs2($"{templateName}_merged");
        //    }
        //}

        public void MailMergeForASpecificTemplateOpenXML(string templateName)
        {
            //public string ConvertDocxToHtml(string docxFileEncodedData)
            //{

            templateName = "Form-Opp - Term Sheet - Standard.doc";
            string webRootPath = _webHostEnvironment.WebRootPath;
            var folderLocation = $"Templates/Contacts/TestDoc.doc";
            var fileLocation = $"Templates/Contacts/{templateName}";
            var folderPath = Path.Combine(webRootPath, folderLocation);
            var filePath = Path.Combine(webRootPath, fileLocation);


            Aspose.Words.Document doc = new Aspose.Words.Document(filePath);

            doc.MailMerge.Execute(new string[] { "CONTACT_FULLNAME" },
        new object[] { "John Doe" });

            //DataTable workTable = new DataTable("Customers");

            //DataColumn column = new DataColumn();
            //column.ColumnName = "CONTACT_FULLNAME";
            //column.ReadOnly = true;
            //column.Unique = false;
            //// Add the Column to the DataColumnCollection.
            //workTable.Columns.Add(column);


            //DataRow workRow = workTable.NewRow();
            //workRow["CONTACT_FULLNAME"] = "John Doe"; 
            //workTable.Rows.Add(workRow); 

            //doc.MailMerge.ExecuteWithRegions(workTable);

            doc.Save(folderPath);

            //// Use DataTable as a data source.
            //int orderId = 10444;
            //DataTable orderTable = GetTestOrder(orderId);
            //doc.MailMerge.ExecuteWithRegions(orderTable);

            ////// Instead of using DataTable, you can create a DataView for custom sort or filter and then mail merge.
            ////DataView orderDetailsView = new DataView(GetTestOrderDetails(orderId));
            ////orderDetailsView.Sort = "ExtendedPrice DESC";

            ////// Execute the mail merge operation.
            ////doc.MailMerge.ExecuteWithRegions(orderDetailsView);

            //// Save the merged document.
            //dataDir = dataDir + RunExamples.GetOutputFilePath(fileName);
            //doc.Save(dataDir);
        }

            //string inputFileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + ".docx";
            //    string imageDirectoryName = inputFileName.Split('.')[0] + "_files";

            //    DirectoryInfo imgDirInfo = new DirectoryInfo(folderPath);

            //    int imageCounter = 0;
            //    byte[] byteArray = Convert.FromBase64String(filePath);//File.ReadAllBytes(docxFile);
            //    using (MemoryStream memoryStream = new MemoryStream())
            //    {
            //        memoryStream.Write(byteArray, 0, byteArray.Length);
            //        using (WordprocessingDocument doc =
            //            WordprocessingDocument.Open(memoryStream, true))
            //        {


            //            //HtmlConverterSettings settings = new HtmlConverterSettings()
            //            //{
            //            //    PageTitle = inputFileName,
            //            //    ConvertFormatting = false,
            //            //};
            //            //XElement html = HtmlConverter.ConvertToHtml(doc, settings,
            //            //    imageInfo =>
            //            //    {
            //            //        DirectoryInfo localDirInfo = imgDirInfo;
            //            //        if (!localDirInfo.Exists)
            //            //            localDirInfo.Create();
            //            //        ++imageCounter;
            //            //        string extension = imageInfo.ContentType.Split('/')[1].ToLower();
            //            //        ImageFormat imageFormat = null;
            //            //        if (extension == "png")
            //            //        {
            //            //        // Convert the .png file to a .jpeg file.
            //            //        extension = "jpeg";
            //            //            imageFormat = ImageFormat.Jpeg;
            //            //        }
            //            //        else if (extension == "bmp")
            //            //            imageFormat = ImageFormat.Bmp;
            //            //        else if (extension == "jpeg")
            //            //            imageFormat = ImageFormat.Jpeg;
            //            //        else if (extension == "tiff")
            //            //            imageFormat = ImageFormat.Tiff;

            //            //    // If the image format is not one that you expect, ignore it,
            //            //    // and do not return markup for the link.
            //            //    if (imageFormat == null)
            //            //            return null;

            //            //        string imageFileName = "image" + imageCounter.ToString() + "." + extension;
            //            //        try
            //            //        {
            //            //            imageInfo.Bitmap.Save(imgDirInfo.FullName + "/" + imageFileName, imageFormat);
            //            //        }
            //            //        catch (System.Runtime.InteropServices.ExternalException)
            //            //        {
            //            //            return null;
            //            //        }
            //            //        XElement img = new XElement(Xhtml.img,
            //            //            new XAttribute(NoNamespace.src, imageDirectoryName + "/" + imageFileName),
            //            //            imageInfo.ImgStyleAttribute,
            //            //            imageInfo.AltText != null ?
            //            //                new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
            //            //        return img;
            //            //    });

            //            ////string htmlFilePath = HttpContext.Current.Server.MapPath("~/Documents/" + inputFileName.Split('.')[0] + ".html");
            //            ////File.WriteAllText(htmlFilePath, html.ToStringNewLineOnAttributes());

            //            ////return ConfigurationManager.AppSettings["ServerUri"].ToString() + "/Documents/" + inputFileName.Split('.')[0] + ".html";
            //        }
            //    }

            //}
        //}

    }

    public class MergeEntry
    {
        internal string TemplateFieldName { get; set; }
        internal string AppFieldName { get; set; }
        internal string AppFieldValue { get; set; }
    }
}
