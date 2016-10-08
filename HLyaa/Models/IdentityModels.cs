using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System;
using HLyaa.Entities;


namespace HLyaa.Models
{
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