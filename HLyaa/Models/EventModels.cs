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
    public double DeptPart { get; set; }
    public double DeptSumm { get; set; }
  }
  public class SetPartModel
  {
    public List<DebtorDataItem> DebtorDataItems { get; set; }
    public SetPartModel()
    {
      DebtorDataItems = new List<DebtorDataItem>();
    }
  }
}
