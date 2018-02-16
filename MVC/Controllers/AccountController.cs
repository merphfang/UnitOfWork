using MVC.Models.Account;
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
using UnitOfWorkExample.Domain.Enums;
using MVC.Models;
using AutoMapper;

namespace MVC.Controllers
{
    public class AccountController : BaseController
    {
        private IAccountService _accountService;
        private ICustomerService _customerService;
        public AccountController(IAccountService accountService, ICustomerService customerService) {
            _accountService = accountService;
            _customerService = customerService;
        }


        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            User user = new User() { Email = model.Email, Password = model.Password };

            user = _accountService.GetUserDetails(user);



            if (user != null && user.IsActive && Crypto.VerifyHashedPassword(user.Password, model.Password)) {
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




        #region Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Add() {
            var model = new AccountDetailModel();

            var customers = _customerService.GetAll();
            model.CustomersList = customers.Select(c => new SelectListItem {
                Value = c.Id.ToString(),
                Text = c.Name

            });

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id) {
            User user = _accountService.GetById(id);
            AccountDetailModel model = Mapper.Map<AccountDetailModel>(user);

            var customers = _customerService.GetAll();
            model.CustomersList = customers.Select(c => new SelectListItem {
                Value = c.Id.ToString(),
                Text = c.Name

            });
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Index() {

            return View();
        }



        [Authorize(Roles = "Admin")]
        public JsonResult GetAccounts(jQueryDatatableParam param) {
            int total = 0;
            string sortCol = param.columns[Convert.ToInt32(param.order[0]["column"])]["name"];
            string sortDir = param.order[0]["dir"];



            var users = _accountService.GetUsers(param.search["value"] ?? "", param.start, param.length, sortCol, sortDir, out total);
            if (users != null) {
                List<AccountViewModel> usersViewModel = Mapper.Map<List<AccountViewModel>>(users.ToList());
                return Json(new {
                    recordsTotal = total,
                    recordsFiltered = total,
                    data = usersViewModel.AsQueryable()
                },
           JsonRequestBehavior.AllowGet);
            } else {
                return Json(new {
                    recordsTotal = total,
                    recordsFiltered = total,
                    data = Enumerable.Empty<AccountViewModel>().AsQueryable()
                },
           JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(AccountDetailModel model) {
           
                try {
                    var nowdate = DateTime.Now;
                    var customerId = model.CustomerId.Value;
                    if (!model.Id.HasValue) {
                        User user = new User();
                        var newGuid = Guid.NewGuid();
                        user.CreatedDate = nowdate;
                        user.UpdatedDate = nowdate;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.UserId = newGuid;
                        user.Email = model.Email;
                        user.Roles = UserRoles.User.ToString();
                        user.Customer = _customerService.GetById(customerId);
                        user.IsActive = Convert.ToBoolean(Request.Form["IsActive"]);

                        var passwordHash = Crypto.HashPassword(model.Password);
                        user.Password = passwordHash;
                        _accountService.Create(user);

                    } else {
                        User user = _accountService.GetById(model.Id.Value);
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Email = model.Email;
                        user.Roles = UserRoles.User.ToString();
                        user.UpdatedDate = nowdate;
                        user.Customer = _customerService.GetById(customerId);
                        user.IsActive = Convert.ToBoolean(Request.Form["IsActive"]);
                        var passwordHash = Crypto.HashPassword(model.Password);
                        user.Password = passwordHash;

                        _accountService.Update(user);
                    }
                    return RedirectToAction("Index", "Account");
                }
                catch (Exception ex) {
                throw ex;
            }
            
        }
        #endregion
    }
}