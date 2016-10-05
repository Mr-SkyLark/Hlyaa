using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System;


namespace HLyaa.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser
  {

    public virtual UserInfo UserInfo { get; set; }
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      return userIdentity;
    }
  }

  public class UserInfo
  {

    public int Id { get; set; }
    public int? Age { get; set; }
    public DateTime? BirthdayDate { get; set; }
    public string Name { get; set; }
    public string Nick { get; set; }

    public ICollection<Event> Events { get; set; }
    public ICollection<DebtPart> DebtParts { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public virtual ICollection<EventType> Types { get; set; }
    public UserInfo()
    {
      DebtParts = new List<DebtPart>();
      Events = new List<Event>();
      Payments = new List<Payment>();
      Types = new List<EventType>();
    }

  }
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<UserInfo> UsersInfo { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<DebtPart> DebtParts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public ApplicationDbContext()
      : base("DefaultConnection", throwIfV1Schema: false)
    {
    }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<EventType>().HasMany(c => c.Users)
          .WithMany(s => s.Types)
          .Map(t => t.MapLeftKey("TypeId")
            .MapRightKey("UserId")
            .ToTable("UserType"));

      base.OnModelCreating(modelBuilder);
    }
    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }
  }
}