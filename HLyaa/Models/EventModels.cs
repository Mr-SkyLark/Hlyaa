using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace HLyaa.Models
{
  // CreateEvent
  public class BuyerDataItem
  {
    public int UserId { get; set; }
    public string Name { get; set; }
    public double Data { get; set; }
  }
  public class CreateNewEventModel
  {
    [Required(ErrorMessage = "Введите заголовок события")]
    [Display(Name = "Заголовок события")]
    public string EventName { get; set; }
    public List<BuyerDataItem> BuyerDataItems { get; set; }
    public CreateNewEventModel()
    {
      BuyerDataItems = new List<BuyerDataItem>();
    }
  }
  //SetDebt
  public class DebtorCoiseItem
  {
    public int UserId { get; set; }
    public bool Selected { get; set;}
    public string Name { get; set; }
  }
  public class SetDebtModel
  {
    public int EventId { get; set; }
    public List<DebtorCoiseItem> DebtorCoiseItems { get; set; }
    public SetDebtModel()
    {
      DebtorCoiseItems = new List<DebtorCoiseItem>();
    }
  }
  //SetPartAndPrice
  public class DebtorDataItem
  {
    public int UserId { get; set; }
    public string Name { get; set; }
    public double DebtPart { get; set; }
    public double DebtSum { get; set; }
  }
  public class SetPartModel
  {
    public int EventId { get; set; }
    public List<DebtorDataItem> DebtorDataItems { get; set; }
    public SetPartModel()
    {
      DebtorDataItems = new List<DebtorDataItem>();
    }
  }
}
