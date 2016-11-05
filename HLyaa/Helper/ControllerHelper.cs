using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using HLyaa.Models;
using HLyaa.Entities;
using HLyaa.Logger;

namespace HLyaa.Helper
{
  public class ControllerHelper
  {
    private static ApplicationDbContext UsersContext;
    private static UserManager<ApplicationUser> UserManager;
    private static NLogLogger logger = new NLogLogger();
    public ControllerHelper(ApplicationDbContext context)
    {
      UsersContext = context;
      UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UsersContext));
    }
    public List<string> getRolesByUserInfoId(int userInfoId)
    {
      var userInfo =  UsersContext.UsersInfo.ToList().FirstOrDefault(m => m.Id == userInfoId);
      IList<string> roles = new List<string> { "Роль не определена" };
      ApplicationUser user = userInfo.ApplicationUser;
      if (user != null)
        roles = UserManager.GetRoles(user.Id);
      return roles.ToList();
    }

    public List<UserInfo> getUserInfoList()
    {
      return UsersContext.UsersInfo.ToList();
    }
    public UserInfo CurrentUserInfo()
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
      return curUser.ToList().First();
    }      

  }
}