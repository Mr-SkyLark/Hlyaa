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
    //POST: Events/CreateEvent
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
      SetDebtModel model = new SetDebtModel() { EventId = eventId };
      var usersList = db.UsersInfo.OrderBy(m => m.Name).ToList();
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
    //POST: Events/SetDebt
    [HttpPost]
    public ActionResult SetDebt(SetDebtModel model)
    {
      string retList = "";
      bool warningFlag = true;
      foreach(var item in model.DebtorCoiseItems)
      {
        
        if (item.Selected == false)
        {
          var list = db.DebtParts.Where(m => m.EventId == model.EventId &&
            m.UserId == item.UserId && ((m.Part > 0) || (m.Summ < 0))).ToList();

          db.DebtParts.RemoveRange(list);
          try
          {
            db.SaveChanges();
            logger.Debug(String.Format("Remove user {0} in event {1}", item.UserId, model.EventId));
          }
          catch (Exception)
          {
            logger.Error("DataBase error!");
            logger.Error(String.Format("Remove user {0} in event {1}", item.UserId, model.EventId));
          }
        }
        else
        {
          retList += item.UserId.ToString() + ",";
          warningFlag = false;
        }
      }
      
      if(warningFlag)
      {
        return View(model);
      }
      if (retList.Count() > 0)
      {
        retList = retList.Substring(0, retList.Length - 1);
      }
      var test = retList.Split(',').Select(int.Parse).ToList();
      return RedirectToAction("SetPartAndPrice", "Events", new { userList = retList, eventId = model.EventId });
    }

    public class IntListModelBinder : DefaultModelBinder
    {
      public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
      {
        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (value == null || string.IsNullOrEmpty(value.AttemptedValue))
        {
          return null;
        }

        return value
            .AttemptedValue
            .Split(',')
            .Select(int.Parse)
            .ToList();
      }
    }

    //GET: Events/SetPartAndPrice
    public ActionResult SetPartAndPrice([ModelBinder(typeof(IntListModelBinder))]List<int> userList, int eventId)
    {
      
      SetPartModel model = new SetPartModel() { EventId = eventId };
      var usersList = db.UsersInfo.OrderBy(m => m.Name).ToList();
      foreach (var user in usersList)
      {
        if (userList.Contains(user.Id))
        {
          model.DebtorDataItems.Add(new DebtorDataItem()
          {
            UserId = user.Id,
            Name = user.Name,
            DebtPart = 1,
            DebtSum = 0
          });
        }
      }
      return View(model);
    }
    //POST: Events/SetPartAndPrice
    [HttpPost]
    public ActionResult SetPartAndPrice(SetPartModel model)
    {
      int debtPartCount = 0;
      double debtSumSum = 0;
      double debtPartSum = 0;

      foreach (var item in model.DebtorDataItems)
      {        
        if (item.DebtPart > 0)
        {
          ++debtPartCount;
          debtPartSum += item.DebtPart;
        }
        else if (item.DebtSum > 0)
        {
          debtSumSum += item.DebtSum;
        }
      }

      double globalSum = db.DebtParts.Where(m => m.EventId == model.EventId && m.Summ > 0).Sum(m => m.Summ);
      if(debtSumSum >globalSum)
      {
        return View(model);
      }

      double calcSum = globalSum - debtSumSum;
      foreach (var item in model.DebtorDataItems)
      {
        var debtor = db.DebtParts.SingleOrDefault(m => m.EventId == model.EventId &&
          m.UserId == item.UserId && ((m.Part > 0) || (m.Summ < 0)));
        if (item.DebtPart > 0)
        {
          double realSum = (item.DebtPart * calcSum) / debtPartSum;
          if (debtor == null)
          {
            debtor = db.DebtParts.Add(new DebtPart()
            {
              Part = item.DebtPart,
              Summ = -realSum,
              GlobalFlag = false,
              EventId = model.EventId,
              UserId = item.UserId
            });
          }
          else
          {
            debtor.Part = item.DebtPart;
            debtor.Summ = -realSum;
          }
        }
        else if(item.DebtSum > 0)
        {          
          if (debtor == null)
          {
            debtor = db.DebtParts.Add(new DebtPart()
            {
              Part = null,
              Summ = -item.DebtSum,
              GlobalFlag = false,
              EventId = model.EventId,
              UserId = item.UserId
            });
          }
          else
          {
            debtor.Part = null;
            debtor.Summ = -item.DebtSum;
          }
        }
      }

      try
      {
        db.SaveChanges();
        logger.Info(String.Format("Compleate create event {0}", model.EventId));
      }
      catch (Exception)
      {
        logger.Error("DataBase error!");
        logger.Error(String.Format("Compleate create event {0}", model.EventId));
        return View(model);
      }
      double test2 = db.DebtParts.Where(m => m.EventId == model.EventId).Sum(m => m.Summ);
      if (test2 > -0.005 && test2 < 0.005)
      {
        logger.Fatal("Logic error!");
        logger.Fatal(String.Format("Debt sum is not equal 0. Event {0}", model.EventId));
        return View(model);
      }

      return View(model);
    }

  }
}