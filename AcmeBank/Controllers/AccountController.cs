using AcmeBank.Models;

using Microsoft.AspNetCore.Mvc;

namespace AcmeBank.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBankService svc;
        private string email = "hank@propane.com";
        public AccountController(IBankService svc)
        {
            this.svc = svc;
        }


        public IActionResult Index()
        {
            // step 1 - validate parameters
            // interact with the business layer
            string user = svc.GetUserName(email);
            var accounts = svc.GetAccounts(email);
            // create a view model
            var model = new AccountSummaryPageModel(
                               user,
                               accounts);
            // return the view model to the view
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var details = svc.GetAccountDetail(email, id);
            var transactions = svc.GetTransactions(email, id);
            var model = new TransactionModel(details.AcctNum, details.BBal, details.EBal, transactions);
            return View(model);
        }

        public IActionResult Profile()
        {
            var cust = svc.GetCustomer(email);
            AddressViewModel? home = null;
            AddressViewModel? billing = null;
            if (cust.HomeAddress != null)
            {
                home = new (cust.HomeAddress.Street, cust.HomeAddress.City, cust.HomeAddress.State, cust.HomeAddress.ZipCode);
            }
            if (cust.BillingAddress != null)
            {
                billing = new (cust.BillingAddress.Street, cust.BillingAddress.City, cust.BillingAddress.State, cust.BillingAddress.ZipCode);
            }
            var model = new ProfileModel(cust.Id, cust.Name, cust.Email, cust.Phone, home!, billing);
            return View(model);
        }
        public IActionResult SaveProfile(ProfileModel model)
        {
            if (!ModelState.IsValid) return View(nameof(Profile), model);

            Address? home = null;
            Address? billing = null;
            if (model.HomeAddress != null)
            {
                home = new Address()
                {
                    Street = model.HomeAddress.Street,
                    City = model.HomeAddress.City,
                    State = model.HomeAddress.State,
                    ZipCode = model.HomeAddress.ZipCode
                };
            }
            if (model.BillingAddress != null)
            {
                billing = new Address()
                {
                    Street = model.BillingAddress.Street,
                    City = model.BillingAddress.City,
                    State = model.BillingAddress.State,
                    ZipCode = model.BillingAddress.ZipCode
                };
            }
            svc.SaveCustomer(new CustomerProfile(model.Id, model.Name, model.Email, model.Phone, home, billing));


            return RedirectToAction(nameof(Index));
        }
    }
}
