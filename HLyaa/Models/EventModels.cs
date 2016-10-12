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
    [Range(0, 100000, ErrorMessage = "Предел денежного взноса на одну персону: от 0 до 100000") ]
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
    [Range(0, 100000, ErrorMessage = "Предел разумной части задолженности на одну персону: от 0 до 100000")]
    public double DebtPart { get; set; }
    [Range(0, 100000, ErrorMessage = "Предел суммы задолженности на одну персону: от 0 до 100000")]
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
