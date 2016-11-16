using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace HLyaa.Domain.Entities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser
  {

    public virtual ICollection<UserInfo> UserInfo { get; set; }
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      return userIdentity;
    }
  }

  public class UserInfo
  {
    public int Id { get; set; }
    [ForeignKey("ApplicationUserId")]
    public virtual ApplicationUser ApplicationUser { get; set; }
    public string ApplicationUserId { get; set; }    
    public DateTime? BirthdayDate { get; set; }
    public string Name { get; set; }
    public string Nick { get; set; }

    public ICollection<Event> Events { get; set; }
    public ICollection<DebtPart> DebtParts { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<UserEventType> UserEventTypes { get; set; }
    public UserInfo()
    {
      DebtParts = new List<DebtPart>();
      Events = new List<Event>();
      Payments = new List<Payment>();
      UserEventTypes = new List<UserEventType>();
    }

  }
}