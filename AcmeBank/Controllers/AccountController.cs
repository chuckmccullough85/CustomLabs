using AcmeBank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcmeBank.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IBankService svc;
        private string Email => User.Identity!.Name!;
        public AccountController(IBankService svc)
        {
            this.svc = svc;
        }


        public async Task<IActionResult> Index()
        {
            // step 1 - validate parameters
            // interact with the business layer
            string user = await svc.GetUserName(Email);
            var accounts = await svc.GetAccounts(Email);
            // create a view model
            var model = new AccountSummaryPageModel(
                               user,
                               accounts);
            // return the view model to the view
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var details_t = svc.GetAccountDetail(Email, id);
            var transactions_t = svc.GetTransactions(Email, id);
            var details = await details_t;
            var transactions = await transactions_t;
            var model = new TransactionModel(details.AcctNum, details.BBal, details.EBal, transactions);
            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            var cust = await svc.GetCustomer(Email);
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
        public async Task<IActionResult> SaveProfile(ProfileModel model)
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
            await svc.SaveCustomer(new CustomerProfile(model.Id, model.Name, model.Email, model.Phone, home, billing));
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Transfer()
        {
            var accounts = await svc.GetAccounts(Email);
            var name = await svc.GetUserName(Email);
            return View(new TransferModel(accounts, name));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DoTransfer(TransferModel model)
        {
            if (!ModelState.IsValid || model.FromAccount == model.ToAccount)
            {
                model.Accounts = (await svc.GetAccounts(Email))
                    .Select(a=>new SelectListItem(a.Text, a.Id.ToString()));
                model.Name = await svc.GetUserName(Email);
                return View(nameof(Transfer), model);
            }

            await svc.Transfer(Email, model.FromAccount, model.ToAccount, model.Amount);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult>  BillPay()
        {
            var model = new BillPayPageModel(
                new BillPayItem(null, DateOnly.FromDateTime(DateTime.Today), 0, ""),
                await GetScheduledBillsHelper());
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task AddBillPayItem(BillPayPageModel item)
        {
            if (ModelState.IsValid)
            {
                await svc.ScheduleBillPay(Email, new ScheduledBillPay(0, 
                    item.NewItem.Scheduled, 
                    item.NewItem.Amount, 
                    item.NewItem.Payee));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteScheduledBillPay(int id)
        {
            await svc.DeleteScheduledBillPay(Email, id);
            return PartialView("_ScheduledBillPay", await GetScheduledBillsHelper());
        }

        public async Task<IActionResult> GetScheduledBills()
        {
            return PartialView("_ScheduledBillPay", await GetScheduledBillsHelper());
        }

        private async Task<IEnumerable<BillPayItem>> GetScheduledBillsHelper()
            => (await svc.GetScheduledBillPayItems(Email)).Select(i =>
                new BillPayItem(i.Id, DateOnly.FromDateTime(i.Scheduled), i.Amount, i.Payee));
 

    }
}
