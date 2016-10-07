using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace HLyaa.Models
{
  public class Event
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool GodDebt { get; set; }
    public DateTime DateCreated { get; set; }
    public int? ReporterId { get; set; }
    public UserInfo Reporter { get; set; }
    public double Price { get; set; }
    public EventType Type { get; set; }
    public ICollection<DebtPart> DebtParts { get; set; }
    public Event()
    {
      DebtParts = new List<DebtPart>();
    }
  }
  public class DebtPart
  {
    public int Id { get; set; }
    public double Part { get; set; }
    public double Summ { get; set; }
    public bool GlobalFlag { get; set; }
    public int? UserId { get; set; }
    public UserInfo User { get; set; }
    public int? EventId { get; set; }
    public Event Event { get; set; }

    public ICollection<Payment> Payments { get; set; }
    public DebtPart()
    {
      Payments = new List<Payment>();
    }
  }
  public class Payment
  {
    public int Id { get; set; }
    public double Summ { get; set; }
    public bool CompleteFlag { get; set; }
    public int? UserId { get; set; }
    public UserInfo User { get; set; }
    public int DebtId { get; set; }
    public DebtPart Debt { get; set; }
  }
  public class EventType
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<UserInfo> Users { get; set; }
    public ICollection<DebtPart> DebtParts { get; set; }
    public EventType()
    {
      Users = new List<UserInfo>();
      DebtParts = new List<DebtPart>();
    }
  }

  public class CheckBoxDoubleModel
  {
    public List<SelectListItem> CheckBoxDataItems { get; set; }
    public Dictionary<int,double> DoubleItems { get; set; }
    public CheckBoxDoubleModel()
    {
      CheckBoxDataItems = new List<SelectListItem>();
      DoubleItems = new Dictionary<int, double>();
    }

  }
}
