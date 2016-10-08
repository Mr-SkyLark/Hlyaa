using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace HLyaa.Models
{
  public class CreateNewEventModel
  {
    public string EventName { get; set; }
    public List<SelectListItem> CheckBoxDataItems { get; set; }
    public Dictionary<int,double> DoubleItems { get; set; }
    public CreateNewEventModel()
    {
      CheckBoxDataItems = new List<SelectListItem>();
      DoubleItems = new Dictionary<int, double>();
    }

  }
}
