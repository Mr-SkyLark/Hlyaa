using System.ComponentModel.DataAnnotations;
using System;

namespace HLyaa.Server.Models
{
  public class ExternalLoginConfirmationViewModel
  {
    [Required(ErrorMessage = "Введите Ваш псевдоним")]
    [Display(Name = "Ваш псевдоним")]
    public string UserName { get; set; }
  }

  public class ManageUserViewModel
  {
    [Required(ErrorMessage = "Введите Ваш текущий пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Текущий пароль")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Введите Ваш новый пароль")]
    [StringLength(100, ErrorMessage = " {0} Должен содержать более {2} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Подтвердите пароль")]
    [Compare("NewPassword", ErrorMessage = "Эти пароли не совпадают.")]
    public string ConfirmPassword { get; set; }
  }

  public class LoginViewModel
  {
    [Required(ErrorMessage = "Введите Ваш псевдоним")]
    [Display(Name = "Ваш псевдоним")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Введите Пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Запомнить меня?")]
    public bool RememberMe { get; set; }
  }

  public class RegisterViewModel
  {
    [Required(ErrorMessage = "Введите Ваш псевдоним")]
    [Display(Name = "Ваш псевдоним")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Введите хоть какую-нибудь электронную почту")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Не корректная электронная почта")]
    [Display(Name = "Электронная почта")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(100, ErrorMessage = "{0} Должен содержать более {2} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Подтвердите пароль")]
    [Compare("Password", ErrorMessage = "Эти пароли не совпадают.")]
    public string ConfirmPassword { get; set; }
  }
  public class AddUserInfoViewModel
  {
    [Required(ErrorMessage = "К сожалению это обязательно")]
    [Display(Name = "Фамилия Имя")]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "Псевдоним")]
    public string Nick { get; set; }

    [Required]
    //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата рождения")]
    public DateTime BirthdayDate { get; set; }
  }
}
