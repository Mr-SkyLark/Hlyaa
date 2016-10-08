using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using HLyaa.Models;
using HLyaa.Entities;
using HLyaa.Logger;

namespace HLyaa.Helper
{
  public class ControllerHelper
  {
    ControllerHelper()
    {
    }
    public static UserInfo CurrentUserInfo(ApplicationDbContext db, UserManager<ApplicationUser> UserManager)
    {
      var uId = HttpContext.Current.User.Identity.GetUserId();
      if (uId == null)
      {
        logger.Error(String.Format("uId = null"));
        return null;
      }
      var curUser = UserManager.FindById(uId).UserInfo;
      if (curUser == null)
      {
        logger.Warn(String.Format("User = null"));
        return null;
      }
      return curUser;
    }

    private static NLogLogger logger = new NLogLogger();    

  }
}