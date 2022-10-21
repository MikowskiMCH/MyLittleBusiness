using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace MyLittleBusiness.Areas.Identity.Data;

// Add profile data for application users by adding properties to the MyLittleBusinesUser class
public class MyLittleBusinessUser : IdentityUser
{
    [DisplayName("Imię")]
    [MaxLength(50, ErrorMessage = "Pole *Imię* może zawierać maxymalnie 50 znaków")]
    public string FirstName { get; set; }

    [DisplayName("Drugie imię")]
    [MaxLength(50, ErrorMessage = "Pole *Drugie imię* może zawierać maxymalnie 50 znaków")]
    public string? SecondName { get; set; }

    [DisplayName("Nazwisko")]
    [MaxLength(50, ErrorMessage = "Pole *Nazwisko* może zawierać maxymalnie 50 znaków")]
    public string Surname { get; set; }

    [DisplayName("Nazwa firmy")]
    [MaxLength(100, ErrorMessage = "Pole *Nazwa firmy może zawierać maxymalnie 100 znaków*")]
    public string ConcernName { get; set; }

    [DisplayName("Nr. NIP")]
    [MaxLength(10, ErrorMessage ="Pole *Nr. NIP* musi zawierać 10 cyfr")]
    [MinLength(10, ErrorMessage ="Pole *Nr. NIP* musi zawierać 10 cyfr")]
    public string Nip { get; set; }
}

