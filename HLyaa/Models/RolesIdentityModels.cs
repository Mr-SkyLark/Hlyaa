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
      var role1 = new IdentityRole { Name = "admin" };
      var role2 = new IdentityRole { Name = "user" };

      // добавляем роли в бд
      roleManager.Create(role1);
      roleManager.Create(role2);

      // создаем пользователей
      var adminInfo = new UserInfo { Age = 0, Name = "Галдин Илья", Nick = "admin", BirthdayDate = new DateTime(1992,01,01) };
      var admin = new ApplicationUser { Email = "somemail@mail.ru", UserName = "admin", UserInfo = adminInfo};
      string password = "ad46D_ewr3";
      var result = userManager.Create(admin, password);

      // если создание пользователя прошло успешно
      if (result.Succeeded)
      {
        // добавляем для пользователя роль
        userManager.AddToRole(admin.Id, role1.Name);
        userManager.AddToRole(admin.Id, role2.Name);
      }

      base.Seed(context);
    }
  }
}