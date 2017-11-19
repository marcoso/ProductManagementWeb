using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagementWebSample.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            ViewBag.Title = "Webshop";
            ViewBag.Message = "Online shop";

            return View();
        }        
    }
}