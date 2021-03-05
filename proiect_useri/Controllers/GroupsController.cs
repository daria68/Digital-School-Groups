using FriendRequest.Models;
using Microsoft.AspNet.Identity;
using proiect.Models;
using proiect_useri.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();



        // GET: Groups
        [Authorize(Roles = "Admin,Moderator,User")]
        public ActionResult Index()
        {
            //join pe care il face cu category
            
            var groups = db.Groups.Include("Category").Include("User");
            ViewBag.Groups = groups;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            var users = (from user in db.Users
                                           .Include(u => u.SentRequests)
                                           .Include(u => u.ReceivedRequests)
                         select user).ToList();
            ViewBag.Users = users;
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            return View();
        }
        [Authorize(Roles = "Admin,Moderator,User")]
        public ActionResult Show(int id)
        {
            Group group = db.Groups.Find(id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }
            var users = (from user in db.Users
                                           .Include(u => u.SentRequests)
                                           .Include(u => u.ReceivedRequests)
                         select user).ToList();
            ViewBag.Users = users;
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Moderator") || User.IsInRole("Admin") || group.UserId == User.Identity.GetUserId())
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
            return View(group);
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        public ActionResult New()
        {

            Group group = new Group();
            group.Categ = GetAllCategories();

            group.UserId = User.Identity.GetUserId();
            return View(group);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator,User")]
        public ActionResult New(Group group)
        {
            group.Categ = GetAllCategories();
           group.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Groups.Add(group);
                    db.SaveChanges();
                    TempData["message"] = "Grupul a fost adaugat!";
                    return RedirectToAction("Index");
                }
                else
                {
                    group.Categ = GetAllCategories();
                    return View(group);
                }
            }
            catch (Exception e)
            {
                group.Categ = GetAllCategories();
                return View(group);
            }
        }
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id)
        {
            Group group = db.Groups.Find(id);
            group.Categ = GetAllCategories();
            if (group.UserId==User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(group);
            }
            else
            {
                TempData["message"] = "Nu e voie, n-ai drepturi";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int id, Group requestGroup)
        {
            

            try
            {
                if (ModelState.IsValid)
                {
                    Group group = db.Groups.Find(id);
                    if (group.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(group))
                        {

                            group.GroupName = requestGroup.GroupName;
                            group.CategoryId = requestGroup.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Grupul a fost editat";


                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu e voie, n-ai drepturi";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    requestGroup.Categ = GetAllCategories();
                    return View(requestGroup);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {

            Group group = db.Groups.Find(id);
            if (group.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Groups.Remove(group);
                db.SaveChanges();
                TempData["message"] = "Grupul a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu e voie, n-ai drepturi";
                return RedirectToAction("Index");
            }
        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
            {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }
        [HttpPost]
        public ActionResult AddFriend(int id)
        {
            string currentUser = User.Identity.GetUserId();
            
            Group group = db.Groups.Find(id);
            Friend friendship = new Friend();
            friendship.User1_Id = currentUser;
            friendship.User2_Id = group.UserId;
            friendship.GroupId = group.GroupId;
            if (User.IsInRole("Admin"))
            {
                friendship.Accepted = true;
            }
            else
            {
                friendship.Accepted = false; // Accepted = false, iar in lista de cereri -> accept
            }
            
            friendship.RequestTime = DateTime.Now;

            // TODO: sa existe try si catch astfel incat sa nu se trimita o cerere de doua ori
            // verificare daca userul a primit deja cerere de la userul caruia doreste sa ii trimita
            // de verificat ca user1 sa nu fie deja prieten cu user2

            db.Friends.Add(friendship);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddFriend2(int id)
        {
            
            Friend friendship = db.Friends.Find(id);
            if (friendship.User2_Id == User.Identity.GetUserId() || User.IsInRole("Admin"))
                friendship.Accepted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFriend(int id)
        {

            Friend friendship = db.Friends.Find(id);
            if (friendship.User2_Id == User.Identity.GetUserId() || User.IsInRole("Admin"))
                db.Friends.Remove(friendship);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
   

}
