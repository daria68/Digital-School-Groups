using proiect.Models;
using proiect_useri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Activity act)
        {
            try
            {
                db.Activities.Add(act);
                db.SaveChanges();
                return Redirect("/Groups/Show/" + act.GroupId);
            }
            catch (Exception e)
            {
                return Redirect("/Groups/Show/" + act.GroupId);
            }
        }
    }
}