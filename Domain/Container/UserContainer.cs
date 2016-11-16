using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLyaa.Domain.Context;
using HLyaa.Domain.Entities;
using HLyaa.Logger;

namespace Domain.Container
{
  class UserContainer
  {
    private static NLogLogger logger = new NLogLogger();
        private static ApplicationDbContext db = new ApplicationDbContext();
    private IAuthenticationManager AuthenticationManager
    {
      get
      {
        return HttpContext.GetOwinContext().Authentication;
      }
    }
    public UserManager<ApplicationUser> UserManager { get; private set; }

    // private static ControllerHelper userHelper = new ControllerHelper(db);

    public UserContainer()
    : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)))
    {
    }
    public UserContainer(UserManager<ApplicationUser> userManager)
    {
      UserManager = userManager;
    }
    public void RegisterUser()
    {
      
    }
    public async void LoginUser(string userName, string password bool Re)
    {
      var user = await UserManager.FindAsync(userName, password);
      if (user != null)
      {
        logger.Info(String.Format("User {0} is logged", userName));
        await SignInAsync(user, model.RememberMe);
        return RedirectToLocal(returnUrl);
      }
      else
      {
        logger.Warn(String.Format("Invalid username or password for user {0}", model.UserName));
        ModelState.AddModelError("", "Не правильный псевдоним или пароль.");
      }
    }
    private async Task SignInAsync(ApplicationUser user, bool isPersistent)
    {
      AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
      var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
      AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
    }
  }
}
