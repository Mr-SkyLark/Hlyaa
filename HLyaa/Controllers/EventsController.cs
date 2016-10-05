using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HLyaa.Logger;
using HLyaa.Models;

namespace HLyaa.Controllers
{
  public class EventsController : Controller
  {
    private static NLogLogger logger = new NLogLogger();
    ApplicationDbContext db = new ApplicationDbContext();
    // GET: Events
    public ActionResult Index()
    {
      return View();
    }
    public ActionResult CreateEvent()
    {
      return View(db.UsersInfo.ToList());
    }
  }
}