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
      var usersList = db.UsersInfo.OrderBy(m => m.Name);
      foreach (var user in usersList)
      {
        model.BuyerDataItems.Add(new BuyerDataItem()
        {
          UserId = user.Id,
          Name = user.Name,
          Data = 0
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

        foreach (var item in model.BuyerDataItems)
        {
          if (item.Data > 0)
          {
            db.DebtParts.Add(new DebtPart()
            {
              Part = null,
              Summ = item.Data,
              GlobalFlag = false,
              Event = newEvent,
              User = db.UsersInfo.Find(item.UserId)
            });
            db.DebtParts.Add(new DebtPart()
            {
              Part = null,
              Summ = -item.Data,
              GlobalFlag = false,
              Event = newEvent,
              User = db.UsersInfo.Find(item.UserId)
            });
          }
        }
        try
        {
          db.SaveChanges();
          logger.Info(String.Format("User {0} add new event {1}", newEvent.Reporter.Nick, newEvent.Name));
        }
        catch (Exception)
        {
          logger.Error("DataBase error!");
          logger.Error(String.Format("User {0} add new event {1}", newEvent.Reporter.Nick, newEvent.Name));
          return View(model);          
        }
        return RedirectToAction("SetDebt", "Events", new { eventId = newEvent.Id });
      }

      return View(model);
    }


    //GET: Events/SetDebt
    public ActionResult SetDebt(int eventId)
    {
      SetDebtModel model = new SetDebtModel();
      var usersList = db.UsersInfo.OrderBy(m => m.Name).ToList();
      //List<DebtPart> debtParts = db.DebtParts.Where(m => m.EventId == tempVariable && m => m.UserId == user.Id)
          //{
        //selected = ((d.Part > 0) || (d.Summ < 0).ToList();
      foreach (var user in usersList)
      {        
        var debtor = db.DebtParts.SingleOrDefault(m => m.EventId == eventId &&
          m.UserId == user.Id && ((m.Part > 0) || (m.Summ < 0)));
        bool selected = (debtor != null);
        model.DebtorCoiseItems.Add(new DebtorCoiseItem()
        {
          UserId = user.Id,
          Name = user.Name,
          Selected = selected
        });
      }
      return View(model);
    }
    [HttpPost]
    public ActionResult SetDebt(SetDebtModel model)
    {

      try
      {
        db.SaveChanges();
        //logger.Debug(String.Format("User {0} add new event {1}", newEvent.Reporter.Nick, newEvent.Name));
      }
      catch (Exception)
      {
        logger.Error("DataBase error!");
        //logger.Error(String.Format("User {0} add new event {1}", newEvent.Reporter.Nick, newEvent.Name));
        return View(model);
      }
      return View(model);
    }
  }
}