using AmeriForce.Data;
using AmeriForce.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmeriForce.Services
{
    public class CompanyContactMerge
    {
        private readonly ApplicationDbContext _context;
        public List<ContactDuplicateInformationViewModel> PotentialContactsToMerge { get; set; }
        public string CompanyID { get; set; }

        public CompanyContactMerge(string companyID, ApplicationDbContext context)
        {
            _context = context;
            CompanyID = companyID;
            this.PotentialContactsToMerge = new List<ContactDuplicateInformationViewModel>();
        }


        public List<ContactDuplicateInformationViewModel> GetPotentialContactsToMerge()
        {
            try
            {
                    var potentialContacts = _context.Contacts.Where(c => c.AccountId == CompanyID);
                    if (potentialContacts != null)
                    {
                        foreach (var potentialContact in potentialContacts)
                        {
                            var mergeContact = new ContactDuplicateInformationViewModel()
                            {
                                ID = potentialContact.Id,
                                FirstName = potentialContact.FirstName,
                                LastName = potentialContact.LastName,
                                RelationshipStatus = potentialContact.Relationship_Status,
                                RatingSort = potentialContact.Rating_Sort,
                                Phone = potentialContact.Phone,
                                Mobile = potentialContact.MobilePhone,
                                Email = potentialContact.Email
                            };
                            PotentialContactsToMerge.Add(mergeContact);
                        }
                        return PotentialContactsToMerge;
                    }
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void MergeContacts(ContactToBeMergedViewModel mergedContact)
        {
            try
            {
                    var contactToBeMerged = _context.Contacts.Where(c => c.Id == mergedContact.ID).FirstOrDefault();
                    if (contactToBeMerged != null)
                    {
                        contactToBeMerged.Id = mergedContact.ID;
                        contactToBeMerged.FirstName = mergedContact.FirstName;
                        contactToBeMerged.LastName = mergedContact.LastName;
                        contactToBeMerged.Relationship_Status = mergedContact.RelationshipStatus;
                        contactToBeMerged.Rating_Sort = mergedContact.RatingSort;
                        contactToBeMerged.Phone = mergedContact.Phone;
                        contactToBeMerged.MobilePhone = mergedContact.Mobile;
                        contactToBeMerged.Email = mergedContact.Email;

                        DeleteMergedContact(mergedContact.NonMasterIDA, mergedContact.ID);
                        DeleteMergedContact(mergedContact.NonMasterIDB, mergedContact.ID);

                        _context.SaveChanges();
                    }
            }
            catch (Exception ex)
            {

            }
        }


        private void DeleteMergedContact(string contactToBeDeleted, string masterContact)
        {
            try
            {
                var associatedClients = _context.Clients.Where(c => c.ContactId == contactToBeDeleted);
                if (associatedClients != null)
                {
                    foreach (var associatedClient in associatedClients)
                    {
                        associatedClient.ContactId = masterContact;
                        _context.SaveChanges();
                    }
                }

                var associatedContactRoles = _context.ClientContactRoles.Where(c => c.ContactId == contactToBeDeleted);
                if (associatedContactRoles != null)
                {
                    foreach (var associatedContactRole in associatedContactRoles)
                    {
                        associatedContactRole.ContactId = masterContact;
                        _context.SaveChanges();
                    }
                }

                // MOVE CONTACT TO DUPLICATE DELETED TABLE



            }
            catch (Exception ex)
            {

            }
        }
    }
}
