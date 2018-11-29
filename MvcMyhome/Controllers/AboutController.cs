using HashidsNet;
using MvcMyhome.Controllers.Attribute;
using QConnectSDK.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMyhome.Controllers
{
    [Auth]
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult About()
        {
            return View();
        }

        public ActionResult ceshi()
        {
            return View();
        }
    }
}