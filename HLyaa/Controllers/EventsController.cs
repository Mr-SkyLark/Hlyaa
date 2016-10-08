using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using HLyaa.Logger;
using HLyaa.Entities;
using HLyaa.Models;
using HLyaa.Helper;

namespace HLyaa.Controllers
{
  public class EventsController : Controller
  {
    private static NLogLogger logger = new NLogLogger();
    private static ApplicationDbContext db = new ApplicationDbContext();

    public EventsController()
    : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)))
    {
    }

    public EventsController(UserManager<ApplicationUser> userManager)
    {
      UserManager = userManager;
    }

    public UserManager<ApplicationUser> UserManager { get; private set; }

    // GET: Events
    public ActionResult Index()
    {
      return View();
    }
    //GET: Events/CreateEvent
    public ActionResult CreateEvent()
    {
      CheckBoxDoubleModel model = new CheckBoxDoubleModel();
      foreach (var user in db.UsersInfo.ToList())
      {
        model.CheckBoxDataItems.Add(new SelectListItem
        {
          Text = user.Name,
          Value = user.Id.ToString()
        });
        model.DoubleItems.Add(user.Id, 0);
      }
      return View(model);
    }
    [HttpPost]
    public ActionResult CreateEvent(CheckBoxDoubleModel model)
    {
      var newEvent = new Event() { Name = "test", GodDebt = false, DateCreated = DateTime.Now,
        Price =123.123, Reporter= ControllerHelper.CurrentUserInfo(db, UserManager)};
      db.Events.Add(newEvent);
      db.SaveChanges();

      return View(model);
    }
  }
}