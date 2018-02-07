﻿using MVC.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UnitOfWorkExample.Domain.Entities;
using UnitOfWorkExample.Domain.Services;
using System.Security.Cryptography;
using System.Web.Helpers;

namespace MVC.Controllers
{
    public class AccountController : BaseController
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }

        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }
         
            User user = new User() { Email = model.Email, Password = model.Password };

            user = _accountService.GetUserDetails(user);

            

            if (user != null&& Crypto.VerifyHashedPassword(user.Password, model.Password)) {
                FormsAuthentication.SetAuthCookie(model.Email, false);

                var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.Roles);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                return RedirectToAction("Index", "Home");
            } else {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout() {
           
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        
        public ActionResult ManageUser() {
            return View(new UserManagementViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddUser(UserManagementViewModel model) {
            try {
                User user = new User();
                var newGuid = Guid.NewGuid();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserId = newGuid;
                user.Email = model.Email;
                var passwordHash = Crypto.HashPassword(model.Password);
                user.Password = passwordHash;
                _accountService.Create(user);
                return RedirectToAction("Index", "Home");
            }catch(Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

    }
}