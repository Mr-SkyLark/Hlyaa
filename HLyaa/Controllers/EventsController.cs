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
      CreateNewEventModel model = new CreateNewEventModel();
      foreach (var user in db.UsersInfo.ToList())
      {
        model.CheckBoxDataItems.Add(user.Id, new CheckBoxItem()
        {
          Name = user.Name,
          Data = 0,
          Selected = false
        });
      }
      return View(model);
    }
    [HttpPost]
    public ActionResult CreateEvent(CreateNewEventModel model)
    {
      if (ModelState.IsValid)
      {
        var newEvent = new Event()
        {
          Name = model.EventName,
          GodDebt = false,
          DateCreated = DateTime.Now,
          Reporter = ControllerHelper.CurrentUserInfo(db, UserManager)
        };
        newEvent = db.Events.Add(newEvent);

        foreach (var keypar in model.CheckBoxDataItems)
        {
          if (keypar.Value.Selected)
          {
            db.DebtParts.Add(new DebtPart()
            {
              Part = null,
              Summ = keypar.Value.Data,
              GlobalFlag = false,
              Event = newEvent,
              User = db.UsersInfo.Find(keypar.Key)
            });
            db.DebtParts.Add(new DebtPart()
            {
              Part = null,
              Summ = -keypar.Value.Data,
              GlobalFlag = false,
              Event = newEvent,
              User = db.UsersInfo.Find(keypar.Key)
            });
          }
        }
        db.SaveChanges();
        return View(model);
      }

      return View(model);
    }
  }
}