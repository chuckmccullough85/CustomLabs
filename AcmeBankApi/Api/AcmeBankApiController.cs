using AcmeLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcmeBankApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcmeBankApiController : ControllerBase
    {
        [HttpGet, Route("getaccounts")]
        public async Task<IActionResult> GetAllAccounts(string email, IBankService svc)
        {
            try
            {
                return Ok(await svc.GetAccounts(email));
            }
            catch (Exception)
            {
                return NotFound();
            }
          
        }

        [HttpGet, Route("getaccountdetail")]
        public async Task<object> GetAccountDetail (string email, int accountId, IBankService svc)
        {
            var data = await svc.GetAccountDetail(email, accountId);
            return new {AccountNum = data.AcctNum, BeginningBalance = data.BBal, EndingBalance = data.EBal};
        }

        [HttpGet, Route("getcustomer")]
        public async Task<CustomerProfile> GetCustomer(string email, IBankService svc) 
            => await svc.GetCustomer(email);

        [HttpPut, Route("savecustomer")]
        public async Task SaveCustomer(CustomerProfile customerProfile, IBankService svc)
            => await svc.SaveCustomer(customerProfile);

        [HttpGet, Route("getcustomername")]
        public async Task<string> GetCustomerName(string email, IBankService svc)
            => await svc.GetUserName(email);

        [HttpGet, Route("gettransactions")]
        public async Task<IEnumerable<TransactionDetail>> GetTransactions(string email, int accountId, IBankService svc)
            => await svc.GetTransactions(email, accountId);

        [HttpPost, Route("transfer")]
        public async Task Transfer(string email, int fromAcct, int toAcct, decimal amount, IBankService svc)
            => await svc.Transfer(email, fromAcct, toAcct, amount);

        [HttpPost, Route("schedulebillpay")]
        public async Task ScheduleBillPay(string email, ScheduledBillPay newItem, IBankService svc)
            => await svc.ScheduleBillPay(email, newItem);
        
        [HttpDelete, Route("deletescheduledbillpay")]
        public async Task DeleteScheduledBillPay(string email, int id, IBankService svc)
            => await svc.DeleteScheduledBillPay(email, id);

        [HttpGet, Route("getscheduledbillpayitems")]
        public async Task<IEnumerable<ScheduledBillPay>> GetScheduledBillPayItems(string email, IBankService svc)
            => await svc.GetScheduledBillPayItems(email);

    }
}
