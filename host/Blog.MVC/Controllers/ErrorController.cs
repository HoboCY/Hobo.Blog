using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blog.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ComponentError(string errorMessage = null)
        {
            return View(errorMessage);
        }

        [HttpGet]
        public IActionResult ServerError(string errorMessage=null)
        {
            return View(errorMessage);
        }
    }
}
