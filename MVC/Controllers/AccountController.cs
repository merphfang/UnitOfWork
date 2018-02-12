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

            

            if (user != null&&user.IsActive&& Crypto.VerifyHashedPassword(user.Password, model.Password)) {
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

        [Authorize(Roles = "Admin")]
        public ActionResult ManageUser() {
            return View(new UserManagementViewModel());
        }


        #region Admin

        [Authorize(Roles = "Admin")]
        public ActionResult Index() {
         
            return View();
        }


       
        [Authorize(Roles = "Admin")]
        public JsonResult GetAccounts(jQueryDatatableParam param) {
            int total = 0;
            string sortCol =param.columns[Convert.ToInt32(param.order[0]["column"])]["name"];
            string sortDir = param.order[0]["dir"];
           


            var users = _accountService.GetUsers(param.search["value"]??"",param.start, param.length,sortCol,sortDir, out total);
            if (users!=null) {
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
        public ActionResult EditUser(UserManagementViewModel model) {
            if (ViewData.ModelState.IsValid) {
                try {


                    if (!model.Id.HasValue) {
                        User user = new User();
                        var newGuid = Guid.NewGuid();
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.UserId = newGuid;
                        user.Email = model.Email;
                        user.Roles = UserRoles.User.ToString();
                        var passwordHash = Crypto.HashPassword(model.Password);
                        user.Password = passwordHash;
                        _accountService.Create(user);

                    } else {
                        User user = _accountService.GetById(model.Id.Value);
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Email = model.Email;
                        user.Roles = UserRoles.User.ToString();
                        var passwordHash = Crypto.HashPassword(model.Password);
                        user.Password = passwordHash;

                        _accountService.Update(user);
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex) {
                    ModelState.AddModelError("", ex.Message);
                    return View("~/Views/Account/ManageUser.cshtml", model);
                }
            }else {
                return View("~/Views/Account/ManageUser.cshtml", model);
            }
        }
        #endregion
    }
}