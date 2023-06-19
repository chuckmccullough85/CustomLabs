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
            return View();
        }
    }
}
