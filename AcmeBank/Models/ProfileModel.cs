using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcmeBank.Models;

public enum States
{
  AL, AK, AZ, AR, CA, CO, CT, DE, FL, GA, HI, ID, IL, IN, IA, KS, KY, LA, ME, MD, MA, MI, MN, MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, ND, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY
}


public record AddressViewModel(
    string Street, 
    string City,
    [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "State must be a two-letter abbreviation")]
    string State,
    [property:Display(Name = "Postal Code", Prompt ="12345")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip code must be 5 digits")]
    string ZipCode)
{
    public IEnumerable<SelectListItem> States 
        => Enum.GetValues<States>().Select(s => new SelectListItem(s.ToString(), s.ToString()));
}


public record ProfileModel (
    int Id, 
    string Name,
    [EmailAddress]
    string Email,
    [Phone]
    string Phone, 
    AddressViewModel HomeAddress, 
    AddressViewModel? BillingAddress
);

