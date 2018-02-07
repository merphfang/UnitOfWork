using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnitOfWorkExample.Domain.Services;

namespace MVC.Controllers
{
    public class HomeController : BaseController
    {
        private IProductService _productService;
        public HomeController(IProductService productService) {
            _productService = productService;
        }
        
        public ActionResult Index() {
            var products = _productService.GetAll();
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}