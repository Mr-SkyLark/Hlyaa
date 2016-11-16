using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using HLyaa.Server.Models;
using HLyaa.Server.Helper;
using HLyaa.Domain.Entities;
using HLyaa.Domain.Context;
using HLyaa.Logger;

namespace HLyaa.Server.Controllers
{

  [Authorize]
  public class AccountController : Controller
  {
    private static NLogLogger logger = new NLogLogger();

    //private static ControllerHelper userHelper = new ControllerHelper(db);

    public AccountController()
    {
    }
    //
    // GET: /Account/Login
    [AllowAnonymous]
    public ActionResult Login(string returnUrl)
    {
      ViewBag.ReturnUrl = returnUrl;
      return View();
    }

    //
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    {
      if (ModelState.IsValid)
      {
        var user = await UserManager.FindAsync(model.UserName, model.Password);
        if (user != null)
        {
          logger.Info(String.Format("User {0} is logged", model.UserName));
          await SignInAsync(user, model.RememberMe);
          return RedirectToLocal(returnUrl);          
        }
        else
        {
          logger.Warn(String.Format("Invalid username or password for user {0}", model.UserName));
          ModelState.AddModelError("", "Не правильный псевдоним или пароль.");
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    //
    // GET: /Account/Register
    [AllowAnonymous]
    public ActionResult Register()
    {
      return View();
    }

    //
    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
      if (ModelState.IsValid)
      {
        logger.Info(String.Format("User {0} is registred", model.UserName));
        var userInfo = new UserInfo() { Nick = model.UserName };
        var user = new ApplicationUser() { UserName = model.UserName, Email = model.EmailAddress,
          UserInfo = new List<UserInfo>() { userInfo }
        };
        var result = await UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          await UserManager.AddToRoleAsync(user.Id, "user");
          await SignInAsync(user, isPersistent: false);          
          return RedirectToAction("AddUserInfo", "Account");
          //identity.AddClaim(ClaimTypes.NameIdentifier, user.Id);
        }
        else
        {
          AddErrors(result);
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    //
    // GET: /Account/AddUserInfo
    public ActionResult AddUserInfo()
    {
      var curUser = userHelper.CurrentUserInfo();
      if (curUser == null)
      {
        return HttpNotFound();
      }
      AddUserInfoViewModel viewModel = new AddUserInfoViewModel { Nick = curUser.Nick };
      return View(viewModel);
    }

    //
    // POST: /Account/AddUserInfo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddUserInfo(AddUserInfoViewModel viewModel)
    {
      var curUser = userHelper.CurrentUserInfo();
      if (curUser == null)
      {
        return HttpNotFound();
      }
      curUser.BirthdayDate = viewModel.BirthdayDate;
      curUser.Name = viewModel.UserName;
      curUser.Nick = viewModel.Nick;
      db.Entry(curUser).State = EntityState.Modified;
      db.SaveChanges();

      // If we got this far, something failed, redisplay form
      return RedirectToAction("Index", "Home");
    }

    //
    // POST: /Account/Disassociate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
    {
      ManageMessageId? message = null;
      IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
      if (result.Succeeded)
      {
        message = ManageMessageId.RemoveLoginSuccess;
      }
      else
      {
        message = ManageMessageId.Error;
      }
      return RedirectToAction("Manage", new { Message = message });
    }

    //
    // GET: /Account/Manage
    public ActionResult Manage(ManageMessageId? message)
    {
      ViewBag.StatusMessage =
        message == ManageMessageId.ChangePasswordSuccess ? "Пароль был успешно изменён."
        : message == ManageMessageId.SetPasswordSuccess ? "Пароль был успешно установлен."
        : message == ManageMessageId.RemoveLoginSuccess ? "Внешняя учётная запись была удалена."
        : message == ManageMessageId.Error ? "Произошла непредвиденная ошибка."
        : "";
      ViewBag.HasLocalPassword = HasPassword();
      ViewBag.ReturnUrl = Url.Action("Manage");
      return View();
    }

    //
    // POST: /Account/Manage
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Manage(ManageUserViewModel model)
    {
      bool hasPassword = HasPassword();
      ViewBag.HasLocalPassword = hasPassword;
      ViewBag.ReturnUrl = Url.Action("Manage");
      if (hasPassword)
      {
        if (ModelState.IsValid)
        {
          IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
          if (result.Succeeded)
          {
            return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
          }
          else
          {
            AddErrors(result);
          }
        }
      }
      else
      {
        // User does not have a password so remove any validation errors caused by a missing OldPassword field
        ModelState state = ModelState["OldPassword"];
        if (state != null)
        {
        state.Errors.Clear();
        }

        if (ModelState.IsValid)
        {
          IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
          if (result.Succeeded)
          {
            return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
          }
          else
          {
            AddErrors(result);
          }
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    //
    // POST: /Account/ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public ActionResult ExternalLogin(string provider, string returnUrl)
    {
    // Request a redirect to the external login provider
      return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
    }

    //
    // GET: /Account/ExternalLoginCallback
    [AllowAnonymous]
    public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
    {
      var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
      if (loginInfo == null)
      {
        return RedirectToAction("Login");
      }

      // Sign in the user with this external login provider if the user already has a login
      var user = await UserManager.FindAsync(loginInfo.Login);
      if (user != null)
      {
        await SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);
      }
      else
      {
        // If the user does not have an account, then prompt the user to create an account
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
      }
    }

    //
    // POST: /Account/LinkLogin
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LinkLogin(string provider)
    {
    // Request a redirect to the external login provider to link a login for the current user
      return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
    }

    //
    // GET: /Account/LinkLoginCallback
    public async Task<ActionResult> LinkLoginCallback()
    {
      var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
      if (loginInfo == null)
      {
        return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
      }
      var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
      if (result.Succeeded)
      {
        return RedirectToAction("Manage");
      }
      return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
    }

    //
    // POST: /Account/ExternalLoginConfirmation
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
    {
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Manage");
      }

      if (ModelState.IsValid)
      {
        // Get the information about the user from the external login provider
        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
          return View("ExternalLoginFailure");
        }
        var user = new ApplicationUser() { UserName = model.UserName };
        var result = await UserManager.CreateAsync(user);
        if (result.Succeeded)
        {
          result = await UserManager.AddLoginAsync(user.Id, info.Login);
          if (result.Succeeded)
          {
            await SignInAsync(user, isPersistent: false);
            return RedirectToLocal(returnUrl);
          }
        }
        AddErrors(result);
      }

      ViewBag.ReturnUrl = returnUrl;
      return View(model);
    }
    //
    // GET: /Account/LogOut
    public ActionResult LogOut()
    {
      AuthenticationManager.SignOut();
      return RedirectToAction("Index", "Home");
    }
    //
    // POST: /Account/LogOff
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LogOff()
    {
      AuthenticationManager.SignOut();
      return RedirectToAction("Index", "Home");
    }

    //
    // GET: /Account/ExternalLoginFailure
    [AllowAnonymous]
    public ActionResult ExternalLoginFailure()
    {
      return View();
    }

    [ChildActionOnly]
    public ActionResult RemoveAccountList()
    {
      var uId = User.Identity.GetUserId();
      if (uId == null || UserManager.FindById(uId) ==null)
      {
        logger.Error(String.Format("uId = null"));
        return RedirectToAction("LogOut", "Account");
      }
      var linkedAccounts = UserManager.GetLogins(uId);
      ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
      return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && UserManager != null)
      {
        UserManager.Dispose();
        UserManager = null;
      }
      base.Dispose(disposing);
    }

    #region Helpers
    // Used for XSRF protection when adding external logins
    private const string XsrfKey = "XsrfId";

    private IAuthenticationManager AuthenticationManager
    {
      get
      {
        return HttpContext.GetOwinContext().Authentication;
      }
    }

    private async Task SignInAsync(ApplicationUser user, bool isPersistent)
    {
      AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
      var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
      AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError("", error);
      }
    }

    private bool HasPassword()
    {
      var user = UserManager.FindById(User.Identity.GetUserId());
      if (user != null)
      {
        return user.PasswordHash != null;
      }
      return false;
    }

    public enum ManageMessageId
    {
      ChangePasswordSuccess,
      SetPasswordSuccess,
      RemoveLoginSuccess,
      Error
    }

    private ActionResult RedirectToLocal(string returnUrl)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      else
      {
        return RedirectToAction("Index", "Home");
      }
    }

    private class ChallengeResult : HttpUnauthorizedResult
    {
      public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
      {
      }

      public ChallengeResult(string provider, string redirectUri, string userId)
      {
        LoginProvider = provider;
        RedirectUri = redirectUri;
        UserId = userId;
      }

      public string LoginProvider { get; set; }
      public string RedirectUri { get; set; }
      public string UserId { get; set; }

      public override void ExecuteResult(ControllerContext context)
      {
        var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
        if (UserId != null)
        {
          properties.Dictionary[XsrfKey] = UserId;
        }
        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
      }
    }
    #endregion
  }
}