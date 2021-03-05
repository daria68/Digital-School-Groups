using Microsoft.AspNet.Identity;
using proiect.Models;
using proiect_useri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    public class MessagesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult New(Message message)
        {

            try
            {
                string currentUser = User.Identity.GetUserId();

                message.User_Id = User.Identity.GetUserId();
                db.Messages.Add(message);
                db.SaveChanges();
                TempData["message"] = "Mesajul a fost adaugat!";
                return Redirect("/Groups/Show/" + message.GroupId);
            }
            catch (Exception e)
            {
                return Redirect("/Groups/Show/" + message.GroupId);
            }
        }
        public ActionResult Edit(int id)
        {
            Message message = db.Messages.Find(id);

            return View(message);
        }

        [HttpPut]
        public ActionResult Edit(int id, Message requestMessage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Message message = db.Messages.Find(id);
                    if (TryUpdateModel(message))
                    {

                        message.MessageContent = requestMessage.MessageContent;
                        TempData["message"] = "Mesajul a fost modificat!";
                        db.SaveChanges();
                    }
                    return Redirect("/Groups/Show/" + message.GroupId);
                }
                else
                {
                    return View(requestMessage);
                }
            }
            catch (Exception e)
            {
                return View(requestMessage);
            }
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            TempData["message"] = "Mesajul a fost sters!";
            db.SaveChanges();
            return Redirect("/Groups/Show/" + message.GroupId);
        }
    }


}
