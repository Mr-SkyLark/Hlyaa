using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace HLyaa.Models
{
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
