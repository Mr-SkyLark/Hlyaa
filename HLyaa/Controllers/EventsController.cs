﻿using System;
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
    private static ControllerHelper userHelper = new ControllerHelper(db);

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
      // Передаём в модель днанных выводимую информацию: id пользователя,
      // имя пользователя, и сумма взноса (равна 0).
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
        // Создаем новое событие
        var newEvent = new Event()
        {
          Name = model.EventName,
          GodDebt = false,
          DateCreated = DateTime.Now,
          Reporter = userHelper.CurrentUserInfo()
        };
        // получаем в newEvent новый Id 
        newEvent = db.Events.Add(newEvent);

        // Сразу создаем информацию о взносе и информацию о долге.
        // Информация о долге нужна для корректной работы системы:
        // "сумма всех операций должна быть равна 0 (или близка к ней)"
        foreach (var item in model.BuyerDataItems)
        {
          if (item.Data > 0)
          {
            // Положительная операция (взнос)
            db.DebtParts.Add(new DebtPart()
            {
              Part = null,
              Summ = item.Data,
              GlobalFlag = false,
              Event = newEvent,
              User = db.UsersInfo.Find(item.UserId)
            });
            // Отрицательная операция (долг)
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
        // Попытка запихать все в БД
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
    /// <summary>
    /// GET запрос метода выбора должников
    /// </summary>
    /// <param name="eventId"> Идентификатор события</param>
    public ActionResult SetDebt(int eventId)
    {
      // Работаем с событием с идентификатором eventId
      SetDebtModel model = new SetDebtModel() { EventId = eventId };
      // Получаем сисок всех пользователей
      var usersList = db.UsersInfo.OrderBy(m => m.Name).ToList();
      // Заполняем модель данных
      foreach (var user in usersList)
      {
        // Ищем записи в таблице операций с отрицательным значением суммы
        var debtor = db.DebtParts.SingleOrDefault(m => m.EventId == eventId &&
          m.UserId == user.Id && ((m.Part > 0) || (m.Summ < 0)));
        // Если найдено, то переменная selected будет равно true,
        // это значение перейдет в CheckBox
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
    /// <summary>
    /// POST запрос метода выбора должников.
    /// В модель приходит список выбранных пользователей
    /// </summary>
    /// <param name="model"> Модель данных</param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult SetDebt(SetDebtModel model)
    {
      string retList = "";
      bool warningFlag = true;
      foreach(var item in model.DebtorCoiseItems)
      {
        // Если пользователь не выбран - очищаем его отрицательные операции этого события
        if (item.Selected == false)
        {
          // Ищем отрицательные операции этого события не выбранного пользователя
          var list = db.DebtParts.Where(m => m.EventId == model.EventId &&
            m.UserId == item.UserId && ((m.Part > 0) || (m.Summ < 0))).ToList();

          // Удаляем из контекста
          db.DebtParts.RemoveRange(list);
          // Пробуем удалить из БД
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
          // Если пользователь выбран Заполняем выходную строку пользователей
          retList += item.UserId.ToString() + ",";
          warningFlag = false;
        }
      }
      
      //Если Ни один пользователь не выбран - ничего не происходит
      if(warningFlag)
      {
        return View(model);
      }
      else
      {
        // Убираем последнюю запятую из списка пользователей
        retList = retList.Substring(0, retList.Length - 1);
      }
      return RedirectToAction("SetPartAndPrice", "Events", new { userList = retList, eventId = model.EventId });
    }


    //GET: Events/SetPartAndPrice
    /// <summary>
    /// GET запрос метода выставления долгов
    /// </summary>
    /// <param name="userList"> Список выбранных должников</param>
    /// <param name="eventId"> Идентификатор события</param>
    /// <returns></returns>
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
    /// <summary>
    /// POST запрос метода выставления долгов
    /// </summary>
    /// <param name="model"> Модель данных</param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult SetPartAndPrice(SetPartModel model)
    {
      // Начало расчета долгов
      double debtSumSum = 0;
      double debtPartSum = 0;

      // Считаем сумму всех частей долгов и сумму всех денежных долгов
      foreach (var item in model.DebtorDataItems)
      {        
        if (item.DebtPart > 0)
        {
          debtPartSum += item.DebtPart;
        }
        else if (item.DebtSum > 0)
        {
          debtSumSum += item.DebtSum;
        }
      }

      // Считаем общую сумму взноса (положительных операций)
      double globalSum = db.DebtParts.Where(m => m.EventId == model.EventId && m.Summ > 0).Sum(m => m.Summ);
      double diffSum = globalSum - debtSumSum;
      // Проверка на не корректные значения
      if ( (debtPartSum == 0 && diffSum > 1) || (debtSumSum > globalSum) )
      {
        return View(model);
      }
      // Расчет долга для каждого пользователя
      foreach (var item in model.DebtorDataItems)
      {
        // Ищем: есть ли запись о долге для этого пользователя в этом событии
        var debtor = db.DebtParts.SingleOrDefault(m => m.EventId == model.EventId &&
          m.UserId == item.UserId && ((m.Part > 0) || (m.Summ < 0)));
        // Если у пользовател указана "часть"
        if (item.DebtPart > 0)        {
          
          // Считаем сумму долга для пользователя
          double realSum = (item.DebtPart * diffSum) / debtPartSum;
          // Если нет пользователя - создаем
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
          // Иначе - изменяем
          else
          {
            debtor.Part = item.DebtPart;
            debtor.Summ = -realSum;
          }
        }
        // Если у пользовател указана "сумма"
        else if (item.DebtSum > 0)
        {
          // Если нет пользователя - создаем
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
          // Иначе - изменяем
          else
          {
            debtor.Part = null;
            debtor.Summ = -item.DebtSum;
          }
        }
      }
      
      // Попытка записи в БД
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
      // Проверка созданнгогог события на корректность сумм
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