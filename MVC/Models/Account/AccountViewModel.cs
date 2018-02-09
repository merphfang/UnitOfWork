using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnitOfWorkExample.Domain.Entities;
using AutoMapper.Attributes;

namespace MVC.Models.Account
{
    
    public class AccountViewModel
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Customer { get; set; }
        public virtual string CreatedDate { get; set; }
       
    }
}