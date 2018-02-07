using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnitOfWorkExample.Domain.Helpers;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        [Inject]
        public IUnitOfWork UnitOfWork { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!filterContext.IsChildAction)
                UnitOfWork.BeginTransaction();
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext) {
            if (!filterContext.IsChildAction)
                UnitOfWork.Commit();
        }
    }
}