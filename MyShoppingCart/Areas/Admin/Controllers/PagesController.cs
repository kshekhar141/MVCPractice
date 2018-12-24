using MyShoppingCart.Models.Data;
using MyShoppingCart.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public ActionResult Index()
        {
            List<PageVM> pagesList;

            using (Db db = new Db())
            {
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

                return View(pagesList);
        }

        //GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        //POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Init PageDTO
            using (Db db = new Db())
            {
                string slug;
                PageDTO dto = new PageDTO();
                dto.Title = model.Title;
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                //Make Sure Title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || (db.Pages.Any(x => x.Slug == model.Slug))){
                    ModelState.AddModelError("", "Title or Slug is already exits.");
                    return View(model);
                }
                //DTO  the rest fields
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                db.Pages.Add(dto);
                db.SaveChanges();
            }
                TempData["Msg"] = "You have added a new Page!!";
                return RedirectToAction("AddPage"); 
        }

        //GET: Admin/Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageVM model;
            using (Db db = new Db())
            {
                PageDTO dto = db.Pages.Find(id);
                if (dto == null)
                {
                    return Content("The Page doesn't exist!!");
                }

                model = new PageVM(dto);
            }
            return View(model);
        }


        //POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult Editpage(PageVM model)
        {
            //Check Model
            if (! ModelState.IsValid)
            {
                return View(model);
            }
            using(Db db=new Db())
            {
                int id = model.Id;

                string slug="Home";

                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Get title
                dto.Title = model.Title;

                //Check for slug
                if (model.Slug == "Home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();

                    }
                }

                //Check for title or slug is unique
                if(db.Pages.Where(x=>x.Id!=id).Any(x=>x.Title==model.Title)|| db.Pages.Where(x => x.Id != id).Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "Title or slug is already exists");
                    return View(model);
                }
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                db.SaveChanges();
            }
            TempData["Msg"] = "You have edited this Page";
            return RedirectToAction("EditPage");
            
        }

        public ActionResult PageDetails(int id)
        {
            PageVM model;
            using (Db db=new Db())
            {
                PageDTO dto = db.Pages.Find(id);
                if (dto == null)
                {
                    return Content("This Page doesnot Exists");
                }
                model = new PageVM(dto);

            }
            return View(model);

        }

        public ActionResult DeletePage(int id)
        {
            //Get the Page
            using (Db db = new Db())
            {

                //remove the Page
                PageDTO dto= db.Pages.Find(id);

                db.Pages.Remove(dto);
                //Save the Page
                db.SaveChanges();

            }
            //Redirect the page

            return RedirectToAction("Index");
        }
    }
}