
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnitOfWorkExample.Domain.Entities;

namespace MVC.Models.Account
{
    public class AccountDetailModel
    {
        public int? Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        public string Password { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Customer")]
        public int? CustomerId { get; set; }

        public IEnumerable<SelectListItem> CustomersList { get; set; }

        public  DateTime CreatedDate { get; set; }
        public  DateTime UpdatedDate { get; set; }
        public  bool IsActive { get; internal set; }
    }
}