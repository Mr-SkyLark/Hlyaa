using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HLyaa.Server.Helper
{
  /// <summary>
  /// Класс перевода строки списка чисел в контейнер чисел
  /// </summary>
  public class IntListModelBinder : DefaultModelBinder
  {
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
      if (value == null || string.IsNullOrEmpty(value.AttemptedValue))
      {
        return null;
      }

      return value
          .AttemptedValue
          .Split(',')
          .Select(int.Parse)
          .ToList();
    }
  }
}