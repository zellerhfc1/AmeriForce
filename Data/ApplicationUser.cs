using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace AmeriForce.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Department { get; set; }

        public string Signature { get; set; }


        [StringLength(14, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "Invalid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone #")]
        public string Phone { get; set; }

        [StringLength(14, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "Invalid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Fax #")]
        public string Fax { get; set; }

        [StringLength(14, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "Invalid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile #")]
        public string MobilePhone { get; set; }
        public string Alias { get; set; }
        public string CommunityNickname { get; set; }
        public bool IsActive { get; set; }
        public string TimeZoneSidKey { get; set; }
        public string UserRoleId { get; set; }
        public string ProfileId { get; set; }
        public string UserType { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedById { get; set; }
        public int? NumberOfFailedLogins { get; set; }
        public int? Extension { get; set; }
        public string ProfilePhotoId { get; set; }
        public string Title { get; set; }
    }
}
