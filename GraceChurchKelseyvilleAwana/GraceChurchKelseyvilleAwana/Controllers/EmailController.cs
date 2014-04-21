using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraceChurchKelseyvilleAwana.Models;

namespace GraceChurchKelseyvilleAwana.Controllers
{
    public class EmailController : Controller
    {
        //
        // GET: /Email/
        public ActionResult Index(bool confirm = false)
        {
            return View(new EmailViewModel { Confirm = confirm });
        }

        [HttpPost]
        public ActionResult Index(EmailViewModel vm)
        {
            if (vm.Recipients.Equals("Leaders"))
            {
                EmailLeaders(vm);
            }
            else if (vm.Recipients.Equals("Students"))
            {
                EmailStudents(vm);
            }

            return RedirectToAction("Index", new { @confirm = true });
        }

        private void EmailStudents(EmailViewModel vm)
        {
            
        }

        private void EmailLeaders(EmailViewModel vm)
        {
            
        }
	}
}