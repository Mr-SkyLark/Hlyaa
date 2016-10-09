using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace HLyaa.Models
{
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
}
