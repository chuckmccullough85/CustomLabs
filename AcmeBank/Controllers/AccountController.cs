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
            var model = new ProfileModel(1, "Hank Hill", "hank@propane.com", "123-456-7890",
                new AddressViewModel("123 Main St", "Arlen", "TX", "12345"),
                null);
            return View(model);
        }
        public IActionResult SaveProfile(ProfileModel model)
        {
            if (!ModelState.IsValid) return View(nameof(Profile), model);

            return RedirectToAction(nameof(Index));
        }
    }
}
