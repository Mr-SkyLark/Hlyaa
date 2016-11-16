using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System;
using HLyaa.Domain.Entities;


namespace HLyaa.Domain.Context
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<UserInfo> UsersInfo { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<DebtPart> DebtParts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<UserEventType> UserEventTypes { get; set; }
    public ApplicationDbContext()
      : base("DefaultConnection", throwIfV1Schema: false)
    {
    }
    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }
  }
}