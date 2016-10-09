﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace HLyaa.Models
{
  public class CheckBoxItem
  {
    public int UserId { get; set; }
    public string Name { get; set; }
    public double Data { get; set; }
    public bool Selected { get; set; }
  }
  public class CreateNewEventModel
  {
    [Required(ErrorMessage = "Введите заголовок события")]
    [Display(Name = "Заголовок события")]
    public string EventName { get; set; }
    public List<CheckBoxItem> CheckBoxDataItems { get; set; }
    public CreateNewEventModel()
    {
      CheckBoxDataItems = new List<CheckBoxItem>();
    }

  }
}
