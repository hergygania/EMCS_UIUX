using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App.Web.Models
{

    public class UserViewModel
    {
        public Data.Domain.UserAccess User { get; set; }
        public List<Data.Domain.RoleAccess> RolesList { get; set; }
        public List<Data.Domain.Store> StoreList { get; set; }
        public List<Data.Domain.Area> AreaList { get; set; }
        public List<Data.Domain.Hub> HubList { get; set; }
        public List<Data.Domain.Group> GroupList { get; set; }

        public List<Data.Domain.EscalationLimit> LevelList { get; set; }

        public List<Data.Domain.UserAccess_Role> UserRoles { get; set; }
        public List<Data.Domain.UserAccess_Store> UserStores { get; set; }
        public List<Data.Domain.UserAccess_Area> UserAreas { get; set; }
        public List<Data.Domain.UserAccess_Hub> UserHub { get; set; }


        public int[] SelectedRoles { get; set; }
        public int[] SelectedStores { get; set; }
        public int[] SelectedAreas { get; set; }
        public int[] SelectedHub { get; set; }
    }

    public class UserSignInViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }
    }

    public class UserSignEprocInViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }
        [Required]
        [Display(Name = "Token")]
        public string Token { get; set; }
        [Required]
        [Display(Name = "User ID")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }
    }
    public class AccountPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}