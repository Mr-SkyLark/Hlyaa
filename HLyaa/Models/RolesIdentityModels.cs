using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;
using HLyaa.Entities;

namespace HLyaa.Models
{
  /*public class ApplicationUserManager : UserManager<ApplicationUser>
  {
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
      : base(store)
    {
    }
    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
                                            IOwinContext context)
    {
      ApplicationDbContext db = context.Get<ApplicationDbContext>();
      ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
      return manager;
    }
  }*/
  public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
  {
    protected override void Seed(ApplicationDbContext context)
    {
      var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

      var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

      // создаем две роли
      var adminRrole = new IdentityRole { Name = "Администратор" };
      var bankManRole = new IdentityRole { Name = "Банкир" };
      var userRole = new IdentityRole { Name = "Пользователь" };
      var studentRole = new IdentityRole { Name = "Студент" };

      // добавляем роли в бд
      roleManager.Create(adminRrole);
      roleManager.Create(bankManRole);
      roleManager.Create(userRole);
      roleManager.Create(studentRole);

      // создаем пользователей
      var adminInfo = new UserInfo { Age = 0, Name = "Галдин Илья", Nick = "admin", BirthdayDate = new DateTime(1992, 01, 01) };
      var admin = new ApplicationUser { Email = "somemail@mail.ru", UserName = "admin", UserInfo = adminInfo };
      string password = "ad46D_ewr3";
      var result = userManager.Create(admin, password);

      // если создание пользователя прошло успешно
      if (result.Succeeded)
      {
        // добавляем для пользователя роль
        userManager.AddToRole(admin.Id, adminRrole.Name);
        userManager.AddToRole(admin.Id, bankManRole.Name);
        userManager.AddToRole(admin.Id, userRole.Name);
      }

      // создаем пользователей
      adminInfo = new UserInfo { Age = 0, Name = "11111 11111", Nick = "1", BirthdayDate = new DateTime(1992, 01, 01) };
      admin = new ApplicationUser { Email = "1@1.ru", UserName = "1", UserInfo = adminInfo };
      password = "11111111";
      result = userManager.Create(admin, password);

      // если создание пользователя прошло успешно
      if (result.Succeeded)
      {
        // добавляем для пользователя роль
        userManager.AddToRole(admin.Id, userRole.Name);
      }

      // создаем пользователей
      adminInfo = new UserInfo { Age = 0, Name = "222 22222", Nick = "2", BirthdayDate = new DateTime(1992, 01, 01) };
      admin = new ApplicationUser { Email = "2@2.ru", UserName = "2", UserInfo = adminInfo };
      password = "22222222";
      result = userManager.Create(admin, password);

      // если создание пользователя прошло успешно
      if (result.Succeeded)
      {
        // добавляем для пользователя роль
        userManager.AddToRole(admin.Id, userRole.Name);
        userManager.AddToRole(admin.Id, studentRole.Name);
      }

      // создаем пользователей
      adminInfo = new UserInfo { Age = 0, Name = "33333 33333", Nick = "3", BirthdayDate = new DateTime(1992, 01, 01) };
      admin = new ApplicationUser { Email = "3@3.ru", UserName = "3", UserInfo = adminInfo };
      password = "33333333";
      result = userManager.Create(admin, password);

      // если создание пользователя прошло успешно
      if (result.Succeeded)
      {
        // добавляем для пользователя роль
        userManager.AddToRole(admin.Id, userRole.Name);
      }

      // создаем пользователей
      adminInfo = new UserInfo { Age = 0, Name = "4444 44444", Nick = "4", BirthdayDate = new DateTime(1992, 01, 01) };
      admin = new ApplicationUser { Email = "4@4.ru", UserName = "4", UserInfo = adminInfo };
      password = "44444444";
      result = userManager.Create(admin, password);

      // если создание пользователя прошло успешно
      if (result.Succeeded)
      {
        // добавляем для пользователя роль
        userManager.AddToRole(admin.Id, userRole.Name);
      }

      // создаем пользователей
      adminInfo = new UserInfo { Age = 0, Name = "555555 55555", Nick = "5", BirthdayDate = new DateTime(1992, 01, 01) };
      admin = new ApplicationUser { Email = "5@5.ru", UserName = "5", UserInfo = adminInfo };
      password = "55555555";
      result = userManager.Create(admin, password);

      // если создание пользователя прошло успешно
      if (result.Succeeded)
      {
        // добавляем для пользователя роль
        userManager.AddToRole(admin.Id, userRole.Name);
      }

      base.Seed(context);
    }
  }
}